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

using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace SunriseMono.NULib.Decompiler;

public static class ChainReductionAnalyzer
{
    public static ControlFlowGraph AnalyzeChains(this ControlFlowGraph controlFlowGraph)
    {
        foreach (var chain in controlFlowGraph.Chains)
            if (chain.TryReduceChain(out var newEdges))
                controlFlowGraph.ReplaceChain(chain, newEdges);

        return controlFlowGraph;
    }

    public static bool TryReduceChain(
        this List<SimpleNode> chain,
        [MaybeNullWhen(false)] out List<Edge> newEdges
    )
    {
        newEdges = new List<Edge>();
        var previous = chain[0].Out.Edge;
        var changed = false;

        for (var i = 1; i < chain.Count; i++)
        {
            var edge = chain[i].Out.Edge;
            if (previous.TryMergeEdges(edge, out var mergedEdge))
            {
                changed = true;
                previous = mergedEdge;
            }
            else
            {
                newEdges.Add(previous);
                previous = edge;
            }
            if (i == chain.Count - 1)
                newEdges.Add(previous);
        }

        return changed;
    }

    public static bool TryMergeEdges(
        this Edge first,
        Edge second,
        [MaybeNullWhen(false)] out Edge mergedEdge
    )
    {
        if (first is SimpleEdge)
        {
            mergedEdge = second;
            return true;
        }

        if (second is SimpleEdge)
        {
            mergedEdge = first;
            return true;
        }
        if (first is ExprEdge firstExpr && second is ExprEdge secondExpr)
        {
            if (firstExpr.Expression.TryMergeWith(secondExpr.Expression, out var merged))
            {
                mergedEdge = merged.ToEdge();
                return true;
            }
        }

        mergedEdge = null;
        return false;
    }
}
