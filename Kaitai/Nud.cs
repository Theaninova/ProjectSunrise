// This is a generated file! Please edit source .ksy file and use kaitai-struct-compiler to rebuild

using System.Collections.Generic;
using Kaitai;

namespace SunriseMono.Kaitai
{
    public partial class Nud : KaitaiStruct
    {
        public static Nud FromFile(string fileName)
        {
            return new Nud(new KaitaiStream(fileName));
        }


        public enum BoneFlags
        {
            Unbound = 0,
            Weighted = 4,
            SingleBound = 8,
        }

        public enum FilterMode
        {
            LinearMipmapLinear = 0,
            Nearest = 1,
            Linear = 2,
            NearestMipmapLinear = 3,
        }

        public enum AlphaFunction
        {
            NoAlpha = 0,
            Alpha1 = 4,
            Alpha2 = 6,
        }

        public enum MipDetail
        {
            Level1 = 1,
            Level1Off = 2,
            Level4 = 3,
            Level4Anisotropic = 4,
            Level4Trilinear = 5,
            Level4TrilinearAnisotripic = 6,
        }

        public enum BoneSize
        {
            NoBones = 0,
            Float = 1,
            HalfFloat = 2,
            Byte = 4,
        }

        public enum Signature
        {
            Ndwd = 1146569806,
            Ndp3 = 1313099827,
        }

        public enum Version
        {
            V2 = 2,
        }

        public enum WrapMode
        {
            Repeat = 1,
            MirroredRepeat = 2,
            ClampToEdge = 3,
        }

        public enum MapMode
        {
            TexCoord = 0,
            EnvCamera = 7424,
            Projection = 7680,
            EnvLight = 7885,
            EnvSpec = 7936,
        }

        public enum CullMode
        {
            None = 0,
            InsidePokken = 2,
            Outside = 1028,
            Inside = 1029,
        }

        public enum VertexColorSize
        {
            NoVertexColors = 0,
            Byte = 1,
            HalfFloat = 2,
        }

        public enum NormalSize
        {
            Float = 0,
            HalfFloat = 1,
        }

        public enum NormalType
        {
            NoNormals = 0,
            NormalsFloat = 1,
            NormalsR1 = 2,
            NormalsTanBitan = 3,
        }
        public Nud(KaitaiStream p__io, KaitaiStruct p__parent = null, Nud p__root = null) : base(p__io)
        {
            m_parent = p__parent;
            m_root = p__root ?? this;
            _read();
        }
        private void _read()
        {
            _magic = ((Signature) m_io.ReadU4le());
            if (!( ((Magic == Signature.Ndwd)) ))
            {
                throw new ValidationNotAnyOfError(Magic, M_Io, "/seq/0");
            }
            _header = new NudHeader(m_io, this, m_root);
            _meshes = new List<Mesh>();
            for (var i = 0; i < Header.PolysetCount; i++)
            {
                _meshes.Add(new Mesh(i, m_io, this, m_root));
            }
            _parts = new List<NudParts>();
            for (var i = 0; i < Header.PolysetCount; i++)
            {
                _parts.Add(new NudParts(Meshes[i].PartCount, m_io, this, m_root));
            }
        }
        public partial class Vertex : KaitaiStruct
        {
            public static Vertex FromFile(string fileName)
            {
                return new Vertex(new KaitaiStream(fileName));
            }

