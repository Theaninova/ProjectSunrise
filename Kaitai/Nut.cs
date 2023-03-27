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

using System.Collections.Generic;
using Kaitai;

#endregion

namespace SunriseMono.Kaitai;

public partial class Nut : KaitaiStruct
{
    public enum NutSignature
    {
        Ntlx = 1314147416,
        Ntp3 = 1314148403,
        Ntwd = 1314150212,
        Ntwu = 1314150229
    }

    public enum PixelFormat
    {
        Dxt1 = 0,
        Dxt3 = 1,
        Dxt5 = 2,
        Rgb16 = 8,
        Rgba16 = 12,
        Rgba = 14,
        Abgr = 15,
        Rgba2 = 16,
        Rgba1 = 17,
        Rgba0 = 21,
        CompressedRgRgtc2 = 22
    }

    public enum TextureType
    {
        Dds = 0,
        Gxt = 1
    }

    public Nut(KaitaiStream p__io, KaitaiStruct p__parent = null, Nut p__root = null)
        : base(p__io)
    {
        M_Parent = p__parent;
        M_Root = p__root ?? this;
        _read();
    }

    public NutSignature Signature { get; private set; }

    public NutBody Body { get; private set; }

    public Nut M_Root { get; }

    public KaitaiStruct M_Parent { get; }

    public static Nut FromFile(string fileName)
    {
        return new Nut(new KaitaiStream(fileName));
    }

    private void _read()
    {
        Signature = (NutSignature)m_io.ReadU4be();
        if (
            !(
                Signature == NutSignature.Ntp3
                || Signature == NutSignature.Ntwd
                || Signature == NutSignature.Ntlx
            )
        )
            throw new ValidationNotAnyOfError(Signature, M_Io, "/seq/0");
        Body = new NutBody(m_io, this, M_Root);
    }

    public class Nothing : KaitaiStruct
    {
        public Nothing(KaitaiStream p__io, KaitaiStruct p__parent = null, Nut p__root = null)
            : base(p__io)
        {
            M_Parent = p__parent;
            M_Root = p__root;
            _read();
        }

        public Nut M_Root { get; }

        public KaitaiStruct M_Parent { get; }

        public static Nothing FromFile(string fileName)
        {
            return new Nothing(new KaitaiStream(fileName));
        }

        private void _read() { }
    }

    public partial class NutBody : KaitaiStruct
    {
        private bool? m_isLe;

        public NutBody(KaitaiStream p__io, Nut p__parent = null, Nut p__root = null)
            : base(p__io)
        {
            M_Parent = p__parent;
            M_Root = p__root;
            _read();
        }

        public NutHeader Header { get; private set; }

        public List<Texture> Textures { get; private set; }

        public Nut M_Root { get; }

        public Nut M_Parent { get; }

        public byte[] M_RawHeader { get; private set; }

        public static NutBody FromFile(string fileName)
        {
            return new NutBody(new KaitaiStream(fileName));
        }

        private void _read()
        {
            switch (M_Root.Signature)
            {
                case NutSignature.Ntwd:
                {
                    m_isLe = true;
                    break;
                }
                case NutSignature.Ntlx:
                {
                    m_isLe = true;
                    break;
                }
                default:
                {
                    m_isLe = false;
                    break;
                }
            }

            if (m_isLe == null)
                throw new UndecidedEndiannessError();
            if (m_isLe == true)
                _readLE();
            else
                _readBE();
        }

        private void _readLE()
        {
            M_RawHeader = m_io.ReadBytes(12);
            var io___raw_header = new KaitaiStream(M_RawHeader);
            Header = new NutHeader(io___raw_header, this, M_Root, m_isLe);
            Textures = new List<Texture>();
            for (var i = 0; i < Header.Count; i++)
                Textures.Add(new Texture(i, m_io, this, M_Root, m_isLe));
        }

