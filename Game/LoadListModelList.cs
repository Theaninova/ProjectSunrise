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

using System.Linq;
using Godot;
using Godot.Collections;
using NLua;

#endregion

namespace SunriseMono.Game;

public partial class LoadListModelList : Resource
{
    public static readonly string[] LodList =
    {
        "LONG",
        "NEAR",
        "LODM",
        "ROAD",
        "ONRD",
        "BACK",
        "CAST",
        "REFC",
        "REFR",
        "RFBG",
        "BGSP",
        "EVER"
    };

    [Export]
    public string Bin;

    [Export]
    public Dictionary<string, Dictionary<string, long[]>> Models;

    [Export]
    public long SectionId;

    public static LoadListModelList FromLua(LuaTable table)
    {
        var list = new LoadListModelList();
        list.SectionId = table["SECTION_ID"] == null ? -1 : (long)table["SECTION_ID"];
        list.Bin = (string)table["BIN"];

        list.Models = new Dictionary<string, Dictionary<string, long[]>>();
        foreach (var lod in LodList)
        {
            if (table[$"{lod}_NAME"] == null)
                continue;
            var names = ((LuaTable)table[$"{lod}_NAME"]).Values.Cast<string>().ToArray();
            var addresses = ((LuaTable)table[$"{lod}_ADDR"]).Values.Cast<long>().ToArray();
            var dict = new Dictionary<string, long[]>();

            for (var i = 0; i < names.Length; i++)
                dict[names[i]] = new[] { addresses[i * 2], addresses[i * 2 + 1] };

            list.Models[lod] = dict;
        }

        return list;
    }
}
