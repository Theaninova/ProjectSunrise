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

public partial class Avm1 : KaitaiStruct
{
    public enum Opcode
    {
        End = 0,
        NextFrame = 4,
        PreviousFrame = 5,
        Play = 6,
        Stop = 7,
        ToggleQuality = 8,
        StopSounds = 9,
        Add = 10,
        Subtract = 11,
        Multiply = 12,
        Divide = 13,
        Equals = 14,
        Less = 15,
        And = 16,
        Or = 17,
        Not = 18,
        StringEquals = 19,
        StringLength = 20,
        StringExtract = 21,
        Pop = 23,
        ToInteger = 24,
        GetVariable = 28,
        SetVariable = 29,
        SetTarget2 = 32,
        StringAdd = 33,
        GetProperty = 34,
        SetProperty = 35,
        CloneSprite = 36,
        RemoveSprite = 37,
        Trace = 38,
        StartDrag = 39,
        EndDrag = 40,
        StringLess = 41,
        Throw = 42,
        CastOp = 43,
        ImplementsOp = 44,
        RandomNumber = 48,
        MbStringLength = 49,
        CharToAscii = 50,
        AsciiToChar = 51,
        GetTime = 52,
        MbStringExtract = 53,
        MbCharToAscii = 54,
        MbAsciiToChar = 55,
        Delete = 58,
        Delete2 = 59,
        DefineLocal = 60,
        CallFunction = 61,
        Return = 62,
        Modulo = 63,
        NewObject = 64,
        DefineLocal2 = 65,
        InitArray = 66,
        InitObject = 67,
        TypeOf = 68,
        TargetPath = 69,
        Enumerate = 70,
        Add2 = 71,
        Less2 = 72,
        Equals2 = 73,
        ToNumber = 74,
        ToString = 75,
        PushDuplicate = 76,
        StackSwap = 77,
        GetMember = 78,
        SetMember = 79,
        Increment = 80,
        Decrement = 81,
        CallMethod = 82,
        NewMethod = 83,
        InstanceOf = 84,
        Enumerate2 = 85,
        BitAnd = 96,
        BitOr = 97,
        BitXor = 98,
        BitLShift = 99,
        BitRShift = 100,
        BitUrShift = 101,
        StrictEquals = 102,
        Greater = 103,
        StringGreater = 104,
        Extends = 105,
        GotoFrame = 129,
        GetUrl = 131,
        StoreRegister = 135,
        ConstantPool = 136,
        WaitForFrame = 138,
        SetTarget = 139,
        GotoLabel = 140,
        WaitForFrame2 = 141,
        DefineFunction2 = 142,
        Try = 143,
        With = 148,
        Push = 150,
        Jump = 153,
        GetUrl2 = 154,
        DefineFunction = 155,
        JumpEqual = 157,
        Call = 158,
        GotoFrame2 = 159
    }

    public enum ValueType
    {
        String = 0,
        Float = 1,
        Null = 2,
        Undefined = 3,
        Register = 4,
        Bool = 5,
        Double = 6,
        Int = 7,
        ConstantPool8 = 8,
        ConstantPool16 = 9
    }

    public Avm1(KaitaiStream p__io, KaitaiStruct p__parent = null, Avm1 p__root = null)
        : base(p__io)
    {
        M_Parent = p__parent;
        M_Root = p__root ?? this;
        _read();
    }

    public uint NumActions { get; private set; }

    public List<Action> Actions { get; private set; }

    public Avm1 M_Root { get; }

    public KaitaiStruct M_Parent { get; }

    public static Avm1 FromFile(string fileName)
    {
        return new Avm1(new KaitaiStream(fileName));
    }

    private void _read()
    {
        NumActions = m_io.ReadU4le();
        Actions = new List<Action>();
        for (var i = 0; i < 5; i++)
            Actions.Add(new Action(m_io, this, M_Root));
    }

    public class ActionBody : KaitaiStruct
    {
        public ActionBody(KaitaiStream p__io, Action p__parent = null, Avm1 p__root = null)
            : base(p__io)
        {
            M_Parent = p__parent;
            M_Root = p__root;
            _read();
        }

        public List<Op> Ops { get; private set; }

        public Avm1 M_Root { get; }

        public Action M_Parent { get; }

        public static ActionBody FromFile(string fileName)
        {
            return new ActionBody(new KaitaiStream(fileName));
        }

