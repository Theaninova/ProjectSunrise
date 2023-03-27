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

namespace SunriseMono.NULib.Nodes;

[Tool]
public partial class EnvParam : Enma3D
{
    private string _path;
    [Export]
    public string Path
    {
        get => _path;
        set
        {
            _path = value;
            Reload();
        }
    }

    public override void _Ready()
    {
        Reload();
    }

    private void Reload()
    {
        try
        {
            AddChild(NuSurface.LoadEnvParam(_path));
        }
        catch
        {
            GD.PushWarning("Loading env param failed");
        }
    }
}