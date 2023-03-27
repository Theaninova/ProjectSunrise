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

using System.Linq;

namespace SunriseMono.NULib.Lua;

public record AddrName(string Name, int Start, int Length)
{
    public static AddrName[] ConvertAddrName(LuaState state, int sect, string value)
    {
        var addr = state.Call($"GetAddr{value}", sect);
        var name = state.Call($"GetName{value}", sect);
        return name
            .Select((t, i) => new AddrName((string)t, (int)(long)addr[i * 2], (int)(long)addr[i * 2 + 1]))
            .ToArray();
    }
    
    public static AddrName[] ConvertAddrName(LuaState state, string value)
    {
        var addr = state.Call($"GetAddr{value}");
        var name = state.Call($"GetName{value}");
        return name
            .Select((t, i) => new AddrName((string)t, (int)(long)addr[i * 2], (int)(long)addr[i * 2 + 1]))
            .ToArray();
    }
}