        private void _readBE()
        {
            M_RawHeader = m_io.ReadBytes(12);
            var io___raw_header = new KaitaiStream(M_RawHeader);
            Header = new NutHeader(io___raw_header, this, M_Root, m_isLe);
            Textures = new List<Texture>();
            for (var i = 0; i < Header.Count; i++)
                Textures.Add(new Texture(i, m_io, this, M_Root, m_isLe));
        }

        public class Cubemap : KaitaiStruct
        {
            private readonly bool? m_isLe;

            public Cubemap(
                KaitaiStream p__io,
                TextureInfo p__parent = null,
                Nut p__root = null,
                bool? isLe = null
            )
                : base(p__io)
            {
                M_Parent = p__parent;
                M_Root = p__root;
                m_isLe = isLe;
                _read();
            }

            public Nothing Padding { get; private set; }

            /// <summary>
            ///     +x -x +y -y +z -z
            /// </summary>
            public List<bool> Sides { get; private set; }

            public bool IsCubemap { get; private set; }

            public Nut M_Root { get; }

            public TextureInfo M_Parent { get; }

            public byte[] M_RawPadding { get; private set; }

            public static Cubemap FromFile(string fileName)
            {
                return new Cubemap(new KaitaiStream(fileName));
            }

            private void _read()
            {
                if (m_isLe == null)
                    throw new UndecidedEndiannessError();
                if (m_isLe == true)
                    _readLE();
                else
                    _readBE();
            }

            private void _readLE()
            {
                M_RawPadding = m_io.ReadBytes(1);
                var io___raw_padding = new KaitaiStream(M_RawPadding);
                Padding = new Nothing(io___raw_padding, this, M_Root);
                Sides = new List<bool>();
                for (var i = 0; i < 6; i++)
                    Sides.Add(m_io.ReadBitsIntBe(1) != 0);
                IsCubemap = m_io.ReadBitsIntBe(1) != 0;
            }

            private void _readBE()
            {
                M_RawPadding = m_io.ReadBytes(1);
                var io___raw_padding = new KaitaiStream(M_RawPadding);
                Padding = new Nothing(io___raw_padding, this, M_Root);
                Sides = new List<bool>();
                for (var i = 0; i < 6; i++)
                    Sides.Add(m_io.ReadBitsIntBe(1) != 0);
                IsCubemap = m_io.ReadBitsIntBe(1) != 0;
            }
        }

        public class NutHeader : KaitaiStruct
        {
            private readonly bool? m_isLe;

            public NutHeader(
                KaitaiStream p__io,
                NutBody p__parent = null,
                Nut p__root = null,
                bool? isLe = null
            )
                : base(p__io)
            {
                M_Parent = p__parent;
                M_Root = p__root;
                m_isLe = isLe;
                _read();
            }

            public ushort Version { get; private set; }

            public ushort Count { get; private set; }

            public Nut M_Root { get; }

            public NutBody M_Parent { get; }

            public static NutHeader FromFile(string fileName)
            {
                return new NutHeader(new KaitaiStream(fileName));
            }

            private void _read()
            {
                if (m_isLe == null)
                    throw new UndecidedEndiannessError();
                if (m_isLe == true)
                    _readLE();
                else
                    _readBE();
            }

            private void _readLE()
            {
                Version = m_io.ReadU2le();
                if (!(Version == 2))
                    throw new ValidationNotAnyOfError(
                        Version,
                        M_Io,
                        "/types/nut_body/types/nut_header/seq/0"
                    );
                Count = m_io.ReadU2le();
            }

            private void _readBE()
            {
                Version = m_io.ReadU2be();
                if (!(Version == 2))
                    throw new ValidationNotAnyOfError(
                        Version,
                        M_Io,
                        "/types/nut_body/types/nut_header/seq/0"
                    );
                Count = m_io.ReadU2be();
            }
        }

        public class Gidx : KaitaiStruct
        {
            private readonly bool? m_isLe;