            public Vertex(KaitaiStream p__io, Nud.Part p__parent = null, Nud p__root = null) : base(p__io)
            {
                m_parent = p__parent;
                m_root = p__root;
                _read();
            }
            private void _read()
            {
                _position = new List<float>();
                for (var i = 0; i < 3; i++)
                {
                    _position.Add(m_io.ReadF4le());
                }
                if ( ((M_Parent.NormalType == Nud.NormalType.NoNormals) || ( ((M_Parent.NormalType == Nud.NormalType.NormalsTanBitan) && (!(M_Parent.NormalHalfFloat))) )) ) {
                    _padding = m_io.ReadBytes(4);
                }
                if (M_Parent.NormalType != Nud.NormalType.NoNormals) {
                    _normal = new List<double>();
                    for (var i = 0; i < 4; i++)
                    {
                        {
                            bool on = M_Parent.NormalHalfFloat;
                            if (on == true)
                            {
                                _normal.Add(m_io.ReadU2le());
                            }
                            else if (on == false)
                            {
                                _normal.Add(m_io.ReadF4le());
                            }
                        }
                    }
                }
                if ( (( ((M_Parent.NormalType == Nud.NormalType.NormalsR1) || (M_Parent.NormalType == Nud.NormalType.NormalsFloat)) ) && (!(M_Parent.NormalHalfFloat))) ) {
                    _r1 = new List<byte[]>();
                    for (var i = 0; i < (M_Parent.NormalType == Nud.NormalType.NormalsFloat ? 1 : 8); i++)
                    {
                        _r1.Add(m_io.ReadBytes(4));
                    }
                }
                if (M_Parent.NormalType == Nud.NormalType.NormalsTanBitan) {
                    _tan = new List<double>();
                    for (var i = 0; i < 4; i++)
                    {
                        {
                            bool on = M_Parent.NormalHalfFloat;
                            if (on == true)
                            {
                                _tan.Add(m_io.ReadU2le());
                            }
                            else if (on == false)
                            {
                                _tan.Add(m_io.ReadF4le());
                            }
                        }
                    }
                }
                if (M_Parent.NormalType == Nud.NormalType.NormalsTanBitan) {
                    _bitan = new List<double>();
                    for (var i = 0; i < 4; i++)
                    {
                        {
                            bool on = M_Parent.NormalHalfFloat;
                            if (on == true)
                            {
                                _bitan.Add(m_io.ReadU2le());
                            }
                            else if (on == false)
                            {
                                _bitan.Add(m_io.ReadF4le());
                            }
                        }
                    }
                }
                if ( ((M_Parent.BoneSize == Nud.BoneSize.NoBones) && (M_Parent.VertexColorSize != Nud.VertexColorSize.NoVertexColors)) ) {
                    _colors = new List<byte>();
                    for (var i = 0; i < 4; i++)
                    {
                        _colors.Add(m_io.ReadU1());
                    }
                }
                if (M_Parent.BoneSize == Nud.BoneSize.NoBones) {
                    _uv = new List<double>();
                    for (var i = 0; i < (((int) (M_Parent.UvChannelCount)) * 2); i++)
                    {
                        {
                            bool on = M_Parent.UvFloat;
                            if (on == true)
                            {
                                _uv.Add(m_io.ReadF4le());
                            }
                            else if (on == false)
                            {
                                _uv.Add(m_io.ReadU2le());
                            }
                        }
                    }
                }
                if (M_Parent.BoneSize != Nud.BoneSize.NoBones) {
                    _boneId = new List<uint>();
                    for (var i = 0; i < 4; i++)
                    {
                        switch (M_Parent.BoneSize) {
                        case Nud.BoneSize.Float: {
                            _boneId.Add(m_io.ReadU4le());
                            break;
                        }
                        case Nud.BoneSize.HalfFloat: {
                            _boneId.Add(m_io.ReadU2le());
                            break;
                        }
                        case Nud.BoneSize.Byte: {
                            _boneId.Add(m_io.ReadU1());
                            break;
                        }
                        }
                    }
                }
                if (M_Parent.BoneSize != Nud.BoneSize.NoBones) {
                    _boneWeight = new List<double>();
                    for (var i = 0; i < 4; i++)
                    {
                        switch (M_Parent.BoneSize) {
                        case Nud.BoneSize.Float: {
                            _boneWeight.Add(m_io.ReadF4le());
                            break;
                        }
                        case Nud.BoneSize.HalfFloat: {
                            _boneWeight.Add(m_io.ReadU2le());
                            break;
                        }
                        case Nud.BoneSize.Byte: {
                            _boneWeight.Add(m_io.ReadU1());
                            break;
                        }
                        }
                    }
                }
            }
            private List<float> _position;
            private byte[] _padding;
            private List<double> _normal;
            private List<byte[]> _r1;
            private List<double> _tan;
            private List<double> _bitan;
            private List<byte> _colors;
            private List<double> _uv;
            private List<uint> _boneId;
            private List<double> _boneWeight;
            private Nud m_root;
            private Nud.Part m_parent;
            public List<float> Position { get { return _position; } }
            public byte[] Padding { get { return _padding; } }
            public List<double> Normal { get { return _normal; } }

