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

namespace SunriseMono.NULib.Lua;

public record SurfacePolygon(Vector3 X, Vector3 Y, Vector3 Z, Vector3 Dx, Vector3 Dy, Vector3 Dz)
{
    public static SurfacePolygon FromPolygon(IEnumerable<object> poly)
    {
        var v = (from chunk in poly.Cast<double>().Chunk(3)
            select new Vector3((float)chunk[0], (float)chunk[1], (float)chunk[2])).ToArray();
        return new SurfacePolygon(v[0], v[2], v[4], v[1], v[3], v[5]);
    }
}
