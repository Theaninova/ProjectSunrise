// This is a generated file! Please edit source .ksy file and use kaitai-struct-compiler to rebuild

using System.Collections.Generic;
using Kaitai;

namespace SunriseMono.Kaitai
{
    public partial class Lmd : KaitaiStruct
    {
        public static Lmd FromFile(string fileName)
        {
            return new Lmd(new KaitaiStream(fileName));
        }


        public enum PositionFlags
        {
            Transform = 0,
            Position = 32768,
            NoTransform = 65535,
        }

        public enum PlacementMode
        {
            Place = 1,
            Move = 2,
        }

        public enum BlendMode
        {
            Normal = 0,
            Layer = 2,
            Multiply = 3,
            Screen = 4,
            Lighten = 5,
            Darken = 6,
            Difference = 7,
            Add = 8,
            Subtract = 9,
            Invert = 10,
            Alpha = 11,
            Erase = 12,
            Overlay = 13,
            HardLight = 14,
        }
        public Lmd(KaitaiStream p__io, KaitaiStruct p__parent = null, Lmd p__root = null) : base(p__io)
        {
            m_parent = p__parent;
            m_root = p__root ?? this;
            f_references = false;
            f_textures = false;
            f_lmb = false;
            _read();
        }
        private void _read()
        {
            _xmd = new Xmd(m_io);
        }
        public partial class RemoveObject : KaitaiStruct
        {
            public static RemoveObject FromFile(string fileName)
            {
                return new RemoveObject(new KaitaiStream(fileName));
            }

            public RemoveObject(KaitaiStream p__io, Lmd.Tag p__parent = null, Lmd p__root = null) : base(p__io)
            {
                m_parent = p__parent;
                m_root = p__root;
                _read();
            }
            private void _read()
            {
                _unknown1 = m_io.ReadU4le();
                _depth = m_io.ReadU2le();
                _unknown2 = m_io.ReadU2le();
            }
            private uint _unknown1;
            private ushort _depth;
            private ushort _unknown2;
            private Lmd m_root;
            private Lmd.Tag m_parent;
            public uint Unknown1 { get { return _unknown1; } }
            public ushort Depth { get { return _depth; } }
            public ushort Unknown2 { get { return _unknown2; } }
            public Lmd M_Root { get { return m_root; } }
            public Lmd.Tag M_Parent { get { return m_parent; } }
        }
        public partial class Fonts : KaitaiStruct
        {
            public static Fonts FromFile(string fileName)
            {
                return new Fonts(new KaitaiStream(fileName));
            }

            public Fonts(KaitaiStream p__io, Lmd.Tag p__parent = null, Lmd p__root = null) : base(p__io)
            {
                m_parent = p__parent;
                m_root = p__root;
                _read();
            }
            private void _read()
            {
                _unknown = m_io.ReadU4le();
            }
            private uint _unknown;
            private Lmd m_root;
            private Lmd.Tag m_parent;
            public uint Unknown { get { return _unknown; } }
            public Lmd M_Root { get { return m_root; } }
            public Lmd.Tag M_Parent { get { return m_parent; } }
        }
        public partial class DynamicText : KaitaiStruct
        {
            public static DynamicText FromFile(string fileName)
            {
                return new DynamicText(new KaitaiStream(fileName));
            }


            public enum TextAlignment
            {
                Left = 0,
                Right = 1,
                Center = 2,
            }
            public DynamicText(KaitaiStream p__io, Lmd.Tag p__parent = null, Lmd p__root = null) : base(p__io)
            {
                m_parent = p__parent;
                m_root = p__root;
                _read();
            }
            private void _read()
            {
                _characterId = m_io.ReadU4le();
                _unknown1 = m_io.ReadU4le();
                _placeholderText = m_io.ReadU4le();
                _unknown2 = m_io.ReadU4le();
                _strokeColorId = m_io.ReadU4le();
                _unknown3 = new List<uint>();
                for (var i = 0; i < 3; i++)
                {
                    _unknown3.Add(m_io.ReadU4le());
                }
                _alignment = ((TextAlignment) m_io.ReadU2le());
                _unknown4 = m_io.ReadU2le();
                _unknown5 = new List<uint>();
                for (var i = 0; i < 2; i++)
                {
                    _unknown5.Add(m_io.ReadU4le());
                }
                _size = m_io.ReadF4le();
                _unknown6 = new List<uint>();
                for (var i = 0; i < 4; i++)
                {
                    _unknown6.Add(m_io.ReadU4le());
                }
            }
            private uint _characterId;
            private uint _unknown1;
            private uint _placeholderText;
            private uint _unknown2;
            private uint _strokeColorId;
            private List<uint> _unknown3;
            private TextAlignment _alignment;
            private ushort _unknown4;
            private List<uint> _unknown5;
            private float _size;
            private List<uint> _unknown6;
            private Lmd m_root;
            private Lmd.Tag m_parent;
            public uint CharacterId { get { return _characterId; } }
            public uint Unknown1 { get { return _unknown1; } }
            public uint PlaceholderText { get { return _placeholderText; } }
            public uint Unknown2 { get { return _unknown2; } }
            public uint StrokeColorId { get { return _strokeColorId; } }
            public List<uint> Unknown3 { get { return _unknown3; } }
            public TextAlignment Alignment { get { return _alignment; } }
            public ushort Unknown4 { get { return _unknown4; } }
            public List<uint> Unknown5 { get { return _unknown5; } }
            public float Size { get { return _size; } }
            public List<uint> Unknown6 { get { return _unknown6; } }
            public Lmd M_Root { get { return m_root; } }
            public Lmd.Tag M_Parent { get { return m_parent; } }
        }
        public partial class Graphic : KaitaiStruct
        {
            public static Graphic FromFile(string fileName)
            {
                return new Graphic(new KaitaiStream(fileName));
            }

            public Graphic(KaitaiStream p__io, Lmd.Tag p__parent = null, Lmd p__root = null) : base(p__io)
            {
                m_parent = p__parent;
                m_root = p__root;
                _read();
            }
            private void _read()
            {
                _atlasId = m_io.ReadU4le();
                _fillType = m_io.ReadU2le();
                _numVertices = m_io.ReadU2le();
                _numIndices = m_io.ReadU4le();
                _vertices = new List<Vertex>();
                for (var i = 0; i < NumVertices; i++)
                {
                    _vertices.Add(new Vertex(m_io, this, m_root));
                }
                _indices = new List<ushort>();
                for (var i = 0; i < NumIndices; i++)
                {
                    _indices.Add(m_io.ReadU2le());
                }
            }
            public partial class Vertex : KaitaiStruct
            {
                public static Vertex FromFile(string fileName)
                {
                    return new Vertex(new KaitaiStream(fileName));
                }