            public Gidx(
                KaitaiStream p__io,
                Texture p__parent = null,
                Nut p__root = null,
                bool? isLe = null
            )
                : base(p__io)
            {
                M_Parent = p__parent;
                M_Root = p__root;
                m_isLe = isLe;
                _read();
            }

            public byte[] Signature { get; private set; }

            public uint Version { get; private set; }

            public uint Version2 { get; private set; }

            public Nothing Unknown { get; private set; }

            public byte[] Type { get; private set; }

            public Nothing Unknown2 { get; private set; }

            public uint HashId { get; private set; }

            public Nut M_Root { get; }

            public Texture M_Parent { get; }

            public byte[] M_RawUnknown { get; private set; }

            public byte[] M_RawUnknown2 { get; private set; }

            public static Gidx FromFile(string fileName)
            {
                return new Gidx(new KaitaiStream(fileName));
            }

            private void _read()
            {
                if (m_isLe == null)
                    throw new UndecidedEndiannessError();
                if (m_isLe == true)
                    _readLE();
                else
                    _readBE();
            }

            private void _readLE()
            {
                Signature = m_io.ReadBytes(4);
                if (
                    !(KaitaiStream.ByteArrayCompare(Signature, new byte[] { 101, 88, 116, 0 }) == 0)
                )
                    throw new ValidationNotEqualError(
                        new byte[] { 101, 88, 116, 0 },
                        Signature,
                        M_Io,
                        "/types/nut_body/types/gidx/seq/0"
                    );
                Version = m_io.ReadU4le();
                Version2 = m_io.ReadU4le();
                M_RawUnknown = m_io.ReadBytes(4);
                var io___raw_unknown = new KaitaiStream(M_RawUnknown);
                Unknown = new Nothing(io___raw_unknown, this, M_Root);
                Type = m_io.ReadBytes(4);
                if (!(KaitaiStream.ByteArrayCompare(Type, new byte[] { 71, 73, 68, 88 }) == 0))
                    throw new ValidationNotEqualError(
                        new byte[] { 71, 73, 68, 88 },
                        Type,
                        M_Io,
                        "/types/nut_body/types/gidx/seq/4"
                    );
                M_RawUnknown2 = m_io.ReadBytes(4);
                var io___raw_unknown2 = new KaitaiStream(M_RawUnknown2);
                Unknown2 = new Nothing(io___raw_unknown2, this, M_Root);
                HashId = m_io.ReadU4le();
            }

            private void _readBE()
            {
                Signature = m_io.ReadBytes(4);
                if (
                    !(KaitaiStream.ByteArrayCompare(Signature, new byte[] { 101, 88, 116, 0 }) == 0)
                )
                    throw new ValidationNotEqualError(
                        new byte[] { 101, 88, 116, 0 },
                        Signature,
                        M_Io,
                        "/types/nut_body/types/gidx/seq/0"
                    );
                Version = m_io.ReadU4be();
                Version2 = m_io.ReadU4be();
                M_RawUnknown = m_io.ReadBytes(4);
                var io___raw_unknown = new KaitaiStream(M_RawUnknown);
                Unknown = new Nothing(io___raw_unknown, this, M_Root);
                Type = m_io.ReadBytes(4);
                if (!(KaitaiStream.ByteArrayCompare(Type, new byte[] { 71, 73, 68, 88 }) == 0))
                    throw new ValidationNotEqualError(
                        new byte[] { 71, 73, 68, 88 },
                        Type,
                        M_Io,
                        "/types/nut_body/types/gidx/seq/4"
                    );
                M_RawUnknown2 = m_io.ReadBytes(4);
                var io___raw_unknown2 = new KaitaiStream(M_RawUnknown2);
                Unknown2 = new Nothing(io___raw_unknown2, this, M_Root);
                HashId = m_io.ReadU4be();
            }
        }

        public class TextureInfo : KaitaiStruct
        {
            private readonly bool? m_isLe;
            private int _cubemapCount;
            private int _surfaceCount;
            private bool f_cubemapCount;
            private bool f_surfaceCount;