        private void _read()
        {
            Ops = new List<Op>();
            {
                var i = 0;
                while (!m_io.IsEof)
                {
                    Ops.Add(new Op(m_io, this, M_Root));
                    i++;
                }
            }
        }
    }

    public class JumpData : KaitaiStruct
    {
        private int _jumpTargetOffset;
        private bool f_jumpTargetOffset;

        public JumpData(KaitaiStream p__io, Op p__parent = null, Avm1 p__root = null)
            : base(p__io)
        {
            M_Parent = p__parent;
            M_Root = p__root;
            f_jumpTargetOffset = false;
            _read();
        }

        public int JumpTargetOffset
        {
            get
            {
                if (f_jumpTargetOffset)
                    return _jumpTargetOffset;
                _jumpTargetOffset = (int)(M_Parent.OffsetNext.Value + Offset);
                f_jumpTargetOffset = true;
                return _jumpTargetOffset;
            }
        }

        public short Offset { get; private set; }

        public Avm1 M_Root { get; }

        public Op M_Parent { get; }

        public static JumpData FromFile(string fileName)
        {
            return new JumpData(new KaitaiStream(fileName));
        }

        private void _read()
        {
            Offset = m_io.ReadS2le();
        }
    }

    public class Nothing : KaitaiStruct
    {
        public Nothing(KaitaiStream p__io, KaitaiStruct p__parent = null, Avm1 p__root = null)
            : base(p__io)
        {
            M_Parent = p__parent;
            M_Root = p__root;
            _read();
        }

        public Avm1 M_Root { get; }

        public KaitaiStruct M_Parent { get; }

        public static Nothing FromFile(string fileName)
        {
            return new Nothing(new KaitaiStream(fileName));
        }

        private void _read() { }
    }

    public class PushValue : KaitaiStruct
    {
        public PushValue(KaitaiStream p__io, PushData p__parent = null, Avm1 p__root = null)
            : base(p__io)
        {
            M_Parent = p__parent;
            M_Root = p__root;
            _read();
        }

        public ValueType Type { get; private set; }

        public object Value { get; private set; }

        public Avm1 M_Root { get; }

        public PushData M_Parent { get; }

        public static PushValue FromFile(string fileName)
        {
            return new PushValue(new KaitaiStream(fileName));
        }

        private void _read()
        {
            Type = (ValueType)m_io.ReadU1();
            switch (Type)
            {
                case ValueType.Float:
                {
                    Value = m_io.ReadF4le();
                    break;
                }
                case ValueType.ConstantPool8:
                {
                    Value = m_io.ReadU1();
                    break;
                }
                case ValueType.Double:
                {
                    Value = m_io.ReadF8le();
                    break;
                }
                case ValueType.ConstantPool16:
                {
                    Value = m_io.ReadU2le();
                    break;
                }
                case ValueType.Register:
                {
                    Value = m_io.ReadU2le();
                    break;
                }
                case ValueType.Bool:
                {
                    Value = m_io.ReadU1();
                    break;
                }
                case ValueType.Int:
                {
                    Value = m_io.ReadS4le();
                    break;
                }
                case ValueType.String:
                {
                    Value = m_io.ReadU2le();
                    break;
                }
                default:
                {
                    Value = new Nothing(m_io, this, M_Root);
                    break;
                }
            }
        }
    }

    public class Instant : KaitaiStruct
    {
        public Instant(long p_value, KaitaiStream p__io, Op p__parent = null, Avm1 p__root = null)
            : base(p__io)
        {
            M_Parent = p__parent;
            M_Root = p__root;
            Value = p_value;
            _read();
        }

        public long Value { get; }

        public Avm1 M_Root { get; }

        public Op M_Parent { get; }

        private void _read() { }
    }

    public class PushData : KaitaiStruct
    {
        public PushData(KaitaiStream p__io, Op p__parent = null, Avm1 p__root = null)
            : base(p__io)
        {
            M_Parent = p__parent;
            M_Root = p__root;
            _read();
        }

        public List<PushValue> Values { get; private set; }

        public Avm1 M_Root { get; }

        public Op M_Parent { get; }

        public static PushData FromFile(string fileName)
        {
            return new PushData(new KaitaiStream(fileName));
        }

