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

using Godot;
using SunriseMono.Kaitai;

namespace SunriseMono.NULib.Lumen;

public static class LumenButton
{
    public static PackedScene ToButton(this LumenTag<Lmd.Button> self)
    {
        var scene = new PackedScene();
        var button = new Button();
        button.Name = self.Tag.CharacterId.ToGdCharacterId();
        button.Flat = true;

        var bounds = self.Flash.Bounds[(int)self.Tag.BoundsId];
        button.Position = bounds.Position;
        button.Size = bounds.Size;
        // TODO
        // var script = new GDScript();
        // // TODO: actual script...
        // script.SourceCode =
        //     "func _button_pressed():"
        //     + $"\n    print(\"Pressed button {button.Name}\")"
        //     + $"\n    print(\"action_offset = 0x{self.Tag.ActionOffset:X}\")"
        //     + $"\n    print(\"track_as_menu = {self.Tag.TrackAsMenu}\")";
        // script.Reload();
        // button.SetScript(script);

        foreach (var child in self.GetChildrenAs<Lmd.Graphic>())
        {
            var rect = child.ToTextureRect();
            button.AddChild(rect);
            rect.Owner = button;
        }

        scene.Pack(button);

        return scene;
    }
}