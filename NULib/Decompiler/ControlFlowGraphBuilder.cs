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

using System.Collections.Generic;
using System.Linq;
using SunriseMono.Kaitai;

#endregion

namespace SunriseMono.NULib.Decompiler;

public class ControlFlowGraphBuilder
{
    private readonly SimpleNode Source = new();

    public ControlFlowGraph FromAvm1(List<Avm1.Op> avm1)
    {
        var startOffset = avm1[0].Offset.Value;
        var offsetToOp = avm1.ToDictionary(it => it.Offset.Value);
        var offsetToNode = new Dictionary<long, SimpleNode> { [startOffset] = Source };
        var incompleteNodes = new HashSet<SimpleNode>(ReferenceEqualityComparer.Instance)
        {
            Source
        };

        var openSet = new Stack<long>();
        openSet.Push(startOffset);

        while (openSet.Count > 0)
        {
            var currentOffset = openSet.Pop();
            var currentNode = offsetToNode[currentOffset];
            var currentOp = offsetToOp[currentOffset];

            void JumpTo(Avm1.Op op, SimpleNode node)
            {
                if (offsetToNode.TryGetValue(op.Offset.Value, out var target))
                {
                    AddSimpleEdge(node, target);
                    incompleteNodes.Remove(node);
                }
                else
                {
                    openSet.Push(op.Offset.Value);
                    offsetToNode[op.Offset.Value] = node;
                    incompleteNodes.Add(node);
                }
            }

            // ReSharper disable once SwitchStatementHandlesSomeKnownEnumValuesWithDefault
            switch (currentOp.Opcode)
            {
                case Avm1.Opcode.JumpEqual:
                {
                    var (ifTrueNode, ifFalseNode) = AppendIf(currentNode);
                    incompleteNodes.Remove(currentNode);

                    if (
                        offsetToOp.TryGetValue(
                            ((Avm1.JumpData)currentOp.Action).JumpTargetOffset,
                            out var jumpOp
                        )
                    )
                        JumpTo(jumpOp, ifTrueNode);
                    else
                        AppendEndToTrueBranch(currentNode);

                    if (offsetToOp.TryGetValue(currentOp.OffsetNext.Value, out var nextOp))
                        JumpTo(nextOp, ifFalseNode);
                    else
                        AppendEndToFalseBranch(currentNode);
                    break;
                }
                case Avm1.Opcode.Jump:
                {
                    incompleteNodes.Remove(currentNode);
                    if (
                        offsetToOp.TryGetValue(
                            ((Avm1.JumpData)currentOp.Action).JumpTargetOffset,
                            out var jumpOp
                        )
                    )
                        JumpTo(jumpOp, AppendActionEdge(currentNode, jumpOp));
                    else
                        AppendEnd(currentNode);
                    break;
                }
                default:
                {
                    incompleteNodes.Remove(currentNode);
                    if (offsetToOp.TryGetValue(currentOp.OffsetNext.Value, out var nextOp))
                        JumpTo(nextOp, AppendActionEdge(currentNode, currentOp));
                    else
                        AppendEnd(currentNode);
                    break;
                }
            }
        }

        foreach (var incompleteNode in incompleteNodes)
            AppendEnd(incompleteNode);

        return Build();
    }

    public ControlFlowGraph BuildLinear(IEnumerable<Edge> edges)
    {
        AppendEnd(edges.Aggregate(Source, AppendEdge));
        return Build();
    }

    private ControlFlowGraph Build()
    {
        return new(Source);
    }

    private static void AddSimpleEdge(SimpleNode from, Node To)
    {
        from.Out = new HalfBoundEdge { To = To, Edge = new SimpleEdge() };
    }

    private static SimpleNode AppendActionEdge(SimpleNode from, Avm1.Op action)
    {
        return AppendEdge(from, new ActionEdge { Action = action });
    }

    private static EndNode AppendEnd(SimpleNode from)
    {
        var endNode = new EndNode();
        from.Out = new HalfBoundEdge { To = endNode, Edge = new SimpleEdge() };
        return endNode;
    }

    private static EndNode AppendEndToTrueBranch(SimpleNode from)
    {
        var endNode = new EndNode();
        ((IfNode)from.Out.To).OutTrue.To = endNode;
        return endNode;
    }

    private static EndNode AppendEndToFalseBranch(SimpleNode from)
    {
        var endNode = new EndNode();
        ((IfNode)from.Out.To).OutFalse.To = endNode;
        return endNode;
    }

    private static (SimpleNode ifFalseNode, SimpleNode ifTrueNode) AppendIf(SimpleNode from)
    {
        var ifTrueNode = new SimpleNode();
        var ifFalseNode = new SimpleNode();

        from.Out = new HalfBoundEdge
        {
            To = new IfNode
            {
                OutTrue = new HalfBoundEdge { To = ifTrueNode, Edge = new IfTrueEdge() },
                OutFalse = new HalfBoundEdge { To = ifFalseNode, Edge = new IfFalseEdge() }
            },
            Edge = new IfTestEdge()
        };

        return (ifFalseNode, ifTrueNode);
    }

    private static SimpleNode AppendEdge(SimpleNode from, Edge edge)
    {
        var to = new SimpleNode();
        from.Out = new HalfBoundEdge { To = to, Edge = edge };

        return to;
    }
}
