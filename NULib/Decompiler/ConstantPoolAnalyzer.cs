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

using System;
using System.Collections.Generic;
using System.Linq;

namespace SunriseMono.NULib.Decompiler;

public static class ConstantPoolAnalyzer
{
    private static Dictionary<Node, ConstantPool> AnalyzeConstantPool(this ControlFlowGraph controlFlowGraph)
    {
        var pools = controlFlowGraph.Nodes.ToDictionary<Node, Node, ConstantPool>(
            node => node,
            _ => new ConstantPool(),
            ReferenceEqualityComparer.Instance
        );
        
        while (true)
        {
            var hasChanged = false;
            foreach (var node in controlFlowGraph.Nodes)
            {
                var joined = new ConstantPool();
                foreach (var edge in controlFlowGraph.GetInEdges(node))
                {
                    joined.UnionWith(pools[edge.From].Transfer(edge.Edge));
                }

                if (joined.SetEquals(pools[node])) continue;
                pools[node] = joined;
                hasChanged = true;
            }

            if (hasChanged) return pools;
        }
    }

    public static ControlFlowGraph ReduceConstantPool(this ControlFlowGraph controlFlowGraph)
    {
        var pools = controlFlowGraph.AnalyzeConstantPool();

        foreach (var edge in controlFlowGraph.Edges)
        {
            if (edge.From is not SimpleNode) continue;
            if (!pools.TryGetValue(edge.From, out var state)) continue;
        }

        throw new NotImplementedException();
    }
}

public class ConstantPool : HashSet<Node[]>, IEquatable<ConstantPool>
{
    public ConstantPoolState State => Count switch
    {
        0 => ConstantPoolState.Uninitialized,
        1 => ConstantPoolState.Any,
        _ => ConstantPoolState.Initialized,
    };

    public ConstantPool() : base(ReferenceEqualityComparer.Instance)
    {
    }

    public bool Equals(ConstantPool other)
    {
        if (ReferenceEquals(null, other)) return false;
        if (ReferenceEquals(this, other)) return true;
        return State == other.State && SetEquals(other);
    }
    
    public ConstantPool Transfer(Edge edge)
    {
        return edge.ToConstantPoolUpdate() switch
        {
            ConstantPoolUpdateSet => throw new NotImplementedException(),
            ConstantPoolUpdateNone => this,
            ConstantPoolUpdateUnknown => new ConstantPool(),
            _ => throw new NotImplementedException(),
        };
    }
}

public enum ConstantPoolState
{
    Any,
    Uninitialized,
    Initialized,
}