            /// <summary>
            /// no idea what this is
            /// </summary>
            public List<byte[]> R1 { get { return _r1; } }
            public List<double> Tan { get { return _tan; } }
            public List<double> Bitan { get { return _bitan; } }
            public List<byte> Colors { get { return _colors; } }
            public List<double> Uv { get { return _uv; } }
            public List<uint> BoneId { get { return _boneId; } }
            public List<double> BoneWeight { get { return _boneWeight; } }
            public Nud M_Root { get { return m_root; } }
            public Nud.Part M_Parent { get { return m_parent; } }
        }
        public partial class MaterialTexture : KaitaiStruct
        {
            public static MaterialTexture FromFile(string fileName)
            {
                return new MaterialTexture(new KaitaiStream(fileName));
            }

            public MaterialTexture(KaitaiStream p__io, Nud.Material p__parent = null, Nud p__root = null) : base(p__io)
            {
                m_parent = p__parent;
                m_root = p__root;
                _read();
            }
            private void _read()
            {
                _hash = m_io.ReadS4le();
                _unknown = m_io.ReadBytes(6);
                _mapMode = ((Nud.MapMode) m_io.ReadU2le());
                _wrapModeS = ((Nud.WrapMode) m_io.ReadU1());
                _wrapModeT = ((Nud.WrapMode) m_io.ReadU1());
                _minFilter = ((Nud.FilterMode) m_io.ReadU1());
                _magFilter = ((Nud.FilterMode) m_io.ReadU1());
                _mipDetail = ((Nud.MipDetail) m_io.ReadU1());
                _unknown2 = m_io.ReadBytes(7);
            }
            private int _hash;
            private byte[] _unknown;
            private MapMode _mapMode;
            private WrapMode _wrapModeS;
            private WrapMode _wrapModeT;
            private FilterMode _minFilter;
            private FilterMode _magFilter;
            private MipDetail _mipDetail;
            private byte[] _unknown2;
            private Nud m_root;
            private Nud.Material m_parent;
            public int Hash { get { return _hash; } }
            public byte[] Unknown { get { return _unknown; } }
            public MapMode MapMode { get { return _mapMode; } }
            public WrapMode WrapModeS { get { return _wrapModeS; } }
            public WrapMode WrapModeT { get { return _wrapModeT; } }
            public FilterMode MinFilter { get { return _minFilter; } }
            public FilterMode MagFilter { get { return _magFilter; } }
            public MipDetail MipDetail { get { return _mipDetail; } }
            public byte[] Unknown2 { get { return _unknown2; } }
            public Nud M_Root { get { return m_root; } }
            public Nud.Material M_Parent { get { return m_parent; } }
        }
        public partial class MaterialWrapper : KaitaiStruct
        {
            public MaterialWrapper(uint p_position, KaitaiStream p__io, Nud.Part p__parent = null, Nud p__root = null) : base(p__io)
            {
                m_parent = p__parent;
                m_root = p__root;
                _position = p_position;
                f_material = false;
                _read();
            }
            private void _read()
            {
            }
            private bool f_material;
            private Material _material;
            public Material Material
            {
                get
                {
                    if (f_material)
                        return _material;
                    KaitaiStream io = M_Root.M_Io;
                    long _pos = io.Pos;
                    io.Seek(Position);
                    _material = new Material(io, this, m_root);
                    io.Seek(_pos);
                    f_material = true;
                    return _material;
                }
            }
            private uint _position;
            private Nud m_root;
            private Nud.Part m_parent;
            public uint Position { get { return _position; } }
            public Nud M_Root { get { return m_root; } }
            public Nud.Part M_Parent { get { return m_parent; } }
        }
        public partial class MaterialAttribute : KaitaiStruct
        {
            public static MaterialAttribute FromFile(string fileName)
            {
                return new MaterialAttribute(new KaitaiStream(fileName));
            }

