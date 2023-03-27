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

public class Xmd : KaitaiStruct
{
    public enum ListCounts
    {
        LmbTexturesResources = 1,
        PosLenId = 3
    }

    public Xmd(KaitaiStream p__io, KaitaiStruct p__parent = null, Xmd p__root = null)
        : base(p__io)
    {
        M_Parent = p__parent;
        M_Root = p__root ?? this;
        _read();
    }

    public XmdHeader Header { get; private set; }

    public List<uint> Positions { get; private set; }

    public List<uint> Lengths { get; private set; }

    public List<uint> ItemIds { get; private set; }

    public Xmd M_Root { get; }

    public KaitaiStruct M_Parent { get; }

    public static Xmd FromFile(string fileName)
    {
        return new Xmd(new KaitaiStream(fileName));
    }

    private void _read()
    {
        Header = new XmdHeader(m_io, this, M_Root);
        Positions = new List<uint>();
        for (var i = 0; i < Header.AlignedCount; i++)
            Positions.Add(m_io.ReadU4le());
        Lengths = new List<uint>();
        for (var i = 0; i < Header.AlignedCount; i++)
            Lengths.Add(m_io.ReadU4le());
        ItemIds = new List<uint>();
        for (var i = 0; i < Header.AlignedCount; i++)
            ItemIds.Add(m_io.ReadU4le());
    }

    public class XmdHeader : KaitaiStruct
    {
        private int _alignedCount;
        private bool f_alignedCount;

        public XmdHeader(KaitaiStream p__io, Xmd p__parent = null, Xmd p__root = null)
            : base(p__io)
        {
            M_Parent = p__parent;
            M_Root = p__root;
            f_alignedCount = false;
            _read();
        }

        public int AlignedCount
        {
            get
            {
                if (f_alignedCount)
                    return _alignedCount;
                _alignedCount = (int)(Count + KaitaiStream.Mod(4 - Count, 4));
                f_alignedCount = true;
                return _alignedCount;
            }
        }

        public byte[] Signature { get; private set; }

        public ListCounts Layout { get; private set; }

        public uint Count { get; private set; }

        public Xmd M_Root { get; }

        public Xmd M_Parent { get; }

        public static XmdHeader FromFile(string fileName)
        {
            return new XmdHeader(new KaitaiStream(fileName));
        }

        private void _read()
        {
            Signature = m_io.ReadBytes(8);
            if (
                !(
                    KaitaiStream.ByteArrayCompare(
                        Signature,
                        new byte[] { 88, 77, 68, 0, 48, 48, 49, 0 }
                    ) == 0
                )
            )
                throw new ValidationNotEqualError(
                    new byte[] { 88, 77, 68, 0, 48, 48, 49, 0 },
                    Signature,
                    M_Io,
                    "/types/xmd_header/seq/0"
                );
            Layout = (ListCounts)m_io.ReadU4le();
            Count = m_io.ReadU4le();
        }
    }
}