                public Vertex(KaitaiStream p__io, Lmd.Graphic p__parent = null, Lmd p__root = null) : base(p__io)
                {
                    m_parent = p__parent;
                    m_root = p__root;
                    _read();
                }
                private void _read()
                {
                    _x = m_io.ReadF4le();
                    _y = m_io.ReadF4le();
                    _u = m_io.ReadF4le();
                    _v = m_io.ReadF4le();
                }
                private float _x;
                private float _y;
                private float _u;
                private float _v;
                private Lmd m_root;
                private Lmd.Graphic m_parent;
                public float X { get { return _x; } }
                public float Y { get { return _y; } }
                public float U { get { return _u; } }
                public float V { get { return _v; } }
                public Lmd M_Root { get { return m_root; } }
                public Lmd.Graphic M_Parent { get { return m_parent; } }
            }
            private uint _atlasId;
            private ushort _fillType;
            private ushort _numVertices;
            private uint _numIndices;
            private List<Vertex> _vertices;
            private List<ushort> _indices;
            private Lmd m_root;
            private Lmd.Tag m_parent;
            public uint AtlasId { get { return _atlasId; } }
            public ushort FillType { get { return _fillType; } }
            public ushort NumVertices { get { return _numVertices; } }
            public uint NumIndices { get { return _numIndices; } }
            public List<Vertex> Vertices { get { return _vertices; } }
            public List<ushort> Indices { get { return _indices; } }
            public Lmd M_Root { get { return m_root; } }
            public Lmd.Tag M_Parent { get { return m_parent; } }
        }

        /// <summary>
        /// tags are read from top to bottom
        /// BE CAREFUL:
        /// Any tag can depend on any other tag that comes before it.
        /// So while resolving references keep that in mind.
        /// However also by spec, a tag cannot depend on tags that
        /// come after it.
        /// </summary>
        public partial class Tag : KaitaiStruct
        {
            public static Tag FromFile(string fileName)
            {
                return new Tag(new KaitaiStream(fileName));
            }