            public MaterialAttribute(KaitaiStream p__io, Nud.Material p__parent = null, Nud p__root = null) : base(p__io)
            {
                m_parent = p__parent;
                m_root = p__root;
                f_name = false;
                _read();
            }
            private void _read()
            {
                _size = m_io.ReadU4le();
                _nameOffset = m_io.ReadU4le();
                _numValues = m_io.ReadU4be();
                _padding = m_io.ReadBytes(4);
                _values = new List<float>();
                for (var i = 0; i < NumValues; i++)
                {
                    _values.Add(m_io.ReadF4le());
                }
            }
            private bool f_name;
            private string _name;
            public string Name
            {
                get
                {
                    if (f_name)
                        return _name;
                    if (NumValues != 0) {
                        long _pos = m_io.Pos;
                        m_io.Seek((M_Root.Header.NameStart + NameOffset));
                        _name = System.Text.Encoding.GetEncoding("UTF-8").GetString(m_io.ReadBytesTerm(0, false, true, true));
                        m_io.Seek(_pos);
                        f_name = true;
                    }
                    return _name;
                }
            }
            private uint _size;
            private uint _nameOffset;
            private uint _numValues;
            private byte[] _padding;
            private List<float> _values;
            private Nud m_root;
            private Nud.Material m_parent;
            public uint Size { get { return _size; } }
            public uint NameOffset { get { return _nameOffset; } }
            public uint NumValues { get { return _numValues; } }
            public byte[] Padding { get { return _padding; } }
            public List<float> Values { get { return _values; } }
            public Nud M_Root { get { return m_root; } }
            public Nud.Material M_Parent { get { return m_parent; } }
        }
        public partial class Mesh : KaitaiStruct
        {
            public Mesh(int p_i, KaitaiStream p__io, Nud p__parent = null, Nud p__root = null) : base(p__io)
            {
                m_parent = p__parent;
                m_root = p__root;
                _i = p_i;
                f_name = false;
                f_parts = false;
                _read();
            }
            private void _read()
            {
                _boundingSphere = new List<float>();
                for (var i = 0; i < 8; i++)
                {
                    _boundingSphere.Add(m_io.ReadF4le());
                }
                _nameOffset = m_io.ReadU4le();
                _emptyBytes = m_io.ReadBytes(2);
                if (!((KaitaiStream.ByteArrayCompare(EmptyBytes, new byte[] { 0, 0 }) == 0)))
                {
                    throw new ValidationNotEqualError(new byte[] { 0, 0 }, EmptyBytes, M_Io, "/types/mesh/seq/2");
                }
                _boneFlags = m_io.ReadU2le();
                _boneIndex = m_io.ReadS2le();
                _partCount = m_io.ReadU2le();
                _positionB = m_io.ReadS4le();
            }
            private bool f_name;
            private string _name;
            public string Name
            {
                get
                {
                    if (f_name)
                        return _name;
                    long _pos = m_io.Pos;
                    m_io.Seek((M_Parent.Header.NameStart + NameOffset));
                    _name = System.Text.Encoding.GetEncoding("UTF-8").GetString(m_io.ReadBytesTerm(0, false, true, true));
                    m_io.Seek(_pos);
                    f_name = true;
                    return _name;
                }
            }
            private bool f_parts;
            private List<Part> _parts;
            public List<Part> Parts
            {
                get
                {
                    if (f_parts)
                        return _parts;
                    _parts = (List<Part>) (M_Root.Parts[I].Parts);
                    f_parts = true;
                    return _parts;
                }
            }
            private List<float> _boundingSphere;
            private uint _nameOffset;
            private byte[] _emptyBytes;
            private ushort _boneFlags;
            private short _boneIndex;
            private ushort _partCount;
            private int _positionB;
            private int _i;
            private Nud m_root;
            private Nud m_parent;
            public List<float> BoundingSphere { get { return _boundingSphere; } }
            public uint NameOffset { get { return _nameOffset; } }

            /// <summary>
            /// this is just for alignment
            /// </summary>
            public byte[] EmptyBytes { get { return _emptyBytes; } }
            public ushort BoneFlags { get { return _boneFlags; } }
            public short BoneIndex { get { return _boneIndex; } }
            public ushort PartCount { get { return _partCount; } }
            public int PositionB { get { return _positionB; } }
            public int I { get { return _i; } }
            public Nud M_Root { get { return m_root; } }
            public Nud M_Parent { get { return m_parent; } }
        }
        public partial class NudParts : KaitaiStruct
        {
            public NudParts(ushort p_numParts, KaitaiStream p__io, Nud p__parent = null, Nud p__root = null) : base(p__io)
            {
                m_parent = p__parent;
                m_root = p__root;
                _numParts = p_numParts;
                _read();
            }
            private void _read()
            {
                __raw_parts = new List<byte[]>();
                _parts = new List<Part>();
                for (var i = 0; i < NumParts; i++)
                {
                    __raw_parts.Add(m_io.ReadBytes(48));
                    var io___raw_parts = new KaitaiStream(__raw_parts[__raw_parts.Count - 1]);
                    _parts.Add(new Part(io___raw_parts, this, m_root));
                }
            }
            private List<Part> _parts;
            private ushort _numParts;
            private Nud m_root;
            private Nud m_parent;
            private List<byte[]> __raw_parts;
            public List<Part> Parts { get { return _parts; } }
            public ushort NumParts { get { return _numParts; } }
            public Nud M_Root { get { return m_root; } }
            public Nud M_Parent { get { return m_parent; } }
            public List<byte[]> M_RawParts { get { return __raw_parts; } }
        }
        public partial class Material : KaitaiStruct
        {
            public static Material FromFile(string fileName)
            {
                return new Material(new KaitaiStream(fileName));
            }