            public TextureInfo(
                KaitaiStream p__io,
                Texture p__parent = null,
                Nut p__root = null,
                bool? isLe = null
            )
                : base(p__io)
            {
                M_Parent = p__parent;
                M_Root = p__root;
                m_isLe = isLe;
                f_cubemapCount = false;
                f_surfaceCount = false;
                _read();
            }

            public int CubemapCount
            {
                get
                {
                    if (f_cubemapCount)
                        return _cubemapCount;
                    _cubemapCount =
                        (Cubemap.Sides[0] ? 1 : 0)
                        + (Cubemap.Sides[1] ? 1 : 0)
                        + (Cubemap.Sides[2] ? 1 : 0)
                        + (Cubemap.Sides[3] ? 1 : 0)
                        + (Cubemap.Sides[4] ? 1 : 0)
                        + (Cubemap.Sides[5] ? 1 : 0);
                    f_cubemapCount = true;
                    return _cubemapCount;
                }
            }

            public int SurfaceCount
            {
                get
                {
                    if (f_surfaceCount)
                        return _surfaceCount;
                    _surfaceCount = CubemapCount == 0 ? 1 : CubemapCount;
                    f_surfaceCount = true;
                    return _surfaceCount;
                }
            }

            public Nothing Padding { get; private set; }

            public byte MipmapCount { get; private set; }

            public Nothing Padding3 { get; private set; }

            public PixelFormat PixelFormat { get; private set; }

            public ushort Width { get; private set; }

            public ushort Height { get; private set; }

            /// <summary>
            ///     It's a little unclear if this is actually that
            /// </summary>
            public TextureType TextureType { get; private set; }

            public Cubemap Cubemap { get; private set; }

            public uint DataOffset { get; private set; }

            public Nothing Padding4 { get; private set; }

            public ushort? CubemapSize1 { get; private set; }

            public ushort? CubemapSize2 { get; private set; }

            public Nothing CubemapPadding { get; private set; }

            public List<uint> MipmapSizes { get; private set; }

            public Nut M_Root { get; }

            public Texture M_Parent { get; }

            public byte[] M_RawPadding { get; private set; }

            public byte[] M_RawPadding3 { get; private set; }

            public byte[] M_RawCubemap { get; private set; }

            public byte[] M_RawPadding4 { get; private set; }

            public byte[] M_RawCubemapPadding { get; private set; }

            public static TextureInfo FromFile(string fileName)
            {
                return new TextureInfo(new KaitaiStream(fileName));
            }

            private void _read()
            {
                if (m_isLe == null)
                    throw new UndecidedEndiannessError();
                if (m_isLe == true)
                    _readLE();
                else
                    _readBE();
            }

            private void _readLE()
            {
                M_RawPadding = m_io.ReadBytes(1);
                var io___raw_padding = new KaitaiStream(M_RawPadding);
                Padding = new Nothing(io___raw_padding, this, M_Root);
                MipmapCount = m_io.ReadU1();
                M_RawPadding3 = m_io.ReadBytes(1);
                var io___raw_padding3 = new KaitaiStream(M_RawPadding3);
                Padding3 = new Nothing(io___raw_padding3, this, M_Root);
                PixelFormat = (PixelFormat)m_io.ReadU1();
                Width = m_io.ReadU2le();
                Height = m_io.ReadU2le();
                TextureType = (TextureType)m_io.ReadU4le();
                if (!(TextureType == TextureType.Dds))
                    throw new ValidationNotAnyOfError(
                        TextureType,
                        M_Io,
                        "/types/nut_body/types/texture_info/seq/6"
                    );
                M_RawCubemap = m_io.ReadBytes(4);
                var io___raw_cubemap = new KaitaiStream(M_RawCubemap);
                Cubemap = new Cubemap(io___raw_cubemap, this, M_Root, m_isLe);
                DataOffset = m_io.ReadU4le();
                M_RawPadding4 = m_io.ReadBytes(12);
                var io___raw_padding4 = new KaitaiStream(M_RawPadding4);
                Padding4 = new Nothing(io___raw_padding4, this, M_Root);
                if (Cubemap.IsCubemap)
                    CubemapSize1 = m_io.ReadU2le();
                if (Cubemap.IsCubemap)
                    CubemapSize2 = m_io.ReadU2le();
                if (Cubemap.IsCubemap)
                {
                    M_RawCubemapPadding = m_io.ReadBytes(8);
                    var io___raw_cubemapPadding = new KaitaiStream(M_RawCubemapPadding);
                    CubemapPadding = new Nothing(io___raw_cubemapPadding, this, M_Root);
                }

                MipmapSizes = new List<uint>();
                for (var i = 0; i < MipmapCount - 1; i++)
                    MipmapSizes.Add(m_io.ReadU4le());
            }