            public enum FlashTagType
            {
                ShowFrame = 1,
                PlaceObject = 4,
                RemoveObject = 5,
                Fonts = 10,
                DoAction = 12,
                DynamicText = 37,
                DefineSprite = 39,
                FrameLabel = 43,
                Symbols = 61441,
                Colors = 61442,
                Transforms = 61443,
                Bounds = 61444,
                ActionScript = 61445,
                TextureAtlases = 61447,
                UnknownF008 = 61448,
                UnknownF009 = 61449,
                UnknownF00a = 61450,
                UnknownF00b = 61451,
                Properties = 61452,
                Defines = 61453,
                PlaySound = 61460,
                Button = 61474,
                Graphic = 61476,
                ColorMatrix = 61495,
                Positions = 61699,
                Keyframe = 61701,
                End = 65280,
                ActionScript2 = 65285,
            }
            public Tag(KaitaiStream p__io, KaitaiStruct p__parent = null, Lmd p__root = null) : base(p__io)
            {
                m_parent = p__parent;
                m_root = p__root;
                _read();
            }
            private void _read()
            {
                _tagType = ((FlashTagType) m_io.ReadU2le());
                _offset = m_io.ReadU2le();
                if (!( ((Offset == 0)) ))
                {
                    throw new ValidationNotAnyOfError(Offset, M_Io, "/types/tag/seq/1");
                }
                _dataLen = m_io.ReadU4le();
                switch (TagType) {
                case FlashTagType.TextureAtlases: {
                    __raw_data = m_io.ReadBytes((DataLen * 4));
                    var io___raw_data = new KaitaiStream(__raw_data);
                    _data = new TextureAtlases(io___raw_data, this, m_root);
                    break;
                }
                case FlashTagType.DynamicText: {
                    __raw_data = m_io.ReadBytes((DataLen * 4));
                    var io___raw_data = new KaitaiStream(__raw_data);
                    _data = new DynamicText(io___raw_data, this, m_root);
                    break;
                }
                case FlashTagType.Bounds: {
                    __raw_data = m_io.ReadBytes((DataLen * 4));
                    var io___raw_data = new KaitaiStream(__raw_data);
                    _data = new Bounds(io___raw_data, this, m_root);
                    break;
                }
                case FlashTagType.Defines: {
                    __raw_data = m_io.ReadBytes((DataLen * 4));
                    var io___raw_data = new KaitaiStream(__raw_data);
                    _data = new Defines(io___raw_data, this, m_root);
                    break;
                }
                case FlashTagType.ShowFrame: {
                    __raw_data = m_io.ReadBytes((DataLen * 4));
                    var io___raw_data = new KaitaiStream(__raw_data);
                    _data = new Frame(io___raw_data, this, m_root);
                    break;
                }
                case FlashTagType.Keyframe: {
                    __raw_data = m_io.ReadBytes((DataLen * 4));
                    var io___raw_data = new KaitaiStream(__raw_data);
                    _data = new Frame(io___raw_data, this, m_root);
                    break;
                }
                case FlashTagType.Colors: {
                    __raw_data = m_io.ReadBytes((DataLen * 4));
                    var io___raw_data = new KaitaiStream(__raw_data);
                    _data = new Colors(io___raw_data, this, m_root);
                    break;
                }
                case FlashTagType.DefineSprite: {
                    __raw_data = m_io.ReadBytes((DataLen * 4));
                    var io___raw_data = new KaitaiStream(__raw_data);
                    _data = new DefineSprite(io___raw_data, this, m_root);
                    break;
                }
                case FlashTagType.Fonts: {
                    __raw_data = m_io.ReadBytes((DataLen * 4));
                    var io___raw_data = new KaitaiStream(__raw_data);
                    _data = new Fonts(io___raw_data, this, m_root);
                    break;
                }
                case FlashTagType.Properties: {
                    __raw_data = m_io.ReadBytes((DataLen * 4));
                    var io___raw_data = new KaitaiStream(__raw_data);
                    _data = new Properties(io___raw_data, this, m_root);
                    break;
                }
                case FlashTagType.ActionScript: {
                    __raw_data = m_io.ReadBytes((DataLen * 4));
                    var io___raw_data = new KaitaiStream(__raw_data);
                    _data = new Actionscript(io___raw_data, this, m_root);
                    break;
                }
                case FlashTagType.Graphic: {
                    __raw_data = m_io.ReadBytes((DataLen * 4));
                    var io___raw_data = new KaitaiStream(__raw_data);
                    _data = new Graphic(io___raw_data, this, m_root);
                    break;
                }
                case FlashTagType.Symbols: {
                    __raw_data = m_io.ReadBytes((DataLen * 4));
                    var io___raw_data = new KaitaiStream(__raw_data);
                    _data = new Symbols(io___raw_data, this, m_root);
                    break;
                }
                case FlashTagType.PlaceObject: {
                    __raw_data = m_io.ReadBytes((DataLen * 4));
                    var io___raw_data = new KaitaiStream(__raw_data);
                    _data = new PlaceObject(io___raw_data, this, m_root);
                    break;
                }
                case FlashTagType.Button: {
                    __raw_data = m_io.ReadBytes((DataLen * 4));
                    var io___raw_data = new KaitaiStream(__raw_data);
                    _data = new Button(io___raw_data, this, m_root);
                    break;
                }
                case FlashTagType.Positions: {
                    __raw_data = m_io.ReadBytes((DataLen * 4));
                    var io___raw_data = new KaitaiStream(__raw_data);
                    _data = new Positions(io___raw_data, this, m_root);
                    break;
                }
                case FlashTagType.Transforms: {
                    __raw_data = m_io.ReadBytes((DataLen * 4));
                    var io___raw_data = new KaitaiStream(__raw_data);
                    _data = new Transforms(io___raw_data, this, m_root);
                    break;
                }
                case FlashTagType.DoAction: {
                    __raw_data = m_io.ReadBytes((DataLen * 4));
                    var io___raw_data = new KaitaiStream(__raw_data);
                    _data = new DoAction(io___raw_data, this, m_root);
                    break;
                }
                case FlashTagType.FrameLabel: {
                    __raw_data = m_io.ReadBytes((DataLen * 4));
                    var io___raw_data = new KaitaiStream(__raw_data);
                    _data = new FrameLabel(io___raw_data, this, m_root);
                    break;
                }
                case FlashTagType.RemoveObject: {
                    __raw_data = m_io.ReadBytes((DataLen * 4));
                    var io___raw_data = new KaitaiStream(__raw_data);
                    _data = new RemoveObject(io___raw_data, this, m_root);
                    break;
                }
                default: {
                    __raw_data = m_io.ReadBytes((DataLen * 4));
                    var io___raw_data = new KaitaiStream(__raw_data);
                    _data = new Nothing(io___raw_data, this, m_root);
                    break;
                }
                }
                _children = new List<Tag>();
                for (var i = 0; i < (TagType == FlashTagType.Defines ? ((Lmd.Defines) (Data)).NumChildren : (TagType == FlashTagType.Keyframe ? ((Lmd.Frame) (Data)).NumChildren : (TagType == FlashTagType.ShowFrame ? ((Lmd.Frame) (Data)).NumChildren : (TagType == FlashTagType.DefineSprite ? ((Lmd.DefineSprite) (Data)).NumChildren : (TagType == FlashTagType.Button ? ((Lmd.Button) (Data)).NumChildren : (TagType == FlashTagType.PlaceObject ? ((Lmd.PlaceObject) (Data)).NumChildren : 0)))))); i++)
                {
                    _children.Add(new Tag(m_io, this, m_root));
                }
            }
            private FlashTagType _tagType;
            private ushort _offset;
            private uint _dataLen;
            private KaitaiStruct _data;
            private List<Tag> _children;
            private Lmd m_root;
            private KaitaiStruct m_parent;
            private byte[] __raw_data;
            public FlashTagType TagType { get { return _tagType; } }
            public ushort Offset { get { return _offset; } }
            public uint DataLen { get { return _dataLen; } }
            public KaitaiStruct Data { get { return _data; } }
            public List<Tag> Children { get { return _children; } }
            public Lmd M_Root { get { return m_root; } }
            public KaitaiStruct M_Parent { get { return m_parent; } }
            public byte[] M_RawData { get { return __raw_data; } }
        }
        public partial class DoAction : KaitaiStruct
        {
            public static DoAction FromFile(string fileName)
            {
                return new DoAction(new KaitaiStream(fileName));
            }

            public DoAction(KaitaiStream p__io, Lmd.Tag p__parent = null, Lmd p__root = null) : base(p__io)
            {
                m_parent = p__parent;
                m_root = p__root;
                _read();
            }
            private void _read()
            {
                _actionId = m_io.ReadU4le();
                _unknown = m_io.ReadU4le();
            }
            private uint _actionId;
            private uint _unknown;
            private Lmd m_root;
            private Lmd.Tag m_parent;
            public uint ActionId { get { return _actionId; } }
            public uint Unknown { get { return _unknown; } }
            public Lmd M_Root { get { return m_root; } }
            public Lmd.Tag M_Parent { get { return m_parent; } }
        }
        public partial class TexturesType : KaitaiStruct
        {
            public static TexturesType FromFile(string fileName)
            {
                return new TexturesType(new KaitaiStream(fileName));
            }

            public TexturesType(KaitaiStream p__io, Lmd p__parent = null, Lmd p__root = null) : base(p__io)
            {
                m_parent = p__parent;
                m_root = p__root;
                _read();
            }
            private void _read()
            {
                _textureHashes = new List<uint>();
                {
                    var i = 0;
                    while (!m_io.IsEof) {
                        _textureHashes.Add(m_io.ReadU4le());
                        i++;
                    }
                }
            }
            private List<uint> _textureHashes;
            private Lmd m_root;
            private Lmd m_parent;
            public List<uint> TextureHashes { get { return _textureHashes; } }
            public Lmd M_Root { get { return m_root; } }
            public Lmd M_Parent { get { return m_parent; } }
        }
        public partial class FrameLabel : KaitaiStruct
        {
            public static FrameLabel FromFile(string fileName)
            {
                return new FrameLabel(new KaitaiStream(fileName));
            }

