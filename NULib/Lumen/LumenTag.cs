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

using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using Kaitai;
using SunriseMono.Kaitai;

namespace SunriseMono.NULib.Lumen;


public record LumenTag<TTag>(TTag Tag, List<Lmd.Tag> Children, Lmd.Tag.FlashTagType Type, NuFlash Flash) : IEnumerable<Lmd.Tag>
    where TTag : KaitaiStruct
{
    public LumenTag(TTag tag, Lmd.Tag.FlashTagType type, NuFlash flash)
        : this(tag, new List<Lmd.Tag>(), type, flash) { }

    public LumenTag<TOut> FindChild<TOut>()
        where TOut : KaitaiStruct => Children.FindChild<TOut>(Flash);

    public IEnumerable<LumenTag<TOut>> FindChildren<TOut>()
        where TOut : KaitaiStruct => Children.FindChildren<TOut>(Flash);

    public IEnumerable<LumenTag<TOut>> FindChildren<TOut>(Lmd.Tag.FlashTagType tagType)
        where TOut : KaitaiStruct => Children.FindChildren<TOut>(Flash, tagType);

    public IEnumerable<LumenTag<TOut>> GetChildrenAs<TOut>()
        where TOut : KaitaiStruct => from child in Children select child.ToLumenTag<TOut>(Flash);

    public IEnumerator<Lmd.Tag> GetEnumerator() => Children.GetEnumerator();

    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
}

public static class TagTraversal
{
    public static IEnumerable<LumenTag<TOut>> FindChildren<TOut>(
        this IEnumerable<Lmd.Tag> self,
        NuFlash flash,
        Lmd.Tag.FlashTagType tagType
    )
        where TOut : KaitaiStruct =>
        from tag in self
        where tag.TagType == tagType
        select tag.ToLumenTag<TOut>(flash);

    public static IEnumerable<LumenTag<TOut>> FindChildren<TOut>(
        this IEnumerable<Lmd.Tag> self,
        NuFlash flash
    )
        where TOut : KaitaiStruct =>
        from tag in self
        where tag.Data is TOut
        select tag.ToLumenTag<TOut>(flash);

    public static LumenTag<TOut> FindChild<TOut>(this List<Lmd.Tag> self, NuFlash flash)
        where TOut : KaitaiStruct => self.Find(it => it.Data is TOut).ToLumenTag<TOut>(flash);

    public static bool TryToLumenTag<TOut>(
        this Lmd.Tag self,
        NuFlash flash,
        [MaybeNullWhen(false)] out LumenTag<TOut> tag
    )
        where TOut : KaitaiStruct
    {
        if (self.Data is TOut typedTag)
        {
            tag = new LumenTag<TOut>(typedTag, self.Children, self.TagType, flash);
            return true;
        }

        tag = null;
        return false;
    }

    public static LumenTag<TOut> ToLumenTag<TOut>(this Lmd.Tag tag, NuFlash flash)
        where TOut : KaitaiStruct => new((TOut)tag.Data, tag.Children, tag.TagType, flash);

    public static LumenTag<KaitaiStruct> BundleTags(this List<Lmd.Tag> self, NuFlash flash) =>
        new(null, self, Lmd.Tag.FlashTagType.End, flash);
}

