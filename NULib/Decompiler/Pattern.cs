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

namespace SunriseMono.NULib.Decompiler;

public abstract record Pattern
{
    public dynamic Loc { get; init; }
}

public record IdentifierPattern : Pattern
{
    public string Name { get; init; }
}

public record MemberPattern : Pattern
{
    public Expr Base { get; init; }
    public Expr Key { get; init; }
}

public record OpRegisterPattern : Pattern
{
    public int Id { get; init; }
}

public record OpTemporaryPattern : Pattern
{
    public int Id { get; init; }
}