        private void _read()
        {
            Values = new List<PushValue>();
            {
                var i = 0;
                while (!m_io.IsEof)
                {
                    Values.Add(new PushValue(m_io, this, M_Root));
                    i++;
                }
            }
        }
    }

    public class Action : KaitaiStruct
    {
        public Action(KaitaiStream p__io, Avm1 p__parent = null, Avm1 p__root = null)
            : base(p__io)
        {
            M_Parent = p__parent;
            M_Root = p__root;
            _read();
        }

        public ushort Size { get; private set; }

        public Nothing Padding { get; private set; }

        public ActionBody Body { get; private set; }

        public Nothing Padding2 { get; private set; }

        public Nothing Align { get; private set; }

        public Avm1 M_Root { get; }

        public Avm1 M_Parent { get; }

        public byte[] M_RawPadding { get; private set; }

        public byte[] M_RawBody { get; private set; }

        public byte[] M_RawPadding2 { get; private set; }

        public byte[] M_RawAlign { get; private set; }

        public static Action FromFile(string fileName)
        {
            return new Action(new KaitaiStream(fileName));
        }

        private void _read()
        {
            Size = m_io.ReadU2le();
            M_RawPadding = m_io.ReadBytes(2);
            var io___raw_padding = new KaitaiStream(M_RawPadding);
            Padding = new Nothing(io___raw_padding, this, M_Root);
            M_RawBody = m_io.ReadBytes(Size - 2);
            var io___raw_body = new KaitaiStream(M_RawBody);
            Body = new ActionBody(io___raw_body, this, M_Root);
            M_RawPadding2 = m_io.ReadBytes(2);
            var io___raw_padding2 = new KaitaiStream(M_RawPadding2);
            Padding2 = new Nothing(io___raw_padding2, this, M_Root);
            if (KaitaiStream.Mod(Size + 4, 4) != 0)
            {
                M_RawAlign = m_io.ReadBytes(4 - KaitaiStream.Mod(Size + 4, 4));
                var io___raw_align = new KaitaiStream(M_RawAlign);
                Align = new Nothing(io___raw_align, this, M_Root);
            }
        }
    }

    public class Op : KaitaiStruct
    {
        public Op(KaitaiStream p__io, ActionBody p__parent = null, Avm1 p__root = null)
            : base(p__io)
        {
            M_Parent = p__parent;
            M_Root = p__root;
            _read();
        }

        public Instant Offset { get; private set; }

        public Opcode Opcode { get; private set; }

        public ushort? ActionLen { get; private set; }

        public KaitaiStruct Action { get; private set; }

        public Instant OffsetNext { get; private set; }

        public Avm1 M_Root { get; }

        public ActionBody M_Parent { get; }

        public byte[] M_RawAction { get; private set; }

        public static Op FromFile(string fileName)
        {
            return new Op(new KaitaiStream(fileName));
        }

        private void _read()
        {
            Offset = new Instant(M_Io.Pos, m_io, this, M_Root);
            Opcode = (Opcode)m_io.ReadU1();
            if ((byte)Opcode >= 128)
                ActionLen = m_io.ReadU2le();
            if ((byte)Opcode >= 128)
                switch (Opcode)
                {
                    case Opcode.Jump:
                    {
                        M_RawAction = m_io.ReadBytes((ushort)ActionLen);
                        var io___raw_action = new KaitaiStream(M_RawAction);
                        Action = new JumpData(io___raw_action, this, M_Root);
                        break;
                    }
                    case Opcode.JumpEqual:
                    {
                        M_RawAction = m_io.ReadBytes((ushort)ActionLen);
                        var io___raw_action = new KaitaiStream(M_RawAction);
                        Action = new JumpData(io___raw_action, this, M_Root);
                        break;
                    }
                    case Opcode.Push:
                    {
                        M_RawAction = m_io.ReadBytes((ushort)ActionLen);
                        var io___raw_action = new KaitaiStream(M_RawAction);
                        Action = new PushData(io___raw_action, this, M_Root);
                        break;
                    }
                    default:
                    {
                        M_RawAction = m_io.ReadBytes((ushort)ActionLen);
                        var io___raw_action = new KaitaiStream(M_RawAction);
                        Action = new Nothing(io___raw_action, this, M_Root);
                        break;
                    }
                }

            OffsetNext = new Instant(M_Io.Pos, m_io, this, M_Root);
        }
    }
}