            public Material(KaitaiStream p__io, Nud.MaterialWrapper p__parent = null, Nud p__root = null) : base(p__io)
            {
                m_parent = p__parent;
                m_root = p__root;
                _read();
            }
            private void _read()
            {
                _flags = m_io.ReadU1();
                _fog = m_io.ReadU1();
                _texLayers = m_io.ReadU1();
                _materialEffects = m_io.ReadU1();
                _padding = m_io.ReadBytes(4);
                _srcFactor = m_io.ReadU2le();
                _numMaterialTextures = m_io.ReadU2le();
                _dstFactor = m_io.ReadU2le();
                _alphaTest = m_io.ReadBitsIntBe(1) != 0;
                m_io.AlignToByte();
                _alphaFunction = ((Nud.AlphaFunction) m_io.ReadU1());
                _drawPriority = m_io.ReadU2le();
                _cullMode = ((Nud.CullMode) m_io.ReadU2le());
                _padding2 = m_io.ReadBytes(8);
                _zBufferOffset = m_io.ReadS4le();
                _materialTextures = new List<MaterialTexture>();
                for (var i = 0; i < NumMaterialTextures; i++)
                {
                    _materialTextures.Add(new MaterialTexture(m_io, this, m_root));
                }
                _materialAttributes = new List<MaterialAttribute>();
                {
                    var i = 0;
                    MaterialAttribute M_;
                    do {
                        M_ = new MaterialAttribute(m_io, this, m_root);
                        _materialAttributes.Add(M_);
                        i++;
                    } while (!(M_.Size == 0));
                }
            }
            public partial class TextureFlags : KaitaiStruct
            {
                public static TextureFlags FromFile(string fileName)
                {
                    return new TextureFlags(new KaitaiStream(fileName));
                }

