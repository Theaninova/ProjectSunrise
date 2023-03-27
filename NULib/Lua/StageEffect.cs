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

namespace SunriseMono.NULib.Lua;

public class StageEffect
{
    private const string ScriptPath = "game://data/effect/stage_effect/script/EffectSetTool.lua";

    private readonly LuaState _state;

    public const string GimmickEffect = "gimmick";
    public const string JunctionEffect = "junction";
    public const string KeepOutEffect = "keepout";
    public const string PatternRunnerEffect = "pattern_runner";
    public const string RailRunnerEffect = "rail_runner";
    public const string ScreenToneEffect = "screen_tone";
    public const string SoundEffect = "sound";

    public StageEffect(string areaName, string effect)
    {
        // TODO...
    }

    public void Dispose() => _state.Dispose();
}