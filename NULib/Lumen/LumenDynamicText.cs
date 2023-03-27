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

using System;
using Godot;
using SunriseMono.Kaitai;

namespace SunriseMono.NULib.Lumen;

public static class LumenDynamicText
{
    public static PackedScene ToLabel(this LumenTag<Lmd.DynamicText> self)
    {
        var label = new Label();
        var scene = new PackedScene();
        label.Name = self.Tag.CharacterId.ToGdCharacterId();
        label.Text = self.Flash.Symbols[(int)self.Tag.PlaceholderText];
        label.HorizontalAlignment = self.Tag.Alignment switch
        {
            Lmd.DynamicText.TextAlignment.Center => HorizontalAlignment.Center,
            Lmd.DynamicText.TextAlignment.Left => HorizontalAlignment.Right,
            Lmd.DynamicText.TextAlignment.Right => HorizontalAlignment.Right,
            _ => throw new NotImplementedException($"Unknown alignment {self.Tag.Alignment:X}")
        };
        scene.Pack(label);
        // TODO
        // label.AddThemeFontSizeOverride("default", (int)self.Tag.Size);

        return scene;
    }
}