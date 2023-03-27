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
using System.IO.Compression;
using System.Resources;
using Godot;
using FileAccess = Godot.FileAccess;

#endregion

namespace SunriseMono.NULib;

public class NuCache
{
    private static readonly string GameDir = OS.HasFeature("editor")
        ? (string)ProjectSettings.GetSetting("application/nu_cache/paths/editor_game_dir")
        : OS.GetExecutablePath().GetBaseDir().Replace('\\', '/');

    public static readonly string CachePrefix = (string)
        ProjectSettings.GetSetting("application/nu_cache/paths/cache_prefix");

    public static readonly string SavePrefix = (string)
        ProjectSettings.GetSetting("application/nu_cache/paths/save_prefix");

    public static readonly string GamePrefix = (string)
        ProjectSettings.GetSetting("application/nu_cache/paths/game_prefix");

    private static readonly string DataDirName = (string)
        ProjectSettings.GetSetting("application/nu_cache/directories/data_directory_name");

    private static readonly string CacheDirName = (string)
        ProjectSettings.GetSetting("application/nu_cache/directories/cache_directory_name");

    public static string GetPath(string path)
    {
        var segments = path.Split("://", 2);
        if (segments.Length == 1)
            return path;

        var prefix = segments[0];
        var rest = segments[1];

        return prefix == SavePrefix
            ? $"{GameDir}/{DataDirName}/{rest}"
            : prefix == CachePrefix
                ? $"{GameDir}/{DataDirName}/{CacheDirName}/{rest}"
                : prefix == GamePrefix
                    ? $"{GameDir}/{rest}"
                    : path;
    }

    public static string ReplacePrefix(string prefix, string path)
    {
        var segments = path.Split("://", 2);
        return $"{prefix}://{(segments.Length == 1 ? segments[0] : segments[1])}";
    }

    public static void Write(string path, byte[] buffer)
    {
        GD.Print(GetPath(path));
        var access = FileAccess.Open(GetPath(path), FileAccess.ModeFlags.Write);
        access.StoreBuffer(buffer);
    }

    public static Stream Open(string path, FileAccess.ModeFlags flags)
    {
        GD.Print(GetPath(path));
        var access = FileAccess.Open(GetPath(path), flags);
        
        var buffer = access.GetBuffer((long)access.GetLength());

        if (new BinaryReader(new MemoryStream(buffer)).ReadUInt16() != (ushort)FileSignatures.Gzip)
            return new MemoryStream(buffer);

        var decompressedStream = new MemoryStream();
        new GZipStream(new MemoryStream(buffer), CompressionMode.Decompress).CopyTo(
            decompressedStream
        );
        decompressedStream.Seek(0, SeekOrigin.Begin);
        return decompressedStream;
    }

    public static void Save(
        Resource resource,
        string path = "",
        ResourceSaver.SaverFlags flags = ResourceSaver.SaverFlags.None
    )
    {
        ResourceSaver.Save(resource, GetPath(path), flags);
    }

    public static T Load<T>(
        string path,
        string typeHint = null,
        ResourceLoader.CacheMode cacheMode = ResourceLoader.CacheMode.Reuse
    )
        where T : class
    {
        return ResourceLoader.Load<T>(GetPath(path), typeHint, cacheMode);
    }

    public static void MakeDirRecursive(string path)
    {
        DirAccess.MakeDirRecursiveAbsolute(GetPath(path));
    }

    public static string[] GetFilesAt(string path)
    {
        return DirAccess.GetFilesAt(GetPath(path));
    }

    private enum FileSignatures
    {
        Gzip = 0x8b1f
    }
}