            private void _readBE()
            {
                M_RawPadding = m_io.ReadBytes(1);
                var io___raw_padding = new KaitaiStream(M_RawPadding);
                Padding = new Nothing(io___raw_padding, this, M_Root);
                MipmapCount = m_io.ReadU1();
                M_RawPadding3 = m_io.ReadBytes(1);
                var io___raw_padding3 = new KaitaiStream(M_RawPadding3);
                Padding3 = new Nothing(io___raw_padding3, this, M_Root);
                PixelFormat = (PixelFormat)m_io.ReadU1();
                Width = m_io.ReadU2be();
                Height = m_io.ReadU2be();
                TextureType = (TextureType)m_io.ReadU4be();
                if (!(TextureType == TextureType.Dds))
                    throw new ValidationNotAnyOfError(
                        TextureType,
                        M_Io,
                        "/types/nut_body/types/texture_info/seq/6"
                    );
                M_RawCubemap = m_io.ReadBytes(4);
                var io___raw_cubemap = new KaitaiStream(M_RawCubemap);
                Cubemap = new Cubemap(io___raw_cubemap, this, M_Root, m_isLe);
                DataOffset = m_io.ReadU4be();
                M_RawPadding4 = m_io.ReadBytes(12);
                var io___raw_padding4 = new KaitaiStream(M_RawPadding4);
                Padding4 = new Nothing(io___raw_padding4, this, M_Root);
                if (Cubemap.IsCubemap)
                    CubemapSize1 = m_io.ReadU2be();
                if (Cubemap.IsCubemap)
                    CubemapSize2 = m_io.ReadU2be();
                if (Cubemap.IsCubemap)
                {
                    M_RawCubemapPadding = m_io.ReadBytes(8);
                    var io___raw_cubemapPadding = new KaitaiStream(M_RawCubemapPadding);
                    CubemapPadding = new Nothing(io___raw_cubemapPadding, this, M_Root);
                }

                MipmapSizes = new List<uint>();
                for (var i = 0; i < MipmapCount - 1; i++)
                    MipmapSizes.Add(m_io.ReadU4be());
            }
        }

        public class TextureSurface : KaitaiStruct
        {
            private readonly bool? m_isLe;

            public TextureSurface(
                KaitaiStream p__io,
                TextureSurfaces p__parent = null,
                Nut p__root = null,
                bool? isLe = null
            )
                : base(p__io)
            {
                M_Parent = p__parent;
                M_Root = p__root;
                m_isLe = isLe;
                _read();
            }

            public byte[] Mipmaps { get; private set; }

            public Nut M_Root { get; }

            public TextureSurfaces M_Parent { get; }

            public static TextureSurface FromFile(string fileName)
            {
                return new TextureSurface(new KaitaiStream(fileName));
            }

            private void _read()
            {
                if (m_isLe == null)
                    throw new UndecidedEndiannessError();
                if (m_isLe == true)
                    _readLE();
                else
                    _readBE();
            }

