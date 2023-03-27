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
using Godot;
using SunriseMono.Kaitai;

#endregion

namespace SunriseMono.NULib.Decompiler;

public abstract record Edge
{
    public virtual ConstantPoolUpdate ToConstantPoolUpdate()
        => new ConstantPoolUpdateUnknown();
};

public record ActionEdge : Edge
{
    public Avm1.Op Action;

    public override ConstantPoolUpdate ToConstantPoolUpdate()
        => Action.Opcode switch
        {
            Avm1.Opcode.ConstantPool => throw new NotImplementedException(),
            _ => new ConstantPoolUpdateNone(),
        };
}

public record ExprEdge : Edge
{
    public PartialExpr Expression;
}

public record IfTrueEdge : Edge
{
    public override ConstantPoolUpdate ToConstantPoolUpdate()
        => new ConstantPoolUpdateNone();
}

public record IfFalseEdge : Edge
{
    public override ConstantPoolUpdate ToConstantPoolUpdate()
        => new ConstantPoolUpdateNone();
}

public record IfTestEdge : Edge
{
    public override ConstantPoolUpdate ToConstantPoolUpdate()
        => new ConstantPoolUpdateNone();
}

public record VariableDefinitionMarker
{
    public string Name { get; init; }
}

public record MarkerEdge : Edge
{
    public VariableDefinitionMarker Marker { get; init; }
}

public record SimpleEdge : Edge
{
    public override ConstantPoolUpdate ToConstantPoolUpdate()
        => new ConstantPoolUpdateNone();
}

public record SubCfgEdge : Edge;

public record ConditionalEdge : Edge
{
    public PartialExpr Test { get; init; }
    public ControlFlowGraph IfTrue { get; init; }
    public ControlFlowGraph IfFalse { get; init; }
}

public abstract record ConstantPoolUpdate;

public record ConstantPoolUpdateNone : ConstantPoolUpdate;

public record ConstantPoolUpdateUnknown : ConstantPoolUpdate;

public record ConstantPoolUpdateSet(string[] Value) : ConstantPoolUpdate;