                public TextureFlags(KaitaiStream p__io, KaitaiStruct p__parent = null, Nud p__root = null) : base(p__io)
                {
                    m_parent = p__parent;
                    m_root = p__root;
                    _read();
                }
                private void _read()
                {
                    _glow = m_io.ReadBitsIntBe(1) != 0;
                    _shadow = m_io.ReadBitsIntBe(1) != 0;
                    _dummyRamp = m_io.ReadBitsIntBe(1) != 0;
                    _sphereMap = m_io.ReadBitsIntBe(1) != 0;
                    _stageAoMap = m_io.ReadBitsIntBe(1) != 0;
                    _rampCubeMap = m_io.ReadBitsIntBe(1) != 0;
                    _normalMap = m_io.ReadBitsIntBe(1) != 0;
                    _diffuseMap = m_io.ReadBitsIntBe(1) != 0;
                }
                private bool _glow;
                private bool _shadow;
                private bool _dummyRamp;
                private bool _sphereMap;
                private bool _stageAoMap;
                private bool _rampCubeMap;
                private bool _normalMap;
                private bool _diffuseMap;
                private Nud m_root;
                private KaitaiStruct m_parent;
                public bool Glow { get { return _glow; } }
                public bool Shadow { get { return _shadow; } }
                public bool DummyRamp { get { return _dummyRamp; } }
                public bool SphereMap { get { return _sphereMap; } }
                public bool StageAoMap { get { return _stageAoMap; } }
                public bool RampCubeMap { get { return _rampCubeMap; } }
                public bool NormalMap { get { return _normalMap; } }
                public bool DiffuseMap { get { return _diffuseMap; } }
                public Nud M_Root { get { return m_root; } }
                public KaitaiStruct M_Parent { get { return m_parent; } }
            }
            private byte _flags;
            private byte _fog;
            private byte _texLayers;
            private byte _materialEffects;
            private byte[] _padding;
            private ushort _srcFactor;
            private ushort _numMaterialTextures;
            private ushort _dstFactor;
            private bool _alphaTest;
            private AlphaFunction _alphaFunction;
            private ushort _drawPriority;
            private CullMode _cullMode;
            private byte[] _padding2;
            private int _zBufferOffset;
            private List<MaterialTexture> _materialTextures;
            private List<MaterialAttribute> _materialAttributes;
            private Nud m_root;
            private Nud.MaterialWrapper m_parent;
            public byte Flags { get { return _flags; } }
            public byte Fog { get { return _fog; } }
            public byte TexLayers { get { return _texLayers; } }
            public byte MaterialEffects { get { return _materialEffects; } }
            public byte[] Padding { get { return _padding; } }
            public ushort SrcFactor { get { return _srcFactor; } }
            public ushort NumMaterialTextures { get { return _numMaterialTextures; } }
            public ushort DstFactor { get { return _dstFactor; } }
            public bool AlphaTest { get { return _alphaTest; } }
            public AlphaFunction AlphaFunction { get { return _alphaFunction; } }
            public ushort DrawPriority { get { return _drawPriority; } }
            public CullMode CullMode { get { return _cullMode; } }
            public byte[] Padding2 { get { return _padding2; } }
            public int ZBufferOffset { get { return _zBufferOffset; } }
            public List<MaterialTexture> MaterialTextures { get { return _materialTextures; } }
            public List<MaterialAttribute> MaterialAttributes { get { return _materialAttributes; } }
            public Nud M_Root { get { return m_root; } }
            public Nud.MaterialWrapper M_Parent { get { return m_parent; } }
        }
        public partial class Part : KaitaiStruct
        {
            public static Part FromFile(string fileName)
            {
                return new Part(new KaitaiStream(fileName));
            }