            public FrameLabel(KaitaiStream p__io, Lmd.Tag p__parent = null, Lmd p__root = null) : base(p__io)
            {
                m_parent = p__parent;
                m_root = p__root;
                _read();
            }
            private void _read()
            {
                _nameId = m_io.ReadU4le();
                _startFrame = m_io.ReadU4le();
            }
            private uint _nameId;
            private uint _startFrame;
            private Lmd m_root;
            private Lmd.Tag m_parent;
            public uint NameId { get { return _nameId; } }
            public uint StartFrame { get { return _startFrame; } }
            public Lmd M_Root { get { return m_root; } }
            public Lmd.Tag M_Parent { get { return m_parent; } }
        }
        public partial class Bounds : KaitaiStruct
        {
            public static Bounds FromFile(string fileName)
            {
                return new Bounds(new KaitaiStream(fileName));
            }

            public Bounds(KaitaiStream p__io, Lmd.Tag p__parent = null, Lmd p__root = null) : base(p__io)
            {
                m_parent = p__parent;
                m_root = p__root;
                _read();
            }
            private void _read()
            {
                _numValues = m_io.ReadU4le();
                _values = new List<Rect>();
                for (var i = 0; i < NumValues; i++)
                {
                    _values.Add(new Rect(m_io, this, m_root));
                }
            }
            public partial class Rect : KaitaiStruct
            {
                public static Rect FromFile(string fileName)
                {
                    return new Rect(new KaitaiStream(fileName));
                }

                public Rect(KaitaiStream p__io, Lmd.Bounds p__parent = null, Lmd p__root = null) : base(p__io)
                {
                    m_parent = p__parent;
                    m_root = p__root;
                    _read();
                }
                private void _read()
                {
                    _x = m_io.ReadF4le();
                    _y = m_io.ReadF4le();
                    _width = m_io.ReadF4le();
                    _height = m_io.ReadF4le();
                }
                private float _x;
                private float _y;
                private float _width;
                private float _height;
                private Lmd m_root;
                private Lmd.Bounds m_parent;
                public float X { get { return _x; } }
                public float Y { get { return _y; } }
                public float Width { get { return _width; } }
                public float Height { get { return _height; } }
                public Lmd M_Root { get { return m_root; } }
                public Lmd.Bounds M_Parent { get { return m_parent; } }
            }
            private uint _numValues;
            private List<Rect> _values;
            private Lmd m_root;
            private Lmd.Tag m_parent;
            public uint NumValues { get { return _numValues; } }
            public List<Rect> Values { get { return _values; } }
            public Lmd M_Root { get { return m_root; } }
            public Lmd.Tag M_Parent { get { return m_parent; } }
        }
        public partial class Frame : KaitaiStruct
        {
            public static Frame FromFile(string fileName)
            {
                return new Frame(new KaitaiStream(fileName));
            }

            public Frame(KaitaiStream p__io, Lmd.Tag p__parent = null, Lmd p__root = null) : base(p__io)
            {
                m_parent = p__parent;
                m_root = p__root;
                _read();
            }
            private void _read()
            {
                _id = m_io.ReadU4le();
                _numChildren = m_io.ReadU4le();
            }
            private uint _id;
            private uint _numChildren;
            private Lmd m_root;
            private Lmd.Tag m_parent;
            public uint Id { get { return _id; } }

            /// <summary>
            /// children directly follow this tag, they may be place/remove object or do_action
            /// </summary>
            public uint NumChildren { get { return _numChildren; } }
            public Lmd M_Root { get { return m_root; } }
            public Lmd.Tag M_Parent { get { return m_parent; } }
        }
        public partial class Symbols : KaitaiStruct
        {
            public static Symbols FromFile(string fileName)
            {
                return new Symbols(new KaitaiStream(fileName));
            }

            public Symbols(KaitaiStream p__io, Lmd.Tag p__parent = null, Lmd p__root = null) : base(p__io)
            {
                m_parent = p__parent;
                m_root = p__root;
                _read();
            }
            private void _read()
            {
                _numValues = m_io.ReadU4le();
                _values = new List<String>();
                for (var i = 0; i < NumValues; i++)
                {
                    _values.Add(new String(m_io, this, m_root));
                }
            }
            public partial class String : KaitaiStruct
            {
                public static String FromFile(string fileName)
                {
                    return new String(new KaitaiStream(fileName));
                }

                public String(KaitaiStream p__io, Lmd.Symbols p__parent = null, Lmd p__root = null) : base(p__io)
                {
                    m_parent = p__parent;
                    m_root = p__root;
                    _read();
                }
                private void _read()
                {
                    _len = m_io.ReadU4le();
                    _value = System.Text.Encoding.GetEncoding("utf-8").GetString(m_io.ReadBytes(Len));
                    __raw_padding = m_io.ReadBytes((4 - KaitaiStream.Mod(Len, 4)));
                    var io___raw_padding = new KaitaiStream(__raw_padding);
                    _padding = new Nothing(io___raw_padding, this, m_root);
                }
                private uint _len;
                private string _value;
                private Nothing _padding;
                private Lmd m_root;
                private Lmd.Symbols m_parent;
                private byte[] __raw_padding;
                public uint Len { get { return _len; } }
                public string Value { get { return _value; } }
                public Nothing Padding { get { return _padding; } }
                public Lmd M_Root { get { return m_root; } }
                public Lmd.Symbols M_Parent { get { return m_parent; } }
                public byte[] M_RawPadding { get { return __raw_padding; } }
            }
            private uint _numValues;
            private List<String> _values;
            private Lmd m_root;
            private Lmd.Tag m_parent;
            public uint NumValues { get { return _numValues; } }
            public List<String> Values { get { return _values; } }
            public Lmd M_Root { get { return m_root; } }
            public Lmd.Tag M_Parent { get { return m_parent; } }
        }
        public partial class Colors : KaitaiStruct
        {
            public static Colors FromFile(string fileName)
            {
                return new Colors(new KaitaiStream(fileName));
            }

            public Colors(KaitaiStream p__io, Lmd.Tag p__parent = null, Lmd p__root = null) : base(p__io)
            {
                m_parent = p__parent;
                m_root = p__root;
                _read();
            }
            private void _read()
            {
                _numValues = m_io.ReadU4le();
                __raw_values = new List<byte[]>();
                _values = new List<Color>();
                for (var i = 0; i < NumValues; i++)
                {
                    __raw_values.Add(m_io.ReadBytes(8));
                    var io___raw_values = new KaitaiStream(__raw_values[__raw_values.Count - 1]);
                    _values.Add(new Color(io___raw_values, this, m_root));
                }
            }
            public partial class Color : KaitaiStruct
            {
                public static Color FromFile(string fileName)
                {
                    return new Color(new KaitaiStream(fileName));
                }

