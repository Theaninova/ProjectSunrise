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

public class Mdl : KaitaiStruct
{
    private List<Model> _models;
    private bool f_models;

    public Mdl(KaitaiStream p__io, KaitaiStruct p__parent = null, Mdl p__root = null)
        : base(p__io)
    {
        M_Parent = p__parent;
        M_Root = p__root ?? this;
        f_models = false;
        _read();
    }

    public List<Model> Models
    {
        get
        {
            if (f_models)
                return _models;
            var io = M_Root.M_Io;
            _models = new List<Model>();
            for (var i = 0; i < Xmd.Header.Count; i++)
                _models.Add(new Model(i, io, this, M_Root));
            f_models = true;
            return _models;
        }
    }

    public Xmd Xmd { get; private set; }

    public Mdl M_Root { get; }

    public KaitaiStruct M_Parent { get; }

    public static Mdl FromFile(string fileName)
    {
        return new Mdl(new KaitaiStream(fileName));
    }

    private void _read()
    {
        Xmd = new Xmd(m_io);
    }

    public class Model : KaitaiStruct
    {
        private uint _id;
        private Nud _nud;
        private uint _size;
        private bool f_id;
        private bool f_nud;
        private bool f_size;

        public Model(int p_i, KaitaiStream p__io, Mdl p__parent = null, Mdl p__root = null)
            : base(p__io)
        {
            M_Parent = p__parent;
            M_Root = p__root;
            I = p_i;
            f_id = false;
            f_size = false;
            f_nud = false;
            _read();
        }

        public uint Id
        {
            get
            {
                if (f_id)
                    return _id;
                _id = M_Root.Xmd.ItemIds[I];
                f_id = true;
                return _id;
            }
        }

        public uint Size
        {
            get
            {
                if (f_size)
                    return _size;
                _size = M_Root.Xmd.Lengths[I];
                f_size = true;
                return _size;
            }
        }

        public Nud Nud
        {
            get
            {
                if (f_nud)
                    return _nud;
                var _pos = m_io.Pos;
                m_io.Seek(M_Root.Xmd.Positions[I]);
                M_RawNud = m_io.ReadBytes(Size);
                var io___raw_nud = new KaitaiStream(M_RawNud);
                _nud = new Nud(io___raw_nud);
                m_io.Seek(_pos);
                f_nud = true;
                return _nud;
            }
        }

        public int I { get; }

        public Mdl M_Root { get; }

        public Mdl M_Parent { get; }

        public byte[] M_RawNud { get; private set; }

        private void _read() { }
    }
}
