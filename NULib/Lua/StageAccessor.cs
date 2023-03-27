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

using System.Collections.Generic;
using System.Linq;
using Godot;

// ReSharper disable InconsistentNaming

namespace SunriseMono.NULib.Lua;

/**
 * C# Bindings for StageAccessor.lua
 */
public class StageAccessor
{
    private static readonly string[] Folders = { "env_param", "clip", "model" };
    private const string ScriptPath = "game://data/course/script/StageAccessor.lua";

    private LuaState _state;

    public StageAccessor(string path)
    {
        _state = new LuaState(Folders.SelectMany(folder =>
                from file in NuCache.GetFilesAt($"{path}/{folder}")
                where file.GetExtension().ToLower() == "lua"
                select $"{path}/{folder}/{file}"),
            ScriptPath);
    }
    public void Dispose() => _state.Dispose();

    #region Effect

    public record EffectSurface(int SectionId, string Name, SurfacePolygon[] Polygons);

    public int EffectSurfaceCount => (int)(long)_state.Call("Effect_SurfaceNum")[0];

    public IEnumerable<EffectSurface> EffectSurfaces => from i in Enumerable.Range(0, EffectSurfaceCount) select GetEffectSurface(i);

    public EffectSurface GetEffectSurface(int offset)
    {
        var section = (int)(long)_state.Call("Effect_SurfaceSect", (long)offset)[0];
        var name = (string)_state.Call("Effect_GetName", (long)offset)[0];
        var polyCount = (int)(long)_state.Call("Effect_PolygonNum", (long)offset)[0];
        var polys = from i in Enumerable.Range(0, polyCount)
            select SurfacePolygon.FromPolygon(_state.Call("Effect_GetPolygon", (long)offset, (long)i));

        return new EffectSurface(section, name, polys.ToArray());
    }

    #endregion

    #region Tunnel

    public record TunnelSurface(int SectionId, SurfacePolygon[] Polygons);

    public int TunnelSurfaceCount => (int)(long)_state.Call("Tunnel_SurfaceNum")[0];

    public IEnumerable<TunnelSurface> TunnelSurfaces => from i in Enumerable.Range(0, TunnelSurfaceCount) select GetTunnelSurface(i);

    public TunnelSurface GetTunnelSurface(int offset)
    {
        var section = (int)(long)_state.Call("Tunnel_SurfaceSect", (long)offset)[0];
        var polyCount = (int)(long)_state.Call("Tunnel_PolygonNum", (long)offset)[0];
        var polys = from i in Enumerable.Range(0, polyCount)
            select SurfacePolygon.FromPolygon(_state.Call("Tunnel_GetPolygon", (long)offset, (long)i));

        return new TunnelSurface(section, polys.ToArray());
    }

    #endregion

    #region Shadow

    public record ShadowSurface(int SectionId, SurfacePolygon[] Polygons);

    public int ShadowSurfaceCount => (int)(long)_state.Call("Shadow_SurfaceNum")[0];

    public IEnumerable<ShadowSurface> ShadowSurfaces => from i in Enumerable.Range(0, ShadowSurfaceCount) select GetShadowSurface(i);

    public ShadowSurface GetShadowSurface(int offset)
    {
        var section = (int)(long)_state.Call("Shadow_SurfaceSect", (long)offset)[0];
        var polyCount = (int)(long)_state.Call("Shadow_PolygonNum", (long)offset)[0];
        var polys = from i in Enumerable.Range(0, polyCount)
            select SurfacePolygon.FromPolygon(_state.Call("Shadow_GetPolygon", (long)offset, (long)i));

        return new ShadowSurface(section, polys.ToArray());
    }

    #endregion

    #region HideClip

    public record HideClipSurface(int SectionId, int SurfaceId, string Name, SurfacePolygon[] Polygons);

    public int HideClipSurfaceCount => (int)(long)_state.Call("HideClip_SurfaceNum")[0];

    public IEnumerable<HideClipSurface> HideClipSurfaces =>
        from i in Enumerable.Range(0, HideClipSurfaceCount) select GetHideClipSurface(i);

    public HideClipSurface GetHideClipSurface(int offset)
    {
        var ids = _state.Call("HideClip_SurfaceID", (long)offset);
        var name = (string)_state.Call("HideClip_SurfaceName", (long)offset)[0];
        var polyCount = (int)(long)_state.Call("HideClip_PolygonNum", (long)offset)[0];
        var polys = from i in Enumerable.Range(0, polyCount)
            select SurfacePolygon.FromPolygon(_state.Call("HideClip_GetPolygon", (long)offset, (long)i));

        return new HideClipSurface((int)(long)ids[0], (int)(long)ids[1], name, polys.ToArray());
    }

    #endregion

    #region DistClip

    public record DistClipSurface(int SectionId, int SurfaceId, string Name, SurfacePolygon[] Polygons);

    public int DistClipSurfaceCount => (int)(long)_state.Call("DistClip_SurfaceNum")[0];

    public IEnumerable<DistClipSurface> DistClipSurfaces =>
        from i in Enumerable.Range(0, DistClipSurfaceCount) select GetDistClipSurface(i);

    public DistClipSurface GetDistClipSurface(int offset)
    {
        var ids = _state.Call("DistClip_SurfaceID", (long)offset);
        var name = (string)_state.Call("DistClip_SurfaceName", (long)offset)[0];
        var polyCount = (int)(long)_state.Call("DistClip_PolygonNum", (long)offset)[0];
        var polys = from i in Enumerable.Range(0, polyCount)
            select SurfacePolygon.FromPolygon(_state.Call("DistClip_GetPolygon", (long)offset, (long)i));

        return new DistClipSurface((int)(long)ids[0], (int)(long)ids[1], name, polys.ToArray());
    }

    #endregion

    #region Texture Related

    public string[] TextureNames => _state.Call("GetStageTexture").Cast<string>().ToArray();
    public int TextureNamesCount => (int)(long)_state.Call("StageTexture_Num")[0];

    #endregion

    #region Model Related

    /**
     * Number of sections with models
     */
    public int SectionCount => (int)(long)_state.Call("Model_Num")[0];

    public string GetBinPath(int sect) => (string)_state.State.DoString($"return MODELLIST[{sect}].BIN")[0];

    public AddrName[] GetLongRangeDisplay(int sect) => AddrName.ConvertAddrName(_state, sect, "LONG");
    public AddrName[] GetNearView(int sect) => AddrName.ConvertAddrName(_state, sect, "NEAR");
    public AddrName[] GetLod(int sect) => AddrName.ConvertAddrName(_state, sect, "LODM");
    public AddrName[] GetRoad(int sect) => AddrName.ConvertAddrName(_state, sect, "ROAD");
    public AddrName[] GetOnRoad(int sect) => AddrName.ConvertAddrName(_state, sect, "ONRD");
    public AddrName[] GetRearviewMirror(int sect) => AddrName.ConvertAddrName(_state, sect, "BACK");
    public AddrName[] GetCast(int sect) => AddrName.ConvertAddrName(_state, sect, "CAST");
    public AddrName[] GetVehicleReflection(int sect) => AddrName.ConvertAddrName(_state, sect, "REFC");
    public AddrName[] GetRoadReflection(int sect) => AddrName.ConvertAddrName(_state, sect, "REFR");
    public AddrName[] GetVehicleReflectionBackground(int sect) => AddrName.ConvertAddrName(_state, sect, "RFBG");

    #endregion
}