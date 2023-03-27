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

using System.IO;
using System.Linq;
using Godot;
using NLua;
using SunriseMono.NULib;
using FileAccess = Godot.FileAccess;

#endregion

namespace SunriseMono.Game;

public partial class LoadList : Resource
{
    private string _path;

    [Export]
    public LoadListModelList[] ModelLists;

    [Export]
    public string[] TextureList;

    public LoadList(string path)
    {
        _path = path;
        var state = new Lua();
        state.DoString(new StreamReader(NuCache.Open(path, FileAccess.ModeFlags.Read)).ReadToEnd());

        TextureList = ((LuaTable)state["TEXTURELIST"]).Values.Cast<string>().ToArray();
        ModelLists = ((LuaTable)state["MODELLIST"]).Values
            .Cast<LuaTable>()
            .Select(LoadListModelList.FromLua)
            .ToArray();
        state.Dispose();
    }

    public void SaveToCache()
    {
        var basePath = NuCache.ReplacePrefix(NuCache.CachePrefix, _path);
        NuCache.MakeDirRecursive(basePath.GetBaseDir());
        NuCache.Save(this, $"{basePath.GetBaseName()}.res");
    }
}
