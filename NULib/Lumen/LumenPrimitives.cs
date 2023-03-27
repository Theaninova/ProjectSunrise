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

public static class LumenPrimitives
{
    public static Color ToColor(this Lmd.Colors.Color self) =>
        new(self.R / 255f, self.G / 255f, self.B / 255f, self.A / 255f);

    public static Rect2 ToRect(this Lmd.Bounds.Rect self) =>
        new(self.X, self.Y, self.Width, self.Height);

    public static Vector2 ToVector2(this Lmd.Positions.Position self) => new(self.X, self.Y);

    public static Transform2D ToTransform2D(this Lmd.Transforms.Matrix self) =>
        new(self.A, self.B, self.C, self.D, self.X, self.Y);

    public static StringName ToGdCharacterId(this uint self) => new(self.ToString());
}