            public Part(KaitaiStream p__io, Nud.NudParts p__parent = null, Nud p__root = null) : base(p__io)
            {
                m_parent = p__parent;
                m_root = p__root;
                f_indicesStart = false;
                f_vertAddStart = false;
                f_indices = false;
                f_vertices = false;
                f_vertStart = false;
                f_materials = false;
                _read();
            }
            private void _read()
            {
                _polyOffset = m_io.ReadU4le();
                _vertOffset = m_io.ReadU4le();
                _vertAddOffset = m_io.ReadU4le();
                _numVertices = m_io.ReadU2le();
                _boneSize = ((Nud.BoneSize) m_io.ReadBitsIntBe(4));
                _unusedBit = m_io.ReadBitsIntBe(1) != 0;
                if (!( ((UnusedBit == false)) ))
                {
                    throw new ValidationNotAnyOfError(UnusedBit, M_Io, "/types/part/seq/5");
                }
                _normalHalfFloat = m_io.ReadBitsIntBe(1) != 0;
                _normalType = ((Nud.NormalType) m_io.ReadBitsIntBe(2));
                _uvChannelCount = m_io.ReadBitsIntBe(4);
                _vertexColorSize = ((Nud.VertexColorSize) m_io.ReadBitsIntBe(3));
                _uvFloat = m_io.ReadBitsIntBe(1) != 0;
                m_io.AlignToByte();
                _texprop = new List<uint>();
                for (var i = 0; i < 4; i++)
                {
                    _texprop.Add(m_io.ReadU4le());
                }
                _numIndices = m_io.ReadU2le();
                _polySize = m_io.ReadU1();
                _polyFlag = m_io.ReadU1();
                if (!( ((PolyFlag == 0)) ))
                {
                    throw new ValidationNotAnyOfError(PolyFlag, M_Io, "/types/part/seq/14");
                }
            }
            private bool f_indicesStart;
            private int _indicesStart;
            public int IndicesStart
            {
                get
                {
                    if (f_indicesStart)
                        return _indicesStart;
                    _indicesStart = (int) ((M_Root.Header.IndicesClumpStart + PolyOffset));
                    f_indicesStart = true;
                    return _indicesStart;
                }
            }
            private bool f_vertAddStart;
            private int _vertAddStart;
            public int VertAddStart
            {
                get
                {
                    if (f_vertAddStart)
                        return _vertAddStart;
                    _vertAddStart = (int) ((M_Root.Header.VertAddClumpStart + VertAddOffset));
                    f_vertAddStart = true;
                    return _vertAddStart;
                }
            }
            private bool f_indices;
            private List<ushort> _indices;
            public List<ushort> Indices
            {
                get
                {
                    if (f_indices)
                        return _indices;
                    KaitaiStream io = M_Root.M_Io;
                    long _pos = io.Pos;
                    io.Seek(IndicesStart);
                    _indices = new List<ushort>();
                    for (var i = 0; i < NumIndices; i++)
                    {
                        _indices.Add(io.ReadU2le());
                    }
                    io.Seek(_pos);
                    f_indices = true;
                    return _indices;
                }
            }
            private bool f_vertices;
            private List<Vertex> _vertices;
            public List<Vertex> Vertices
            {
                get
                {
                    if (f_vertices)
                        return _vertices;
                    KaitaiStream io = M_Root.M_Io;
                    long _pos = io.Pos;
                    io.Seek(VertStart);
                    _vertices = new List<Vertex>();
                    for (var i = 0; i < NumVertices; i++)
                    {
                        _vertices.Add(new Vertex(io, this, m_root));
                    }
                    io.Seek(_pos);
                    f_vertices = true;
                    return _vertices;
                }
            }
            private bool f_vertStart;
            private int _vertStart;
            public int VertStart
            {
                get
                {
                    if (f_vertStart)
                        return _vertStart;
                    _vertStart = (int) ((M_Root.Header.VertClumpStart + VertOffset));
                    f_vertStart = true;
                    return _vertStart;
                }
            }
            private bool f_materials;
            private List<MaterialWrapper> _materials;
            public List<MaterialWrapper> Materials
            {
                get
                {
                    if (f_materials)
                        return _materials;
                    _materials = new List<MaterialWrapper>();
                    {
                        var i = 0;
                        MaterialWrapper M_;
                        do {
                            M_ = new MaterialWrapper(Texprop[i], m_io, this, m_root);
                            _materials.Add(M_);
                            i++;
                        } while (!( ((i == 4) || (Texprop[i] == 0)) ));
                    }
                    f_materials = true;
                    return _materials;
                }
            }
            private uint _polyOffset;
            private uint _vertOffset;
            private uint _vertAddOffset;
            private ushort _numVertices;
            private BoneSize _boneSize;
            private bool _unusedBit;
            private bool _normalHalfFloat;
            private NormalType _normalType;
            private ulong _uvChannelCount;
            private VertexColorSize _vertexColorSize;
            private bool _uvFloat;
            private List<uint> _texprop;
            private ushort _numIndices;
            private byte _polySize;
            private byte _polyFlag;
            private Nud m_root;
            private Nud.NudParts m_parent;
            public uint PolyOffset { get { return _polyOffset; } }
            public uint VertOffset { get { return _vertOffset; } }
            public uint VertAddOffset { get { return _vertAddOffset; } }
            public ushort NumVertices { get { return _numVertices; } }
            public BoneSize BoneSize { get { return _boneSize; } }
            public bool UnusedBit { get { return _unusedBit; } }
            public bool NormalHalfFloat { get { return _normalHalfFloat; } }
            public NormalType NormalType { get { return _normalType; } }
            public ulong UvChannelCount { get { return _uvChannelCount; } }
            public VertexColorSize VertexColorSize { get { return _vertexColorSize; } }
            public bool UvFloat { get { return _uvFloat; } }
            public List<uint> Texprop { get { return _texprop; } }
            public ushort NumIndices { get { return _numIndices; } }
            public byte PolySize { get { return _polySize; } }

            /// <summary>
            /// Need to determine what this does
            /// </summary>
            public byte PolyFlag { get { return _polyFlag; } }
            public Nud M_Root { get { return m_root; } }
            public Nud.NudParts M_Parent { get { return m_parent; } }
        }
        public partial class NudHeader : KaitaiStruct
        {
            public static NudHeader FromFile(string fileName)
            {
                return new NudHeader(new KaitaiStream(fileName));
            }

