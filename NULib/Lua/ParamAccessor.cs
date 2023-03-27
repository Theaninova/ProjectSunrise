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
using SunriseMono.NULib.Nodes;

namespace SunriseMono.NULib.Lua;

/// <summary>
/// C# Bindings for ParamAccessor.lua
/// </summary>
public class ParamAccessor
{
    private const string ParamAccessorPath = "game://data/course/script/ParamAccessor.lua";
    private const string ShaderParamPath = "game://data/course/parameter";

    private static readonly string[] ShaderParamNames =
    {
        "navimap/NavimapParam",
        "pivot/PivotParam",
        "shader_param/CarAmbient",
        "shader_param/EnvmapParam",
        "shader_param/GlobalLight",
        "shader_param/KanbanParam",
        "shader_param/ScrollParam",
        "tuning/RampParam",
        "tuning/TAParam"
    };

    private readonly LuaState _state;

    public ParamAccessor()
        => _state = new LuaState(from name in ShaderParamNames select $"{ShaderParamPath}/{name}.lua", ParamAccessorPath);

    public void Dispose() => _state.Dispose();

    public (Vector3 Position, float Pitch) GetGlobalLight_Position(Stage.AreaEnum area, Stage.StageTime time)
    {
        var result = _state.Call("GetGlobalLight_Position", (long)area, (long)time);
        return (
            new Vector3((float)(double)result[0] * NuModel.ImportScale, (float)(double)result[1] * NuModel.ImportScale,
                (float)(double)result[2] * NuModel.ImportScale), Mathf.DegToRad((float)(double)result[3]));
    }

    public Color GetGlobalLight_Color(Stage.AreaEnum area, Stage.StageTime time)
    {
        var result = _state.Call("GetGlobalLight_Color", (long)area, (long)time);
        return new Color((float)(double)result[0], (float)(double)result[1], (float)(double)result[2], (float)(double)result[3]);
    }

    public Color GetGlobalLight_Ambient(Stage.AreaEnum area, Stage.StageTime time)
    {
        // yes, they actually spelled this wrong.
        var result = _state.Call("GetGlobalLight_Anbient", (long)area, (long)time);
        return new Color((float)(double)result[0], (float)(double)result[1], (float)(double)result[2], (float)(double)result[3]);
    }

    /// <summary>
    /// Parameters of the environment map (vehicle reflection)
    /// </summary>
    /// <param name="time"></param>
    /// <returns></returns>
    public (float DefaultContrast, float DefaultLuminance, float BgContrast, float BgLuminance) GetEnvMapParam(Stage.StageTime time)
    {
        var result = _state.Call("GetEnvMapParam", (long)time);
        return ((float)(double)result[0], (float)(double)result[1], (float)(double)result[2], (float)(double)result[3]);
    }
}