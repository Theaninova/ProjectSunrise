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
using Kaitai;
using SunriseMono.NULib;
using FileAccess = Godot.FileAccess;

#endregion

namespace SunriseMono.Game;

public partial class Stage : GodotObject
{
    public string Area;
    public string Name;

    public Stage(string area, string name)
    {
        Area = area;
        Name = name;
        GD.Print(area, name);
    }

    public void SaveToCache()
    {
        var stagePath = $"game://data/course/stage/{Area}/{Name}";

        var loadList = new LoadList($"{stagePath}/model/LOADLIST_{Area}_{Name}.lua");
        var textures = (
            from texture in loadList.TextureList
            select new NuTexture($"{stagePath}/model/nut/{texture}")
        ).ToArray();
        // var hideClip = new HideClip($"{stagePath}/clip");

        var root = new Node3D();
        root.Name = Name;

        foreach (var section in loadList.ModelLists)
        {
            var binPath = $"{stagePath}/model/bin/{section.Bin.GetFile()}";
            var binStream = NuCache.Open(binPath, FileAccess.ModeFlags.Read);

            var sectionRoot = new Node3D();
            sectionRoot.Name = section.Bin.GetBaseName().GetFile();
            root.AddChild(sectionRoot);
            sectionRoot.Owner = root;

            foreach (var (category, models) in section.Models)
            {
                var categoryRoot = new Node3D();
                categoryRoot.Name = category;
                // if (category == "ONRD") categoryRoot.Visible = false;
                sectionRoot.AddChild(categoryRoot);
                categoryRoot.Owner = root;

                foreach (var (_, address) in models)
                {
                    var bytes = new byte[address[1]];
                    binStream.Seek(address[0], SeekOrigin.Begin);
                    binStream.Read(bytes, 0, (int)address[1]);

                    var model = new NuModel(new KaitaiStream(bytes), textures);
                    foreach (var (partName, part) in model.Meshes)
                    {
                        var partInstance = new MeshInstance3D();
                        partInstance.Name = partName;
                        // if (partName.Contains("shadow")) partInstance.Visible = false;
                        // if (!hideClip.Hide.Contains(partName)) partInstance.Visible = false;
                        partInstance.Mesh = part;
                        categoryRoot.AddChild(partInstance);
                        partInstance.Owner = root;
                    }
                }
            }
        }

        var scene = new PackedScene();
        scene.Pack(root);

        NuCache.Save(
            scene,
            NuCache.ReplacePrefix(NuCache.CachePrefix, $"{stagePath}.scn"),
            ResourceSaver.SaverFlags.ChangePath | ResourceSaver.SaverFlags.Compress
        );
    }
}