            public NudHeader(KaitaiStream p__io, Nud p__parent = null, Nud p__root = null) : base(p__io)
            {
                m_parent = p__parent;
                m_root = p__root;
                f_partClumpStart = false;
                f_vertClumpStart = false;
                f_nameStart = false;
                f_vertAddClumpStart = false;
                f_indicesClumpStart = false;
                _read();
            }
            private void _read()
            {
                _fileSize = m_io.ReadU4le();
                _version = ((Nud.Version) m_io.ReadU2le());
                if (!( ((Version == Nud.Version.V2)) ))
                {
                    throw new ValidationNotAnyOfError(Version, M_Io, "/types/nud_header/seq/1");
                }
                _polysetCount = m_io.ReadU2le();
                _boneChannels = m_io.ReadU2le();
                _boneCount = m_io.ReadU2le();
                _partClumpSize = m_io.ReadU4le();
                _indicesClumpSize = m_io.ReadU4le();
                _vertClumpSize = m_io.ReadU4le();
                _vertAddClumpSize = m_io.ReadU4le();
                _boundingSphere = new List<float>();
                for (var i = 0; i < 4; i++)
                {
                    _boundingSphere.Add(m_io.ReadF4le());
                }
            }
            private bool f_partClumpStart;
            private sbyte _partClumpStart;
            public sbyte PartClumpStart
            {
                get
                {
                    if (f_partClumpStart)
                        return _partClumpStart;
                    _partClumpStart = (sbyte) (48);
                    f_partClumpStart = true;
                    return _partClumpStart;
                }
            }
            private bool f_vertClumpStart;
            private int _vertClumpStart;
            public int VertClumpStart
            {
                get
                {
                    if (f_vertClumpStart)
                        return _vertClumpStart;
                    _vertClumpStart = (int) ((IndicesClumpStart + IndicesClumpSize));
                    f_vertClumpStart = true;
                    return _vertClumpStart;
                }
            }
            private bool f_nameStart;
            private int _nameStart;
            public int NameStart
            {
                get
                {
                    if (f_nameStart)
                        return _nameStart;
                    _nameStart = (int) ((VertAddClumpStart + VertAddClumpSize));
                    f_nameStart = true;
                    return _nameStart;
                }
            }
            private bool f_vertAddClumpStart;
            private int _vertAddClumpStart;
            public int VertAddClumpStart
            {
                get
                {
                    if (f_vertAddClumpStart)
                        return _vertAddClumpStart;
                    _vertAddClumpStart = (int) ((VertClumpStart + VertClumpSize));
                    f_vertAddClumpStart = true;
                    return _vertAddClumpStart;
                }
            }
            private bool f_indicesClumpStart;
            private int _indicesClumpStart;
            public int IndicesClumpStart
            {
                get
                {
                    if (f_indicesClumpStart)
                        return _indicesClumpStart;
                    _indicesClumpStart = (int) ((PartClumpStart + PartClumpSize));
                    f_indicesClumpStart = true;
                    return _indicesClumpStart;
                }
            }
            private uint _fileSize;
            private Version _version;
            private ushort _polysetCount;
            private ushort _boneChannels;
            private ushort _boneCount;
            private uint _partClumpSize;
            private uint _indicesClumpSize;
            private uint _vertClumpSize;
            private uint _vertAddClumpSize;
            private List<float> _boundingSphere;
            private Nud m_root;
            private Nud m_parent;
            public uint FileSize { get { return _fileSize; } }
            public Version Version { get { return _version; } }
            public ushort PolysetCount { get { return _polysetCount; } }
            public ushort BoneChannels { get { return _boneChannels; } }
            public ushort BoneCount { get { return _boneCount; } }
            public uint PartClumpSize { get { return _partClumpSize; } }
            public uint IndicesClumpSize { get { return _indicesClumpSize; } }
            public uint VertClumpSize { get { return _vertClumpSize; } }
            public uint VertAddClumpSize { get { return _vertAddClumpSize; } }
            public List<float> BoundingSphere { get { return _boundingSphere; } }
            public Nud M_Root { get { return m_root; } }
            public Nud M_Parent { get { return m_parent; } }
        }
        private Signature _magic;
        private NudHeader _header;
        private List<Mesh> _meshes;
        private List<NudParts> _parts;
        private Nud m_root;
        private KaitaiStruct m_parent;
        public Signature Magic { get { return _magic; } }
        public NudHeader Header { get { return _header; } }
        public List<Mesh> Meshes { get { return _meshes; } }
        public List<NudParts> Parts { get { return _parts; } }
        public Nud M_Root { get { return m_root; } }
        public KaitaiStruct M_Parent { get { return m_parent; } }
    }
}
