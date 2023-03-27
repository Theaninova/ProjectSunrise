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

namespace SunriseMono.NULib;

public class NuPacker
{
    private readonly PckPacker _packer;
    private readonly string _gamePath;
    private readonly string _path;
    
    public NuPacker(string path)
    {
        // make sure we don't start removing game files
        path = NuCache.ReplacePrefix("user", path.GetBaseName());
        _path = NuCache.GetPath(path);
        _gamePath = NuCache.ReplacePrefix("user", path);
        _packer = new PckPacker();
        _packer.PckStart(NuCache.GetPath(NuCache.ReplacePrefix(NuCache.CachePrefix, $"{path}.pck")));
    }

    public void Add(Resource res, string relativePath, ResourceSaver.SaverFlags flags = ResourceSaver.SaverFlags.None)
    {
        res.ResourcePath = $"{_path}/{relativePath}";
        DirAccess.MakeDirRecursiveAbsolute(res.ResourcePath.GetBaseDir());
        ResourceSaver.Save(res, res.ResourcePath, flags);
        _packer.AddFile($"{_gamePath}/{relativePath}", res.ResourcePath);
    }

    public void Flush()
    {
        _packer.Flush();
        DirAccess.RemoveAbsolute(_path);
    }
}