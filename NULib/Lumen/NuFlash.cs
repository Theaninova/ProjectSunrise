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
using Godot;
using Kaitai;
using SunriseMono.Kaitai;

#endregion

namespace SunriseMono.NULib.Lumen;

public partial class NuFlash : GodotObject
{
    public PackedScene Root;

    public readonly List<Rect2> Bounds;
    public readonly List<Color> Colors;
    public readonly List<Vector2> Positions;

    public readonly List<string> Symbols;
    public readonly List<(string, ImageTexture)> TextureAtlas;
    public readonly List<Transform2D> Transforms;

    public readonly float FrameRate;

    public readonly Dictionary<uint, PackedScene> Defines = new();

    public string Path;

    public NuFlash(string path)
    {
        
        Path = path;
        var lmd = new Lmd(new KaitaiStream(NuCache.Open(path, FileAccess.ModeFlags.Read)));
        var nuTexture = new NuTexture($"{Path.GetBaseName()}.nut");
        var tags = lmd.Lmb.Tags.BundleTags(this);
        var textures = (
            from texture in lmd.Textures.TextureHashes
            select nuTexture.Textures[(int)texture]
        ).ToArray();
        
        var packer = new NuPacker(path);

        Symbols = tags.FindChild<Lmd.Symbols>().Tag.Values.ConvertAll(it => it.Value);
        Colors = tags.FindChild<Lmd.Colors>().Tag.Values.ConvertAll(it => it.ToColor());
        Transforms = tags.FindChild<Lmd.Transforms>()
            .Tag.Values.ConvertAll(it => it.ToTransform2D());
        Positions = tags.FindChild<Lmd.Positions>().Tag.Values.ConvertAll(it => it.ToVector2());
        Bounds = tags.FindChild<Lmd.Bounds>().Tag.Values.ConvertAll(it => it.ToRect());
        // NuCache.Write($"{savePath}/actions.abc", tags.FindChild<Lmd.Actionscript>().Tag.Bytecode.ToArray());
        TextureAtlas = tags.FindChild<Lmd.TextureAtlases>()
            .Tag.Values.ConvertAll(it => (Symbols[(int)it.NameId], textures[it.Id]));
        foreach (var (name, texture) in TextureAtlas)
        {
            packer.Add(texture, $"{name}.res", ResourceSaver.SaverFlags.Compress);
        }

        // TODO: unknown_f008 // jpeg tables?
        // TODO: unknown_f009 // background color?
        // TODO: unknown_f00a // also font?
        // TODO: fonts
        // TODO: unknown_f00b // some text stuff?
        var properties = tags.FindChild<Lmd.Properties>();
        FrameRate = properties.Tag.Framerate;

        var defines = tags.FindChild<Lmd.Defines>();

        foreach (var button in defines.FindChildren<Lmd.Button>())
        {
            var scene = button.ToButton();
            Defines[button.Tag.CharacterId] = scene;
            packer.Add(scene, $"button-{button.Tag.CharacterId}.scn");
        }

        foreach (var text in defines.FindChildren<Lmd.DynamicText>())
        {
            var scene = text.ToLabel();
            Defines[text.Tag.CharacterId] = scene;
            packer.Add(scene, $"dynamic-text-{text.Tag.CharacterId}.scn");
        }

        foreach (var control in defines.FindChildren<Lmd.DefineSprite>())
        {
            var symbol = Symbols[(int)control.Tag.NameId];
            var scene = control.ToControl();
            Defines[control.Tag.CharacterId] = scene;
            packer.Add(scene, $"sprite-{(symbol == "" ? control.Tag.CharacterId : symbol)}.scn");
        }

        Root = properties.ToRootScene();


        // foreach (var (id, define) in Defines)
        // {
        //     NuCache.Save(define, $"{savePath}/define_{id}.res");
        // }

        // NuCache.Save(Root, $"{savePath}/.scene.tscn");
        packer.Flush();
    }
}