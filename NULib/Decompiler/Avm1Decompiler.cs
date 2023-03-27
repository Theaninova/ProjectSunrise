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
using SunriseMono.Kaitai;

#endregion

namespace SunriseMono.NULib.Decompiler;

public class Avm1Decompiler
{
    public static ControlFlowGraph Decompile(List<Avm1.Op> avm1)
    {
        var controlFlowGraph = new ControlFlowGraphBuilder().FromAvm1(avm1);
        // TODO: reduceConstantPool
        // TODO: expressionize
        // TODO: reduceChains
        // TODO: reduceConditionals
        return controlFlowGraph;
    }
}
