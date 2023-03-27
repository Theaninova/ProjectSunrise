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

using System.Linq;
using Godot;

namespace SunriseMono.NULib.Lua;

/**
 * C# Bindings for SectionAccessor.lua
 */
public class SectionAccessor
{
    private static readonly string[] Folders = { "TODO!" }; // TODO
    private const string ScriptPath = "game://data/course/script/SectionAccessor.lua";

    private LuaState _state;

    public SectionAccessor(string path)
    {
        _state = new LuaState(Folders.SelectMany(folder =>
                from file in NuCache.GetFilesAt($"{path}/{folder}")
                where file.GetExtension().ToLower() == "lua"
                select $"{path}/file"),
            ScriptPath);
    }

    public void Dispose() => _state.Dispose();

    public string[] GetHideModelName(int surface) => _state.Call("HideClip_GetHideModel", (long)surface).Cast<string>().ToArray();

    public record ClipDistance(
        float LongRangeDisplay,
        float NearView,
        float Lod,
        float Road,
        float OnRoad,
        float RearviewMirror,
        float Cast,
        float VehicleReflection,
        float RoadReflection,
        float VehicleReflectionBackground
    );

    public ClipDistance GetClipDistance(int surface)
    {
        var distances = _state.Call("DistClip_GetClipDistance", (long)surface).Cast<double>().ToArray();
        return new ClipDistance(
            (float)distances[0], // LONG
            (float)distances[1], // NEAR
            (float)distances[2], // LODM
            (float)distances[3], // ROAD
            (float)distances[4], // ONRD
            (float)distances[5], //BACK
            (float)distances[6], // CAST
            (float)distances[7], //REFC,
            (float)distances[8], //REFR
            (float)distances[9] // RFBG
        );
    }
}