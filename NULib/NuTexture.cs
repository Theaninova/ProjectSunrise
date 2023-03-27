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

using System;
using System.IO;
using Godot;
using Godot.Collections;
using Kaitai;
using SunriseMono.Kaitai;
using FileAccess = Godot.FileAccess;

#endregion

namespace SunriseMono.NULib;

public partial class NuTexture : GodotObject
{
    public Dictionary<int, Cubemap> Cubemaps = new();

    public string Path;
    public Dictionary<int, ImageTexture> Textures = new();

    public NuTexture() {}
    
    public NuTexture(string path)
        : this(NuCache.Open(path, FileAccess.ModeFlags.Read))
    {
        Path = path;
    }

    public NuTexture(Stream stream)
        : this(new Nut(new KaitaiStream(stream))) { }

    public NuTexture(Nut nut)
    {
        foreach (var texture in nut.Body.Textures)
            if (texture.TextureInfo.Cubemap.IsCubemap)
            {
                var cubemap = new Cubemap();
                if (texture.TextureInfo.CubemapCount != 6)
                    GD.PushWarning($"{Path}:{texture.Gidx.HashId} - Cubemap with <6 sides");

                var images = new Array<Image>();
                images.Resize(6);
                for (int i = 0, j = 0; i < images.Count; i++)
                {
                    if (!texture.TextureInfo.Cubemap.Sides[i])
                        continue;
                    images[i] = SurfaceToImage(
                        texture.TextureData.Surfaces.Surfaces[j],
                        texture.TextureInfo
                    );
                    j++;
                }

                cubemap._Images = images;
                cubemap.ResourceName = texture.Gidx.HashId.ToString();
                Cubemaps.Add((int)texture.Gidx.HashId, cubemap);
            }
            else
            {
                var imageTexture = new ImageTexture();
                var image = SurfaceToImage(
                    texture.TextureData.Surfaces.Surfaces[0],
                    texture.TextureInfo
                );
                imageTexture.SetImage(image);
                imageTexture.ResourceName = texture.Gidx.HashId.ToString();
                Textures.Add((int)texture.Gidx.HashId, imageTexture);
            }
    }

    public void SaveToCache()
    {
        var baseDir = NuCache.ReplacePrefix(NuCache.CachePrefix, Path.GetBaseName());
        NuCache.MakeDirRecursive(baseDir);

        foreach (var (hash, texture) in Textures)
            NuCache.Save(texture, $"{baseDir}/{hash}.res");

        foreach (var (hash, cubemap) in Cubemaps)
        {
            // TODO: https://github.com/godotengine/godot/pull/71394
            for (var i = 0; i < cubemap._Images.Count; i++)
                NuCache.Save(cubemap._Images[i], $"{baseDir}/{hash}.{i}.res");
            NuCache.Save(cubemap, $"{baseDir}/{hash}.tres");
        }
    }

    private static Image SurfaceToImage(
        Nut.NutBody.TextureSurface surface,
        Nut.NutBody.TextureInfo textureInfo
    )
    {
        var mipmaps = surface.Mipmaps;
        var mipmap =
            textureInfo.MipmapSizes.Count == 0
                ? mipmaps
                : mipmaps[..(int)textureInfo.MipmapSizes[0]];

        var image = Image.CreateFromData(
            textureInfo.Width,
            textureInfo.Height,
            false,
            ToGodotFormat(textureInfo.PixelFormat),
            mipmap
        );
        image.Decompress();
        image.GenerateMipmaps();

        return image;
    }

    private static Image.Format ToGodotFormat(Nut.PixelFormat pixelFormat)
    {
        return pixelFormat switch
        {
            Nut.PixelFormat.Dxt1 => Image.Format.Dxt1,
            Nut.PixelFormat.Dxt3 => Image.Format.Dxt3,
            Nut.PixelFormat.Dxt5 => Image.Format.Dxt5,
            Nut.PixelFormat.Rgba => Image.Format.Rgbaf,
            Nut.PixelFormat.Rgba0 => Image.Format.Rgba8,
            Nut.PixelFormat.Rgba16 => Image.Format.Rgbah,
            Nut.PixelFormat.Rgb16 => Image.Format.Rgbh,
            _ => throw new Exception($"Unsupported Pixel Format {pixelFormat}")
        };
    }
}
