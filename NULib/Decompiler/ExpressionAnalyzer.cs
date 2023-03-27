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

#nullable enable
using System;
using System.Collections.Generic;
using System.Linq;
using SunriseMono.Kaitai;

namespace SunriseMono.NULib.Decompiler;

public static class ExpressionAnalyzer
{
    public static ControlFlowGraph AnalyzeExpressions(this ControlFlowGraph controlFlowGraph)
    {
        var replacements = new Dictionary<SimpleNode, ExprEdge[]>(
            ReferenceEqualityComparer.Instance
        );

        foreach (var edge in controlFlowGraph.Edges)
        {
            if (edge.From is not SimpleNode || edge.Edge is not ActionEdge)
                continue;
            throw new NotImplementedException();
        }

        throw new NotImplementedException();
    }

    public static IEnumerable<ExprEdge>? AnalyzeExpression(this ActionEdge edge)
    {
        return edge.Action.Opcode switch
        {
            Avm1.Opcode.Not => new[] { new UnaryExpr { Argument = new OpTemporary() }.ToEdge() },
            Avm1.Opcode.Push => ((Avm1.PushData)edge.Action.Action).ToExprEdges(),
            _ => throw new NotImplementedException(),
        };
    }

    private static IEnumerable<ExprEdge> ToExprEdges(this Avm1.PushData pushOp) =>
        from push in pushOp.Values
        select push.ToExprEdge();

    private static ExprEdge ToExprEdge(this Avm1.PushValue value) =>
        value.Type switch
        {
            Avm1.ValueType.Bool => new BooleanLiteralExpr((bool)value.Value).ToEdge(),
            Avm1.ValueType.Double => new NumberLiteralExpr((double)value.Value).ToEdge(),
            Avm1.ValueType.Float => new NumberLiteralExpr((float)value.Value).ToEdge(),
            Avm1.ValueType.Int => new NumberLiteralExpr((int)value.Value).ToEdge(),
            Avm1.ValueType.Null => new NullLiteralExpr().ToEdge(),
            Avm1.ValueType.Undefined => new UndefinedLiteralExpr().ToEdge(),
            Avm1.ValueType.String => new StringLiteralExpr((ushort)value.Value).ToEdge(),
            Avm1.ValueType.ConstantPool8 => new ConstantPoolExpr((byte)value.Value).ToEdge(),
            Avm1.ValueType.ConstantPool16 => new ConstantPoolExpr((ushort)value.Value).ToEdge(),
            Avm1.ValueType.Register => throw new NotImplementedException(),
            _ => throw new NotImplementedException(),
        };
}