                public Color(KaitaiStream p__io, Lmd.Colors p__parent = null, Lmd p__root = null) : base(p__io)
                {
                    m_parent = p__parent;
                    m_root = p__root;
                    _read();
                }
                private void _read()
                {
                    _r = m_io.ReadU2le();
                    _g = m_io.ReadU2le();
                    _b = m_io.ReadU2le();
                    _a = m_io.ReadU2le();
                }
                private ushort _r;
                private ushort _g;
                private ushort _b;
                private ushort _a;
                private Lmd m_root;
                private Lmd.Colors m_parent;
                public ushort R { get { return _r; } }
                public ushort G { get { return _g; } }
                public ushort B { get { return _b; } }
                public ushort A { get { return _a; } }
                public Lmd M_Root { get { return m_root; } }
                public Lmd.Colors M_Parent { get { return m_parent; } }
            }
            private uint _numValues;
            private List<Color> _values;
            private Lmd m_root;
            private Lmd.Tag m_parent;
            private List<byte[]> __raw_values;
            public uint NumValues { get { return _numValues; } }
            public List<Color> Values { get { return _values; } }
            public Lmd M_Root { get { return m_root; } }
            public Lmd.Tag M_Parent { get { return m_parent; } }
            public List<byte[]> M_RawValues { get { return __raw_values; } }
        }
        public partial class Button : KaitaiStruct
        {
            public static Button FromFile(string fileName)
            {
                return new Button(new KaitaiStream(fileName));
            }

            public Button(KaitaiStream p__io, Lmd.Tag p__parent = null, Lmd p__root = null) : base(p__io)
            {
                m_parent = p__parent;
                m_root = p__root;
                f_numChildren = false;
                _read();
            }
            private void _read()
            {
                _characterId = m_io.ReadU4le();
                _trackAsMenu = m_io.ReadBitsIntLe(1) != 0;
                _unknown = m_io.ReadBitsIntLe(15);
                m_io.AlignToByte();
                _actionOffset = m_io.ReadU2le();
                _boundsId = m_io.ReadU4le();
                _unknown2 = m_io.ReadU4le();
                _numGraphics = m_io.ReadU4le();
            }
            private bool f_numChildren;
            private uint _numChildren;
            public uint NumChildren
            {
                get
                {
                    if (f_numChildren)
                        return _numChildren;
                    _numChildren = (uint) (NumGraphics);
                    f_numChildren = true;
                    return _numChildren;
                }
            }
            private uint _characterId;
            private bool _trackAsMenu;
            private ulong _unknown;
            private ushort _actionOffset;
            private uint _boundsId;
            private uint _unknown2;
            private uint _numGraphics;
            private Lmd m_root;
            private Lmd.Tag m_parent;
            public uint CharacterId { get { return _characterId; } }
            public bool TrackAsMenu { get { return _trackAsMenu; } }
            public ulong Unknown { get { return _unknown; } }
            public ushort ActionOffset { get { return _actionOffset; } }
            public uint BoundsId { get { return _boundsId; } }
            public uint Unknown2 { get { return _unknown2; } }

            /// <summary>
            /// graphics are the following tags
            /// </summary>
            public uint NumGraphics { get { return _numGraphics; } }
            public Lmd M_Root { get { return m_root; } }
            public Lmd.Tag M_Parent { get { return m_parent; } }
        }
        public partial class Nothing : KaitaiStruct
        {
            public static Nothing FromFile(string fileName)
            {
                return new Nothing(new KaitaiStream(fileName));
            }

            public Nothing(KaitaiStream p__io, KaitaiStruct p__parent = null, Lmd p__root = null) : base(p__io)
            {
                m_parent = p__parent;
                m_root = p__root;
                _read();
            }
            private void _read()
            {
            }
            private Lmd m_root;
            private KaitaiStruct m_parent;
            public Lmd M_Root { get { return m_root; } }
            public KaitaiStruct M_Parent { get { return m_parent; } }
        }
        public partial class Positions : KaitaiStruct
        {
            public static Positions FromFile(string fileName)
            {
                return new Positions(new KaitaiStream(fileName));
            }

            public Positions(KaitaiStream p__io, Lmd.Tag p__parent = null, Lmd p__root = null) : base(p__io)
            {
                m_parent = p__parent;
                m_root = p__root;
                _read();
            }
            private void _read()
            {
                _numValues = m_io.ReadU4le();
                _values = new List<Position>();
                for (var i = 0; i < NumValues; i++)
                {
                    _values.Add(new Position(m_io, this, m_root));
                }
            }
            public partial class Position : KaitaiStruct
            {
                public static Position FromFile(string fileName)
                {
                    return new Position(new KaitaiStream(fileName));
                }

                public Position(KaitaiStream p__io, Lmd.Positions p__parent = null, Lmd p__root = null) : base(p__io)
                {
                    m_parent = p__parent;
                    m_root = p__root;
                    _read();
                }
                private void _read()
                {
                    _x = m_io.ReadF4le();
                    _y = m_io.ReadF4le();
                }
                private float _x;
                private float _y;
                private Lmd m_root;
                private Lmd.Positions m_parent;
                public float X { get { return _x; } }
                public float Y { get { return _y; } }
                public Lmd M_Root { get { return m_root; } }
                public Lmd.Positions M_Parent { get { return m_parent; } }
            }
            private uint _numValues;
            private List<Position> _values;
            private Lmd m_root;
            private Lmd.Tag m_parent;
            public uint NumValues { get { return _numValues; } }
            public List<Position> Values { get { return _values; } }
            public Lmd M_Root { get { return m_root; } }
            public Lmd.Tag M_Parent { get { return m_parent; } }
        }
        public partial class Properties : KaitaiStruct
        {
            public static Properties FromFile(string fileName)
            {
                return new Properties(new KaitaiStream(fileName));
            }

