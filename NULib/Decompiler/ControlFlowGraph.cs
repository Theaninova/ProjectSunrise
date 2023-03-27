// Copyright (C) 2023  Sunrise Project
//
// This program is free software: you can redistribute it and/or modify
// it under the terms of the GNU Affero General Public License as
// published by the Free Software Foundation, either version 3 of the
// License, or (at your option) any later version.
//
// This program is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU Affero General Public License for more details.
//
// You should have received a copy of the GNU Affero General Public License
// along with this program.  If not, see <http://www.gnu.org/licenses/>.

#region

using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata;

#endregion

namespace SunriseMono.NULib.Decompiler;

public class ControlFlowGraph
{
    public HashSet<string[]> InConstants;
    private Dictionary<Node, Dictionary<Node, Edge>> InEdges;
    private SimpleNode Source;

    public ControlFlowGraph(SimpleNode source)
    {
        var inEdges = new Dictionary<Node, Dictionary<Node, Edge>>(
            ReferenceEqualityComparer.Instance
        );
        var visitedState = new Dictionary<Node, bool>();
        var stack = new Stack<Node>();
        stack.Push(source);

        while (stack.Count > 0)
        {
            var from = stack.Pop();
            visitedState[from] = true;

            foreach (var edge in from.OutEdges)
            {
                if (!visitedState.ContainsKey(edge.To))
                {
                    stack.Push(edge.To);
                    visitedState[edge.To] = false;
                }

                var curInEdges = inEdges.GetValueOrDefault(edge.To, null);
                if (curInEdges == null)
                {
                    curInEdges = new Dictionary<Node, Edge>(ReferenceEqualityComparer.Instance);
                    inEdges[edge.To] = curInEdges;
                }

                curInEdges[edge.From] = edge.Edge;
            }
        }

        Source = source;
        InEdges = inEdges;
        InConstants = null;
    }

    public void ReplaceChain(List<SimpleNode> chain, List<Edge> edges)
    {
        throw new NotImplementedException();
    }

    public IEnumerable<BoundEdge> Edges
    {
        get
        {
            var visitedNodes = new HashSet<Node>(ReferenceEqualityComparer.Instance);
            var stack = new Stack<Node>();
            stack.Push(Source);
            while (stack.TryPop(out var from))
            {
                visitedNodes.Add(from);
                foreach (var edge in from.OutEdges)
                {
                    if (!visitedNodes.Contains(edge.To))
                    {
                        stack.Push(edge.To);
                        visitedNodes.Add(edge.To);
                    }

                    yield return edge;
                }
            }
        }
    }
    
    public IEnumerable<List<SimpleNode>> Chains
    {
        get
        {
            var visitedNodes = new HashSet<Node>(ReferenceEqualityComparer.Instance);
            var stack = new Stack<Node>();
            stack.Push(Source);

            while (stack.TryPop(out var current))
            {
                visitedNodes.Add(current);
                if (current is SimpleNode)
                {
                    var chain = new List<SimpleNode>();
                    while (current is SimpleNode node)
                    {
                        chain.Add(node);
                        visitedNodes.Add(node);
                        if (GetInEdgeCount(node.Out.To) != 1) break;
                        current = node.Out.To;
                    }

                    yield return chain;
                }

                foreach (var edge in current.OutEdges)
                {
                    if (visitedNodes.Contains(edge.To)) continue;
                    stack.Push(edge.To);
                    visitedNodes.Add(edge.To);
                }
            }
        }
    }

    public int GetInEdgeCount(Node to) => InEdges.TryGetValue(to, out var inEdges) ? inEdges.Count : 0;

    public IEnumerable<BoundEdge> GetInEdges(Node to)
    {
        if (!InEdges.TryGetValue(to, out var inEdges)) yield break;
        foreach (var (node, edge) in inEdges)
        {
            yield return new BoundEdge
            {
                From = node,
                To = to,
                Edge = edge,
            };
        }
    }

    public IEnumerable<Node> Nodes => EnumerateNodes();
    public IEnumerable<Node> NodesPreorder => EnumerateNodesPreorder();
    public IEnumerable<Node> NodesPostorder => EnumerateNodesPostorder();

    public IEnumerable<Node> EnumerateNodes(TraversalOrder traversalOrder = TraversalOrder.ReversePostorderDfs)
    {
        return traversalOrder switch
        {
            TraversalOrder.PreorderDfs => EnumerateNodesPreorder(),
            TraversalOrder.PostorderDfs => EnumerateNodesPostorder(),
            TraversalOrder.ReversePostorderDfs => EnumerateNodesPostorder().Reverse(),
            _ => throw new ArgumentOutOfRangeException(nameof(traversalOrder), traversalOrder, null)
        };
    }

    private IEnumerable<Node> EnumerateNodesPreorder()
    {
        var stack = new Stack<Node>();
        stack.Push(Source);
        var known = new HashSet<Node>(ReferenceEqualityComparer.Instance);

        while (stack.TryPop(out var from))
        {
            yield return from;
            foreach (var to in from.OutEdges.Reverse())
            {
                if (known.Contains(to.To)) continue;
                stack.Push(to.To);
                known.Add(to.To);
            }
        }
    }

    private IEnumerable<Node> EnumerateNodesPostorder()
    {
        var stack = new Stack<Node>();
        stack.Push(Source);
        var discoveredOutEdges = new Dictionary<Node, int>(ReferenceEqualityComparer.Instance) { [Source] = 0 };

        bool NavigateNext(Node from, Node next, int stage)
        {
            if (discoveredOutEdges[from] != stage) return false;
            discoveredOutEdges[from] += 1;
            if (!discoveredOutEdges.TryAdd(next, 0)) return false;
            stack.Push(next);
            return true;
        }

        while (stack.TryPeek(out var from))
        {
            switch (from)
            {
                case EndNode:
                    yield return stack.Pop();
                    break;
                case SimpleNode simpleNode:
                    if (NavigateNext(simpleNode, simpleNode.Out.To, 0)) continue;
                    yield return stack.Pop();
                    break;
                case IfNode ifNode:
                    if (NavigateNext(ifNode, ifNode.OutTrue.To, 0)) continue;
                    if (NavigateNext(ifNode, ifNode.OutFalse.To, 1)) continue;
                    yield return stack.Pop();
                    break;
                default:
                    throw new Exception($"Unknown Node type {stack.Peek()}");
            }
        }
    }
}

public enum TraversalOrder
{
    PreorderDfs,
    PostorderDfs,
    ReversePostorderDfs,
}