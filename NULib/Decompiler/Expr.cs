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
using System.Diagnostics.CodeAnalysis;
using System.Linq;

namespace SunriseMono.NULib.Decompiler;

public abstract record Expr
{
    public dynamic Loc;

    protected ExprEdge ToEdge(int inputs) =>
        new ExprEdge
        {
            Expression = new PartialExpr { Inputs = inputs, Expression = this }
        };

    public virtual ExprEdge ToEdge() => ToEdge(0);

    public virtual Expr Eval(int id, int shift, Expr value) => throw new NotImplementedException();
}

public enum AssignmentOperator
{
    Simple,
    Add
}

public record AssignmentExpr : Expr
{
    public AssignmentOperator Operator;
    public Pattern Target;
    public Expr Value;
}

public enum BinaryOperator
{
    Add,
    BitAnd,
    BitOr,
    BitXor,
    Divide,
    Equals,
    Greater,
    InstanceOf,
    LegacyAdd,
    LeftShift,
    Less,
    Multiply,
    NotEquals,
    NotStrictEquals,
    Remainder,
    SignedRightShift,
    Subtract,
    StrictEquals,
    UnsignedRightShift
}

public record BinaryExpr : Expr
{
    public BinaryOperator Operator;
    public Expr Left;
    public Expr Right;
}

public record BooleanLiteralExpr(bool Value) : Expr;

public record CallExpr : Expr
{
    public Expr Callee;
    public Expr[] Arguments;
}

public record ConditionalExpr : Expr
{
    public Expr Test;
    public Expr Truthy;
    public Expr Falsy;
}

public record Identifier : Expr
{
    public string Name;
}

public enum LogicalOperator
{
    And,
    Or
}

public record LogicalExpr : Expr
{
    public LogicalOperator Operator;
    public Expr Left;
    public Expr Right;
}

public record MemberExpr : Expr
{
    public Expr Base;
    public Expr Key;
}

public record NewExpr : Expr
{
    public Expr Callee;
    public Expr[] Arguments;
}

public record NullExpr : Expr { }

public record UndefinedLiteralExpr : Expr;

public record NullLiteralExpr : Expr;

public record NumberLiteralExpr(double Value) : Expr;

/// Caution to anyone viewing this interested in AVM1 decompilation:
/// This is a divergence from standard AVM1 bytecode in where string
/// literals are always feeding from the constant pool.
public record StringLiteralExpr(int Id) : Expr;

public record ConstantPoolExpr(int Id) : Expr;

public record OpGlobal : Expr { }

public record OpPop : Expr { }

public record OpPropertyName : Expr
{
    public Expr Index;
}

public record OpRegister : Expr
{
    public int Id;
}

public record OpTemporary : Expr
{
    public int Id;
}

public record OpVariable : Expr
{
    public Expr Name;
}

public record SequenceExpr : Expr
{
    public Expr[] Expressions;
}

public enum UnaryOperator
{
    BitNot,
    Delete,
    LogicalNot,
    TypeOf,
    Void
}

public record UnaryExpr : Expr
{
    public UnaryOperator Operator;
    public Expr Argument;

    public override ExprEdge ToEdge() => ToEdge(1);
}

public record PartialExpr
{
    public int Inputs;
    public Expr Expression;
    public bool Void;
    public dynamic Type;

    public ExprEdge ToEdge() => new ExprEdge { Expression = this };

    public bool TryMergeWith(PartialExpr other, [MaybeNullWhen(false)] out PartialExpr mergedExpr)
    {
        mergedExpr = null;
        if (other.Inputs == 0 || Void)
            return false;

        mergedExpr = other with
        {
            Inputs = Inputs + other.Inputs - 1,
            Expression = Expression.Eval(other.Inputs - 1, Inputs, other.Expression)
        };
        return true;
    }
}