            public Properties(KaitaiStream p__io, Lmd.Tag p__parent = null, Lmd p__root = null) : base(p__io)
            {
                m_parent = p__parent;
                m_root = p__root;
                _read();
            }
            private void _read()
            {
                _unknown = m_io.ReadBytes((3 * 4));
                _maxCharacterId = m_io.ReadU4le();
                _unknown2 = m_io.ReadU4le();
                _entryCharacterId = m_io.ReadU4le();
                _maxDepth = m_io.ReadU2le();
                _unknown3 = m_io.ReadU2le();
                _framerate = m_io.ReadF4le();
                _width = m_io.ReadF4le();
                _height = m_io.ReadF4le();
                _unknown4 = m_io.ReadBytes((2 * 4));
            }
            private byte[] _unknown;
            private uint _maxCharacterId;
            private uint _unknown2;
            private uint _entryCharacterId;
            private ushort _maxDepth;
            private ushort _unknown3;
            private float _framerate;
            private float _width;
            private float _height;
            private byte[] _unknown4;
            private Lmd m_root;
            private Lmd.Tag m_parent;
            public byte[] Unknown { get { return _unknown; } }
            public uint MaxCharacterId { get { return _maxCharacterId; } }
            public uint Unknown2 { get { return _unknown2; } }
            public uint EntryCharacterId { get { return _entryCharacterId; } }
            public ushort MaxDepth { get { return _maxDepth; } }
            public ushort Unknown3 { get { return _unknown3; } }
            public float Framerate { get { return _framerate; } }
            public float Width { get { return _width; } }
            public float Height { get { return _height; } }
            public byte[] Unknown4 { get { return _unknown4; } }
            public Lmd M_Root { get { return m_root; } }
            public Lmd.Tag M_Parent { get { return m_parent; } }
        }
        public partial class TextureAtlases : KaitaiStruct
        {
            public static TextureAtlases FromFile(string fileName)
            {
                return new TextureAtlases(new KaitaiStream(fileName));
            }

            public TextureAtlases(KaitaiStream p__io, Lmd.Tag p__parent = null, Lmd p__root = null) : base(p__io)
            {
                m_parent = p__parent;
                m_root = p__root;
                _read();
            }
            private void _read()
            {
                _numValues = m_io.ReadU4le();
                _values = new List<TextureAtlas>();
                for (var i = 0; i < NumValues; i++)
                {
                    _values.Add(new TextureAtlas(m_io, this, m_root));
                }
            }
            public partial class TextureAtlas : KaitaiStruct
            {
                public static TextureAtlas FromFile(string fileName)
                {
                    return new TextureAtlas(new KaitaiStream(fileName));
                }

                public TextureAtlas(KaitaiStream p__io, Lmd.TextureAtlases p__parent = null, Lmd p__root = null) : base(p__io)
                {
                    m_parent = p__parent;
                    m_root = p__root;
                    _read();
                }
                private void _read()
                {
                    _id = m_io.ReadU4le();
                    _nameId = m_io.ReadU4le();
                    _width = m_io.ReadF4le();
                    _height = m_io.ReadF4le();
                }
                private uint _id;
                private uint _nameId;
                private float _width;
                private float _height;
                private Lmd m_root;
                private Lmd.TextureAtlases m_parent;
                public uint Id { get { return _id; } }
                public uint NameId { get { return _nameId; } }
                public float Width { get { return _width; } }
                public float Height { get { return _height; } }
                public Lmd M_Root { get { return m_root; } }
                public Lmd.TextureAtlases M_Parent { get { return m_parent; } }
            }
            private uint _numValues;
            private List<TextureAtlas> _values;
            private Lmd m_root;
            private Lmd.Tag m_parent;
            public uint NumValues { get { return _numValues; } }
            public List<TextureAtlas> Values { get { return _values; } }
            public Lmd M_Root { get { return m_root; } }
            public Lmd.Tag M_Parent { get { return m_parent; } }
        }
        public partial class DefineSprite : KaitaiStruct
        {
            public static DefineSprite FromFile(string fileName)
            {
                return new DefineSprite(new KaitaiStream(fileName));
            }

            public DefineSprite(KaitaiStream p__io, Lmd.Tag p__parent = null, Lmd p__root = null) : base(p__io)
            {
                m_parent = p__parent;
                m_root = p__root;
                f_numChildren = false;
                _read();
            }
            private void _read()
            {
                _characterId = m_io.ReadU4le();
                _nameId = m_io.ReadU4le();
                _boundsId = m_io.ReadU4le();
                _numFrameLabels = m_io.ReadU4le();
                _numFrames = m_io.ReadU4le();
                _numKeyframes = m_io.ReadU4le();
                _numPlacedObjects = m_io.ReadU4le();
            }
            private bool f_numChildren;
            private int _numChildren;
            public int NumChildren
            {
                get
                {
                    if (f_numChildren)
                        return _numChildren;
                    _numChildren = (int) (((NumFrameLabels + NumFrames) + NumKeyframes));
                    f_numChildren = true;
                    return _numChildren;
                }
            }
            private uint _characterId;
            private uint _nameId;
            private uint _boundsId;
            private uint _numFrameLabels;
            private uint _numFrames;
            private uint _numKeyframes;
            private uint _numPlacedObjects;
            private Lmd m_root;
            private Lmd.Tag m_parent;
            public uint CharacterId { get { return _characterId; } }

            /// <summary>
            /// don't know if this is correct
            /// </summary>
            public uint NameId { get { return _nameId; } }

            /// <summary>
            /// don't know if this is correct
            /// </summary>
            public uint BoundsId { get { return _boundsId; } }

            /// <summary>
            /// labels follow this tag, their respective index is the keyframe id
            /// </summary>
            public uint NumFrameLabels { get { return _numFrameLabels; } }

            /// <summary>
            /// frames and keyframes may be mixed and come directly after this tag
            /// </summary>
            public uint NumFrames { get { return _numFrames; } }
            public uint NumKeyframes { get { return _numKeyframes; } }
            public uint NumPlacedObjects { get { return _numPlacedObjects; } }
            public Lmd M_Root { get { return m_root; } }
            public Lmd.Tag M_Parent { get { return m_parent; } }
        }
        public partial class Transforms : KaitaiStruct
        {
            public static Transforms FromFile(string fileName)
            {
                return new Transforms(new KaitaiStream(fileName));
            }

