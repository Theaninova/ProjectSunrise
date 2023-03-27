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

// ReSharper disable InconsistentNaming

namespace SunriseMono.NULib.Lua;

///<summary>
///C# Bindings for AreaAccessor.lua
///</summary>
public class AreaAccessor
{
    private static readonly string[] Folders = { "env_param", "clip", "model" };
    private const string ScriptPath = "game://data/course/script/AreaAccessor.lua";

    private readonly LuaState _state;

    public AreaAccessor(string path)
    {
        _state = new LuaState(Folders.SelectMany(folder =>
                from file in NuCache.GetFilesAt($"{path}/{folder}")
                where file.GetExtension().ToLower() == "lua"
                select $"{path}/{folder}/{file}"),
            ScriptPath);
    }

    public void Dispose() => _state.Dispose();

    #region DefaultDistClip

    public record DefaultClipDistance(
        float Skybox,
        float Residential,
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

    public DefaultClipDistance GetClipDistance(int surface)
    {
        var distances = _state.Call("DistClip_GetClipDistance", (long)surface).Cast<double>().ToArray();
        return new DefaultClipDistance(
            (float)distances[0], // BGSP
            (float)distances[1], // EVER
            (float)distances[2], // LONG
            (float)distances[3], // NEAR
            (float)distances[4], // LODM
            (float)distances[5], // ROAD
            (float)distances[6], // ONRD
            (float)distances[7], //BACK
            (float)distances[8], // CAST
            (float)distances[9], //REFC,
            (float)distances[10], //REFR
            (float)distances[11] // RFBG
        );
    }

    #endregion

    #region Fog

    public record FogSurfaceType(float Near, float Far, SurfacePolygon[] Polygons);

    public FogSurfaceType FogSurface
    {
        get
        {
            var range = _state.Call("Fog_Range");
            var polyCount = (int)(long)_state.Call("Fog_PolygonNum")[0];
            var polys = from i in Enumerable.Range(0, polyCount)
                select SurfacePolygon.FromPolygon(_state.Call("Fog_GetPolygon", (long)i));

            return new FogSurfaceType((float)(double)range[0], (float)(double)range[1], polys.ToArray());
        }
    }

    #endregion

    #region Texture Related

    public string[] TextureNames => _state.Call("GetAreaTexture").Cast<string>().ToArray();
    public int TextureNamesCount => (int)(long)_state.Call("AreaTexture_Num")[0];

    #endregion

    #region Model Related

    /// <summary>
    /// Number of sections with models
    /// </summary>
    public int SectionCount => (int)(long)_state.Call("Model_Num")[0];

    public string BinPath => (string)_state.State.DoString($"return MODELLIST[1].BIN")[0];

    public AddrName[] Skybox => AddrName.ConvertAddrName(_state, "BGSP");
    public AddrName[] Residential => AddrName.ConvertAddrName(_state, "EVER");
    public AddrName[] LongRangeDisplay => AddrName.ConvertAddrName(_state, "LONG");
    public AddrName[] NearView => AddrName.ConvertAddrName(_state, "NEAR");
    public AddrName[] Lod => AddrName.ConvertAddrName(_state, "LODM");
    public AddrName[] Road => AddrName.ConvertAddrName(_state, "ROAD");
    public AddrName[] OnRoad => AddrName.ConvertAddrName(_state, "ONRD");
    public AddrName[] RearviewMirror => AddrName.ConvertAddrName(_state, "BACK");
    public AddrName[] Cast => AddrName.ConvertAddrName(_state, "CAST");
    public AddrName[] VehicleReflection => AddrName.ConvertAddrName(_state, "REFC");
    public AddrName[] RoadReflection => AddrName.ConvertAddrName(_state, "REFR");
    public AddrName[] VehicleReflectionBackground => AddrName.ConvertAddrName(_state, "RFBG");

    #endregion
}