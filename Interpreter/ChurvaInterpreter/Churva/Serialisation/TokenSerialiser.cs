using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ChurvaInterpreter
{
    internal static class TokenSerialiser
    {
        #region Heuristic objects and lists, c'tor
        
        //private static BaseSerialiser _rate0;
        private static BaseSerialiser _rate1;
        private static AssignSerialiser _assignSerialiser;

        static TokenSerialiser ()
        {
            _rate1 = new NewlineSerialiser(_rate1);
            _rate1 = new AssignSerialiser(_rate1);
            _rate1 = new VarDeclSerialiser(_rate1);
            //_rate1 = new 
        }

        public static string[] DataTypeStrings = Enum.GetNames(typeof(NativeDataType)).ToArray();

        #endregion

        public static byte[] Serialise (List<ParseAtom> atoms, bool isDebug)
        {
            var bytes = new List<byte>();
            if (isDebug) bytes.Add((byte) BinaryToken.DEBUG);
            
            var atomReader = new AtomReader(atoms, isDebug);
            var previousAtom = 0;
            while (!atomReader.IsExhausted) {
                _rate1.Serialise(atomReader, bytes);
                if (previousAtom == atomReader.CurrentAtom) {
                    Console.WriteLine("Couldn't find anything more.");
                    break;
                }
                previousAtom = atomReader.CurrentAtom;
            }
            return bytes.ToArray();
        }

        #region Serialisation Classes

        private abstract class BaseSerialiser
        {
            protected readonly BaseSerialiser Child;
            public BaseSerialiser (BaseSerialiser child) => Child = child;
            public abstract void Serialise (AtomReader atoms, List<byte> bytes);

            protected byte[] GetNameBytes (string text, bool isDebug) => isDebug
                ? BitConverter.GetBytes(text.Length).Concat(Encoding.UTF8.GetBytes(text)).ToArray()
                : BitConverter.GetBytes(text.GetHashCode());
        }

        private class VarDeclSerialiser : BaseSerialiser
        {
            public VarDeclSerialiser (BaseSerialiser child) : base(child) { }

            public override void Serialise (AtomReader atoms, List<byte> bytes)
            {
                if (atoms.Peek().Token == ParseToken.TEXT && atoms.Peek(1).Token == ParseToken.TEXT) {
                    if (DataTypeStrings.Contains(atoms.Peek().Text)) {
                        bytes.Add((byte) BinaryToken.DECL_VARIABLE);
                        bytes.Add((byte) Enum.Parse<NativeDataType>(atoms.Peek().Text));
                        bytes.AddRange(GetNameBytes(atoms.Peek(1).Text, atoms.IsDebug));
                        atoms.Advance(2);
                    }
                }
                Child?.Serialise(atoms, bytes);
            }
        }

        private class AssignSerialiser : BaseSerialiser
        {
            public AssignSerialiser (BaseSerialiser child) : base(child) { }

            public override void Serialise (AtomReader atoms, List<byte> bytes)
            {
                var afterDecl = atoms.Peek().Token == ParseToken.OP && atoms.Peek().Text == "=";
                var other = atoms.Peek().Token == ParseToken.TEXT && atoms.Peek(1).Text == "=";
                if (afterDecl | other) {
                    bytes.Add((byte) BinaryToken.ASSIGN);
                    if (afterDecl) {
                        bytes.AddRange(GetNameBytes(atoms.Peek(-1).Text, atoms.IsDebug));
                        atoms.Advance();
                    } else {
                        bytes.AddRange(GetNameBytes(atoms.Peek().Text, atoms.IsDebug));
                        atoms.Advance(2);
                    }
                }
                Child?.Serialise(atoms, bytes);
            }
        }

        private class NewlineSerialiser : BaseSerialiser
        {
            public NewlineSerialiser (BaseSerialiser child) : base(child) { }

            public override void Serialise (AtomReader atoms, List<byte> bytes)
            {
                if (atoms.Peek().Token == ParseToken.NL) {
                    bytes.Add((byte) BinaryToken.NEWLINE);
                    atoms.Advance();
                }
                Child?.Serialise(atoms, bytes);
            }
        }

        #endregion

        #region Helper Classes

        private class AtomReader
        {
            private readonly List<ParseAtom> _atoms;
            public bool IsDebug;
            
            public AtomReader (List<ParseAtom> atoms, bool isDebug)
            {
                _atoms = atoms;
                IsDebug = isDebug;
            }

            public int CurrentAtom;
            public bool IsExhausted => CurrentAtom == _atoms.Count;

            public ParseAtom Next (int advance = 1)
            {
                var atom = _atoms[CurrentAtom];
                Advance(advance);
                return atom;
            }

            public ParseAtom Peek (int ahead = 0)
            {
                if (CurrentAtom + ahead >= _atoms.Count) return null;
                return _atoms[CurrentAtom + ahead];
            }

            public void Advance (int advance = 1) => CurrentAtom += advance;
        }

        #endregion
    }
}