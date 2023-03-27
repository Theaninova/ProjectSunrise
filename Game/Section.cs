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

using Godot;
using Godot.Collections;
using SunriseMono.NULib;

#endregion

namespace SunriseMono.Game;

public partial class Section : Resource
{
    [Export]
    public int Id;

    [Export]
    public Dictionary<string, string[]> Models;

    public static void Create()
    {
        var root = new Node3D();

        var scene = new PackedScene();
        var result = scene.Pack(root);

        NuCache.Save(scene, $"{NuCache.CachePrefix}://test.tscn");
    }
}
