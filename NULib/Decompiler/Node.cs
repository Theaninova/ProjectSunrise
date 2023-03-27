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

#endregion

namespace SunriseMono.NULib.Decompiler;

public abstract record Node
{
    public HashSet<string[]> Constants;
    public dynamic Stack;

    public virtual IEnumerable<BoundEdge> OutEdges
    {
        get { yield break; }
    }

    public virtual IEnumerable<BoundEdge> InEdges
    {
        get { yield break; }
    }
}

public record EndNode : Node;

public record IfNode : Node
{
    public HalfBoundEdge OutFalse;
    public HalfBoundEdge OutTrue;

    public override IEnumerable<BoundEdge> OutEdges
    {
        get
        {
            yield return new BoundEdge
            {
                From = this,
                Edge = OutFalse.Edge,
                To = OutFalse.To
            };
            yield return new BoundEdge
            {
                From = this,
                Edge = OutTrue.Edge,
                To = OutTrue.To
            };
        }
    }
}

public record SimpleNode : Node
{
    public HalfBoundEdge Out;

    public override IEnumerable<BoundEdge> OutEdges
    {
        get
        {
            yield return new BoundEdge
            {
                From = this,
                Edge = Out.Edge,
                To = Out.To
            };
        }
    }
}

public record HalfBoundEdge
{
    public Edge Edge;
    public Node To;
}

public record BoundEdge : HalfBoundEdge
{
    public Node From;
}