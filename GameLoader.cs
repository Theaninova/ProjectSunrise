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
using Microsoft.Extensions.FileSystemGlobbing;

#endregion

namespace SunriseMono;

public class GameLoader
{
    public void Load(string path)
    {
        var matcher = new Matcher();
        matcher.AddInclude("**/*.lua");
        var scripts = matcher.GetResultsInFullPath(path);

        Console.WriteLine(scripts);
    }
}