            public Transforms(KaitaiStream p__io, Lmd.Tag p__parent = null, Lmd p__root = null) : base(p__io)
            {
                m_parent = p__parent;
                m_root = p__root;
                _read();
            }
            private void _read()
            {
                _numValues = m_io.ReadU4le();
                _values = new List<Matrix>();
                for (var i = 0; i < NumValues; i++)
                {
                    _values.Add(new Matrix(m_io, this, m_root));
                }
            }
            public partial class Matrix : KaitaiStruct
            {
                public static Matrix FromFile(string fileName)
                {
                    return new Matrix(new KaitaiStream(fileName));
                }

                public Matrix(KaitaiStream p__io, Lmd.Transforms p__parent = null, Lmd p__root = null) : base(p__io)
                {
                    m_parent = p__parent;
                    m_root = p__root;
                    _read();
                }
                private void _read()
                {
                    _a = m_io.ReadF4le();
                    _b = m_io.ReadF4le();
                    _c = m_io.ReadF4le();
                    _d = m_io.ReadF4le();
                    _x = m_io.ReadF4le();
                    _y = m_io.ReadF4le();
                }
                private float _a;
                private float _b;
                private float _c;
                private float _d;
                private float _x;
                private float _y;
                private Lmd m_root;
                private Lmd.Transforms m_parent;
                public float A { get { return _a; } }
                public float B { get { return _b; } }
                public float C { get { return _c; } }
                public float D { get { return _d; } }
                public float X { get { return _x; } }
                public float Y { get { return _y; } }
                public Lmd M_Root { get { return m_root; } }
                public Lmd.Transforms M_Parent { get { return m_parent; } }
            }
            private uint _numValues;
            private List<Matrix> _values;
            private Lmd m_root;
            private Lmd.Tag m_parent;
            public uint NumValues { get { return _numValues; } }
            public List<Matrix> Values { get { return _values; } }
            public Lmd M_Root { get { return m_root; } }
            public Lmd.Tag M_Parent { get { return m_parent; } }
        }
        public partial class Actionscript : KaitaiStruct
        {
            public static Actionscript FromFile(string fileName)
            {
                return new Actionscript(new KaitaiStream(fileName));
            }

            public Actionscript(KaitaiStream p__io, Lmd.Tag p__parent = null, Lmd p__root = null) : base(p__io)
            {
                m_parent = p__parent;
                m_root = p__root;
                _read();
            }
            private void _read()
            {
                _bytecode = new List<byte>();
                {
                    var i = 0;
                    while (!m_io.IsEof) {
                        _bytecode.Add(m_io.ReadU1());
                        i++;
                    }
                }
            }
            private List<byte> _bytecode;
            private Lmd m_root;
            private Lmd.Tag m_parent;
            public List<byte> Bytecode { get { return _bytecode; } }
            public Lmd M_Root { get { return m_root; } }
            public Lmd.Tag M_Parent { get { return m_parent; } }
        }
        public partial class LmbType : KaitaiStruct
        {
            public static LmbType FromFile(string fileName)
            {
                return new LmbType(new KaitaiStream(fileName));
            }

            public LmbType(KaitaiStream p__io, Lmd p__parent = null, Lmd p__root = null) : base(p__io)
            {
                m_parent = p__parent;
                m_root = p__root;
                _read();
            }
            private void _read()
            {
                _magic = m_io.ReadBytes(4);
                if (!((KaitaiStream.ByteArrayCompare(Magic, new byte[] { 76, 77, 66, 0 }) == 0)))
                {
                    throw new ValidationNotEqualError(new byte[] { 76, 77, 66, 0 }, Magic, M_Io, "/types/lmb_type/seq/0");
                }
                _textureId = m_io.ReadU4le();
                _resourceId = m_io.ReadU4le();
                __raw_xmdPadding = m_io.ReadBytes(4);
                var io___raw_xmdPadding = new KaitaiStream(__raw_xmdPadding);
                _xmdPadding = new Nothing(io___raw_xmdPadding, this, m_root);
                _numPadding = m_io.ReadU4le();
                _unknown4 = m_io.ReadU4le();
                _unknown5 = m_io.ReadU4le();
                _totalFileLen = m_io.ReadU4le();
                _padding = new List<byte[]>();
                for (var i = 0; i < NumPadding; i++)
                {
                    _padding.Add(m_io.ReadBytes(16));
                }
                _tags = new List<Tag>();
                {
                    var i = 0;
                    while (!m_io.IsEof) {
                        _tags.Add(new Tag(m_io, this, m_root));
                        i++;
                    }
                }
            }
            private byte[] _magic;
            private uint _textureId;
            private uint _resourceId;
            private Nothing _xmdPadding;
            private uint _numPadding;
            private uint _unknown4;
            private uint _unknown5;
            private uint _totalFileLen;
            private List<byte[]> _padding;
            private List<Tag> _tags;
            private Lmd m_root;
            private Lmd m_parent;
            private byte[] __raw_xmdPadding;
            public byte[] Magic { get { return _magic; } }
            public uint TextureId { get { return _textureId; } }
            public uint ResourceId { get { return _resourceId; } }
            public Nothing XmdPadding { get { return _xmdPadding; } }
            public uint NumPadding { get { return _numPadding; } }
            public uint Unknown4 { get { return _unknown4; } }
            public uint Unknown5 { get { return _unknown5; } }
            public uint TotalFileLen { get { return _totalFileLen; } }
            public List<byte[]> Padding { get { return _padding; } }
            public List<Tag> Tags { get { return _tags; } }
            public Lmd M_Root { get { return m_root; } }
            public Lmd M_Parent { get { return m_parent; } }
            public byte[] M_RawXmdPadding { get { return __raw_xmdPadding; } }
        }
        public partial class PlaceObject : KaitaiStruct
        {
            public static PlaceObject FromFile(string fileName)
            {
                return new PlaceObject(new KaitaiStream(fileName));
            }

