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
using System.IO;
using NLua;
using SunriseMono.NULib;
using FileAccess = Godot.FileAccess;

#endregion

namespace SunriseMono.Game;

public class HideClip
{
    public readonly HashSet<string> Hide = new();

    // public Godot.Collections.Dictionary<string, int> DistClip = new();

    public HideClip(string path)
    {
        foreach (var file in NuCache.GetFilesAt(path))
        {
            if (!file.StartsWith("HIDECLIP"))
                continue;
            if (!file.EndsWith(".lua"))
                continue;

            var state = new Lua();
            state.DoString(
                new StreamReader(
                    NuCache.Open($"{path}/{file}", FileAccess.ModeFlags.Read)
                ).ReadToEnd()
            );
            foreach (
                var global in new[]
                {
                    "HIDE_MODEL_00",
                    "HIDE_MODEL_01",
                    "HIDE_MODEL_02",
                    "HIDE_MODEL_03",
                    "HIDE_MODEL_04",
                    "HIDE_MODEL_05",
                    "HIDE_MODEL_06",
                    "HIDE_MODEL_07",
                    "HIDE_MODEL_08",
                    "HIDE_MODEL_09",
                    "HIDE_MODEL_10",
                    "HIDE_MODEL_11",
                    "HIDE_MODEL_12",
                    "HIDE_MODEL_13",
                    "HIDE_MODEL_14",
                    "HIDE_MODEL_15",
                    "HIDE_MODEL_16",
                    "HIDE_MODEL_17",
                    "HIDE_MODEL_18"
                }
            )
            {
                var table = state[global];
                if (table == null)
                    continue;
                foreach (string value in ((LuaTable)table).Values)
                    Hide.Add(value);

                break;
            }

            state.Dispose();
        }
    }
}