            private void _readLE()
            {
                Mipmaps = m_io.ReadBytes(
                    M_Parent.M_Parent.M_Parent.DataSize
                        / M_Parent.M_Parent.M_Parent.TextureInfo.SurfaceCount
                );
            }

            private void _readBE()
            {
                Mipmaps = m_io.ReadBytes(
                    M_Parent.M_Parent.M_Parent.DataSize
                        / M_Parent.M_Parent.M_Parent.TextureInfo.SurfaceCount
                );
            }
        }

        public class Texture : KaitaiStruct
        {
            private readonly bool? m_isLe;

            public Texture(
                int p_i,
                KaitaiStream p__io,
                NutBody p__parent = null,
                Nut p__root = null,
                bool? isLe = null
            )
                : base(p__io)
            {
                M_Parent = p__parent;
                M_Root = p__root;
                m_isLe = isLe;
                I = p_i;
                _read();
            }

            public uint Size { get; private set; }

            public Nothing Padding { get; private set; }

            public uint DataSize { get; private set; }

            public ushort HeaderSize { get; private set; }

            public TextureData TextureData { get; private set; }

            public TextureInfo TextureInfo { get; private set; }

            public Gidx Gidx { get; private set; }

            public int I { get; }

            public Nut M_Root { get; }

            public NutBody M_Parent { get; }

            public byte[] M_RawPadding { get; private set; }

            public byte[] M_RawTextureData { get; private set; }

            public byte[] M_RawTextureInfo { get; private set; }

            public byte[] M_RawGidx { get; private set; }

            private void _read()
            {
                if (m_isLe == null)
                    throw new UndecidedEndiannessError();
                if (m_isLe == true)
                    _readLE();
                else
                    _readBE();
            }

            private void _readLE()
            {
                Size = m_io.ReadU4le();
                M_RawPadding = m_io.ReadBytes(4);
                var io___raw_padding = new KaitaiStream(M_RawPadding);
                Padding = new Nothing(io___raw_padding, this, M_Root);
                DataSize = m_io.ReadU4le();
                HeaderSize = m_io.ReadU2le();
                M_RawTextureData = m_io.ReadBytes(2);
                var io___raw_textureData = new KaitaiStream(M_RawTextureData);
                TextureData = new TextureData(
                    (int)M_Root.M_Io.Pos,
                    io___raw_textureData,
                    this,
                    M_Root,
                    m_isLe
                );
                M_RawTextureInfo = m_io.ReadBytes(HeaderSize - 16 - 32);
                var io___raw_textureInfo = new KaitaiStream(M_RawTextureInfo);
                TextureInfo = new TextureInfo(io___raw_textureInfo, this, M_Root, m_isLe);
                M_RawGidx = m_io.ReadBytes(32);
                var io___raw_gidx = new KaitaiStream(M_RawGidx);
                Gidx = new Gidx(io___raw_gidx, this, M_Root, m_isLe);
            }

            private void _readBE()
            {
                Size = m_io.ReadU4be();
                M_RawPadding = m_io.ReadBytes(4);
                var io___raw_padding = new KaitaiStream(M_RawPadding);
                Padding = new Nothing(io___raw_padding, this, M_Root);
                DataSize = m_io.ReadU4be();
                HeaderSize = m_io.ReadU2be();
                M_RawTextureData = m_io.ReadBytes(2);
                var io___raw_textureData = new KaitaiStream(M_RawTextureData);
                TextureData = new TextureData(
                    (int)M_Root.M_Io.Pos,
                    io___raw_textureData,
                    this,
                    M_Root,
                    m_isLe
                );
                M_RawTextureInfo = m_io.ReadBytes(HeaderSize - 16 - 32);
                var io___raw_textureInfo = new KaitaiStream(M_RawTextureInfo);
                TextureInfo = new TextureInfo(io___raw_textureInfo, this, M_Root, m_isLe);
                M_RawGidx = m_io.ReadBytes(32);
                var io___raw_gidx = new KaitaiStream(M_RawGidx);
                Gidx = new Gidx(io___raw_gidx, this, M_Root, m_isLe);
            }
        }

