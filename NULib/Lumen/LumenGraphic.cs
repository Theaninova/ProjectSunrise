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
using System.Linq;
using Godot;
using SunriseMono.Kaitai;

namespace SunriseMono.NULib.Lumen;

public static class LumenGraphic
{
    public static TextureRect ToTextureRect(this LumenTag<Lmd.Graphic> self)
    {
        var rect = new TextureRect();
        var meshTexture = new MeshTexture();
        rect.Texture = meshTexture;
        meshTexture.Mesh = self.GetMesh();
        rect.TextureRepeat = CanvasItem.TextureRepeatEnum.Disabled;
        try
        {
            var (name, texture) = self.Flash.TextureAtlas[(int)self.Tag.AtlasId];
            rect.Name = name;
            meshTexture.ResourceName = name;
            meshTexture.BaseTexture = texture;
            meshTexture.ImageSize = texture.GetImage().GetSize();
        }
        catch (ArgumentOutOfRangeException)
        {
            GD.PushError($"Missing texture for atlas ID {self.Tag.AtlasId}");
        }

        return rect;
    }

    private static ArrayMesh GetMesh(this LumenTag<Lmd.Graphic> self)
    {
        var mesh = new ArrayMesh();
        var surfaceArray = new Godot.Collections.Array();
        surfaceArray.Resize((int)Mesh.ArrayType.Max);

        surfaceArray[(int)Mesh.ArrayType.Vertex] = self.Tag.Vertices.Select(it => new Vector2(it.X, it.Y)).ToArray();
        surfaceArray[(int)Mesh.ArrayType.Index] = self.Tag.Indices.Select(it => (int)it).ToArray();
        surfaceArray[(int)Mesh.ArrayType.TexUV] = self.Tag.Vertices.Select(it => new Vector2(it.U, it.V)).ToArray();
        mesh.AddSurfaceFromArrays(Mesh.PrimitiveType.Triangles, surfaceArray);
        
        return mesh;
    }
}