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
using System.Text;
using SunriseMono.Kaitai;
using SunriseMono.NULib.Decompiler;

#endregion

namespace SunriseMono.NULib;

public class Avm1Compiler
{
    private readonly Avm1 _avm1;
    private readonly string[] _symbols;
    public readonly string[] Actions;

    public Avm1Compiler(Avm1 avm1, string[] symbols)
    {
        _avm1 = avm1;
        _symbols = symbols;
        Actions = (
            from action in avm1.Actions
            select new ActionCompiler(action.Body.Ops, symbols).Code
        ).ToArray();
    }

    private class ActionCompiler
    {
        private const string StackName = "ɵstack";
        private const string t0Name = "ɵt0";
        private const string t1Name = "ɵt1";
        private readonly List<Avm1.Op> _body;
        private readonly StringBuilder _builder = new();
        private readonly string[] _symbols;
        private readonly int Cursor;
        private int Indent;

        public ActionCompiler(List<Avm1.Op> body, string[] symbols)
        {
            var controlFlowGraph = Avm1Decompiler.Decompile(body);

            _body = body;
            _symbols = symbols;

            EmitLine("func action:");
            Inc();
            EmitLine($"var {StackName} = []");
            for (Cursor = 0; Cursor < body.Count; Cursor++)
                switch (_body[Cursor].Opcode)
                {
                    case Avm1.Opcode.Push:
                        foreach (var value in ((Avm1.PushData)_body[Cursor].Action).Values)
                            EmitLine($"{StackName}.push_back({ValueToLiteral(value)})");
                        break;
                    case Avm1.Opcode.GetVariable:
                        EmitLine($"{StackName}.push_back(global.get({StackName}.pop_back()))");
                        break;
                    case Avm1.Opcode.SetMember:
                        EmitLine($"{t0Name} = {StackName}.pop_back()");
                        EmitLine($"{t1Name} = {StackName}.pop_back()");
                        EmitLine(
                            $"{StackName}.push_back({StackName}.pop_back().set({t0Name}, {t1Name}))"
                        );
                        break;
                    case Avm1.Opcode.GetMember:
                        EmitLine($"{t0Name} = {StackName}.pop_back()");
                        EmitLine($"{StackName}.push_back({StackName}.pop_back().get({t0Name}))");
                        break;
                }
        }

        public string Code => _builder.ToString();

        private string ValueToLiteral(Avm1.PushValue value)
        {
            switch (value.Type)
            {
                case Avm1.ValueType.String:
                    return $"\"{_symbols[(ushort)value.Value]}\"";
                case Avm1.ValueType.ConstantPool8:
                    return $"\"{_symbols[(byte)value.Value]}\"";
                case Avm1.ValueType.Null:
                    return "null";
                case Avm1.ValueType.Undefined:
                    return "null";
                default:
                    return value.Value.ToString();
            }
        }

        private void Inc()
        {
            Indent += 2;
        }

        private void Dec()
        {
            Indent -= 2;
        }

        private void EmitLine(string line)
        {
            for (var i = 0; i < Indent; i++)
                _builder.Append(' ');
            _builder.Append(line);
            _builder.Append('\n');
        }
    }
}