        public class TextureData : KaitaiStruct
        {
            private readonly bool? m_isLe;
            private int _dataOffset;
            private TextureSurfaces _surfaces;
            private bool f_dataOffset;
            private bool f_surfaces;

            public TextureData(
                int p_offset,
                KaitaiStream p__io,
                Texture p__parent = null,
                Nut p__root = null,
                bool? isLe = null
            )
                : base(p__io)
            {
                M_Parent = p__parent;
                M_Root = p__root;
                m_isLe = isLe;
                Offset = p_offset;
                f_dataOffset = false;
                f_surfaces = false;
                _read();
            }

            public int DataOffset
            {
                get
                {
                    if (f_dataOffset)
                        return _dataOffset;
                    _dataOffset = (int)(Offset - 16 + M_Parent.TextureInfo.DataOffset);
                    f_dataOffset = true;
                    return _dataOffset;
                }
            }

            public TextureSurfaces Surfaces
            {
                get
                {
                    if (f_surfaces)
                        return _surfaces;
                    var io = M_Root.M_Io;
                    var _pos = io.Pos;
                    io.Seek(DataOffset);
                    if (m_isLe == true)
                    {
                        M_RawSurfaces = io.ReadBytes(M_Parent.DataSize);
                        var io___raw_surfaces = new KaitaiStream(M_RawSurfaces);
                        _surfaces = new TextureSurfaces(io___raw_surfaces, this, M_Root, m_isLe);
                    }
                    else
                    {
                        M_RawSurfaces = io.ReadBytes(M_Parent.DataSize);
                        var io___raw_surfaces = new KaitaiStream(M_RawSurfaces);
                        _surfaces = new TextureSurfaces(io___raw_surfaces, this, M_Root, m_isLe);
                    }

                    io.Seek(_pos);
                    f_surfaces = true;
                    return _surfaces;
                }
            }

            public int Offset { get; }

            public Nut M_Root { get; }

            public Texture M_Parent { get; }

            public byte[] M_RawSurfaces { get; private set; }

            private void _read()
            {
                if (m_isLe == null)
                    throw new UndecidedEndiannessError();
                if (m_isLe == true)
                    _readLE();
                else
                    _readBE();
            }

            private void _readLE() { }

            private void _readBE() { }
        }

        public class TextureSurfaces : KaitaiStruct
        {
            private readonly bool? m_isLe;

            public TextureSurfaces(
                KaitaiStream p__io,
                TextureData p__parent = null,
                Nut p__root = null,
                bool? isLe = null
            )
                : base(p__io)
            {
                M_Parent = p__parent;
                M_Root = p__root;
                m_isLe = isLe;
                _read();
            }

            public List<TextureSurface> Surfaces { get; private set; }

            public Nut M_Root { get; }

            public TextureData M_Parent { get; }

            public static TextureSurfaces FromFile(string fileName)
            {
                return new TextureSurfaces(new KaitaiStream(fileName));
            }

            private void _read()
            {
                if (m_isLe == null)
                    throw new UndecidedEndiannessError();
                if (m_isLe == true)
                    _readLE();
                else
                    _readBE();
            }

            private void _readLE()
            {
                Surfaces = new List<TextureSurface>();
                for (var i = 0; i < M_Parent.M_Parent.TextureInfo.SurfaceCount; i++)
                    Surfaces.Add(new TextureSurface(m_io, this, M_Root, m_isLe));
            }

            private void _readBE()
            {
                Surfaces = new List<TextureSurface>();
                for (var i = 0; i < M_Parent.M_Parent.TextureInfo.SurfaceCount; i++)
                    Surfaces.Add(new TextureSurface(m_io, this, M_Root, m_isLe));
            }
        }
    }
}