            public PlaceObject(KaitaiStream p__io, Lmd.Tag p__parent = null, Lmd p__root = null) : base(p__io)
            {
                m_parent = p__parent;
                m_root = p__root;
                f_numChildren = false;
                _read();
            }
            private void _read()
            {
                _characterId = m_io.ReadS4le();
                _placementId = m_io.ReadS4le();
                _unknown1 = m_io.ReadU4le();
                _nameId = m_io.ReadU4le();
                _placementMode = ((Lmd.PlacementMode) m_io.ReadU2le());
                _blendMode = ((Lmd.BlendMode) m_io.ReadU2le());
                _depth = m_io.ReadU2le();
                _unknown2 = m_io.ReadU2le();
                _unknown3 = m_io.ReadU2le();
                _unknown4 = m_io.ReadU2le();
                _positionId = m_io.ReadS2le();
                _positionFlags = ((Lmd.PositionFlags) m_io.ReadU2le());
                _colorMultId = m_io.ReadS4le();
                _colorAddId = m_io.ReadS4le();
                _hasColorMatrix = m_io.ReadU4le();
                _hasUnknownF014 = m_io.ReadU4le();
            }
            private bool f_numChildren;
            private int _numChildren;
            public int NumChildren
            {
                get
                {
                    if (f_numChildren)
                        return _numChildren;
                    _numChildren = (int) ((HasColorMatrix + HasUnknownF014));
                    f_numChildren = true;
                    return _numChildren;
                }
            }
            private int _characterId;
            private int _placementId;
            private uint _unknown1;
            private uint _nameId;
            private PlacementMode _placementMode;
            private BlendMode _blendMode;
            private ushort _depth;
            private ushort _unknown2;
            private ushort _unknown3;
            private ushort _unknown4;
            private short _positionId;
            private PositionFlags _positionFlags;
            private int _colorMultId;
            private int _colorAddId;
            private uint _hasColorMatrix;
            private uint _hasUnknownF014;
            private Lmd m_root;
            private Lmd.Tag m_parent;
            public int CharacterId { get { return _characterId; } }
            public int PlacementId { get { return _placementId; } }
            public uint Unknown1 { get { return _unknown1; } }
            public uint NameId { get { return _nameId; } }
            public PlacementMode PlacementMode { get { return _placementMode; } }
            public BlendMode BlendMode { get { return _blendMode; } }
            public ushort Depth { get { return _depth; } }
            public ushort Unknown2 { get { return _unknown2; } }
            public ushort Unknown3 { get { return _unknown3; } }
            public ushort Unknown4 { get { return _unknown4; } }

            /// <summary>
            /// This is conditionally a position id, transform id, or nothing (-1) depending on position_flags
            /// </summary>
            public short PositionId { get { return _positionId; } }
            public PositionFlags PositionFlags { get { return _positionFlags; } }
            public int ColorMultId { get { return _colorMultId; } }
            public int ColorAddId { get { return _colorAddId; } }
            public uint HasColorMatrix { get { return _hasColorMatrix; } }
            public uint HasUnknownF014 { get { return _hasUnknownF014; } }
            public Lmd M_Root { get { return m_root; } }
            public Lmd.Tag M_Parent { get { return m_parent; } }
        }
        public partial class Defines : KaitaiStruct
        {
            public static Defines FromFile(string fileName)
            {
                return new Defines(new KaitaiStream(fileName));
            }

            public Defines(KaitaiStream p__io, Lmd.Tag p__parent = null, Lmd p__root = null) : base(p__io)
            {
                m_parent = p__parent;
                m_root = p__root;
                f_numChildren = false;
                _read();
            }
            private void _read()
            {
                _numShapes = m_io.ReadU4le();
                _unknown1 = m_io.ReadU4le();
                _numSprites = m_io.ReadU4le();
                _unknown2 = m_io.ReadU4le();
                _numTexts = m_io.ReadU4le();
                _unknown3 = new List<uint>();
                for (var i = 0; i < 3; i++)
                {
                    _unknown3.Add(m_io.ReadU4le());
                }
            }
            private bool f_numChildren;
            private int _numChildren;
            public int NumChildren
            {
                get
                {
                    if (f_numChildren)
                        return _numChildren;
                    _numChildren = (int) (((NumSprites + NumTexts) + NumShapes));
                    f_numChildren = true;
                    return _numChildren;
                }
            }
            private uint _numShapes;
            private uint _unknown1;
            private uint _numSprites;
            private uint _unknown2;
            private uint _numTexts;
            private List<uint> _unknown3;
            private Lmd m_root;
            private Lmd.Tag m_parent;
            public uint NumShapes { get { return _numShapes; } }
            public uint Unknown1 { get { return _unknown1; } }
            public uint NumSprites { get { return _numSprites; } }
            public uint Unknown2 { get { return _unknown2; } }
            public uint NumTexts { get { return _numTexts; } }
            public List<uint> Unknown3 { get { return _unknown3; } }
            public Lmd M_Root { get { return m_root; } }
            public Lmd.Tag M_Parent { get { return m_parent; } }
        }
        private bool f_references;
        private byte[] _references;
        public byte[] References
        {
            get
            {
                if (f_references)
                    return _references;
                long _pos = m_io.Pos;
                m_io.Seek(Xmd.Positions[2]);
                _references = m_io.ReadBytes(Xmd.Lengths[2]);
                m_io.Seek(_pos);
                f_references = true;
                return _references;
            }
        }
        private bool f_textures;
        private TexturesType _textures;
        public TexturesType Textures
        {
            get
            {
                if (f_textures)
                    return _textures;
                long _pos = m_io.Pos;
                m_io.Seek(Xmd.Positions[1]);
                __raw_textures = m_io.ReadBytes(Xmd.Lengths[1]);
                var io___raw_textures = new KaitaiStream(__raw_textures);
                _textures = new TexturesType(io___raw_textures, this, m_root);
                m_io.Seek(_pos);
                f_textures = true;
                return _textures;
            }
        }
        private bool f_lmb;
        private LmbType _lmb;
        public LmbType Lmb
        {
            get
            {
                if (f_lmb)
                    return _lmb;
                long _pos = m_io.Pos;
                m_io.Seek(Xmd.Positions[0]);
                __raw_lmb = m_io.ReadBytes(Xmd.Lengths[0]);
                var io___raw_lmb = new KaitaiStream(__raw_lmb);
                _lmb = new LmbType(io___raw_lmb, this, m_root);
                m_io.Seek(_pos);
                f_lmb = true;
                return _lmb;
            }
        }
        private Xmd _xmd;
        private Lmd m_root;
        private KaitaiStruct m_parent;
        private byte[] __raw_textures;
        private byte[] __raw_lmb;
        public Xmd Xmd { get { return _xmd; } }
        public Lmd M_Root { get { return m_root; } }
        public KaitaiStruct M_Parent { get { return m_parent; } }
        public byte[] M_RawTextures { get { return __raw_textures; } }
        public byte[] M_RawLmb { get { return __raw_lmb; } }
    }
}
