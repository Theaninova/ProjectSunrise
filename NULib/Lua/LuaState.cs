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
using Godot;

namespace SunriseMono.NULib.Lua;

public class LuaState
{
    public readonly NLua.Lua State;

    public LuaState(IEnumerable<string> dependencies, string path)
    {
        State = new NLua.Lua();
        PatchUnpack();
        foreach (var name in dependencies)
            Exec(name);
        Exec(path);
    }

    public void Dispose() => State.Dispose();

    private void Exec(string path)
        => State.DoString(new System.IO.StreamReader(NuCache.Open(path, FileAccess.ModeFlags.Read)).ReadToEnd());

    private void PatchUnpack()
        => State.DoString("function unpack(x)\n  return table.unpack(x)\nend");

    public object[] Call(string name, params object[] parameters) => State.GetFunction(name).Call(parameters);
}