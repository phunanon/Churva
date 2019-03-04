using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
//TODO: name.name = assignment, name.name()
namespace ChurvaDotnet
{
    internal static class TokenSerialiser
    {
        public static byte[] Serialise (List<ParseAtom> atoms, bool isDebug)
        {
            var bytes = new List<byte>();
            if (isDebug) bytes.Add((byte) BinaryToken.DEBUG);
            
            var args = new SerialisationArgs(atoms, bytes, isDebug);
            var prevAtom = 0;
            while (!args.IsExhausted) {
                Rate1(args);
                if (prevAtom == args.CurrentIndex) {
                    Log.Error("End of serialisation competence", args.Peek().OriginalPos, args.Peek().Text);
                    break;
                }
                prevAtom = args.CurrentIndex;
            }
            return bytes.ToArray();
        }

        #region Serialisation Rates

        private static void Rate1 (SerialisationArgs args)
        {
	        StatementSerialise(args);
	        VarDeclSerialise(args);
	        AssignSerialise(args);
	        NewlineSerialise(args);
	        IndentSerialise(args);
        }

        private static void Rate2 (SerialisationArgs args)
        {
	        VarDeclSerialise(args);
	        AssignSerialise(args);
	        NameRefSerialise(args);
	        LiteralSerialise(args);
	        OperatorSerialise(args);
        }

		#endregion

		#region Serialisation Methods

		private static void StatementSerialise (SerialisationArgs args)
		{
			var statement = Array.IndexOf(Dict.Statements, args.Peek().Text);
			if (statement == -1) return;
			switch (args.Next().Text) {
				case "each":
					Action OutNextName = () => args.OutBytes(GetNameBytes(args.Next().Text, args.IsDebug));
					//Determine if with or without iterator
					if (args.Peek(1).Text == ",") {
						args.OutToken(BinaryToken.ST_EACHIT);
						OutNextName();
						args.Skip(); //,
					} else args.OutToken(BinaryToken.ST_EACH);
					OutNextName();
					args.Skip(); //:
					OutNextName();
					break;
			}
		}

		private static void VarDeclSerialise (SerialisationArgs args)
		{
			if (args.Peek().Token == ParseToken.TEXT && args.Peek(1).Token == ParseToken.TEXT) {
				if (Dict.DataTypes.Contains(args.Peek().Text)) {
					args.OutToken(BinaryToken.DECL_VARIABLE);
					args.OutByte((byte) Enum.Parse<NativeDataType>(args.Peek().Text));
					args.OutBytes(GetNameBytes(args.Peek(1).Text, args.IsDebug));
					args.Skip(2);
				}
			}
		}

		private static void AssignSerialise (SerialisationArgs args)
		{
			var afterDecl = (args.Peek().Token == ParseToken.OP) && (args.Peek().Text == "=");
			var other = (args.Peek().Token == ParseToken.TEXT) && (args.Peek(1).Text == "=");
			if (!afterDecl && !other) return;
			args.OutToken(BinaryToken.ASSIGN);
			if (afterDecl && args.CanPeek(-1)) {
				args.OutBytes(GetNameBytes(args.Peek(-1).Text, args.IsDebug));
				args.Skip();
			} else {
				args.OutBytes(GetNameBytes(args.Peek().Text, args.IsDebug));
				args.Skip(2);
			}
			//Defer to 2nd Rate Serialiser
			int prevAtom;
			do {
				prevAtom = args.CurrentIndex;
				Rate2(args);
			} while (args.CurrentIndex != prevAtom);
		}

		private static void NameRefSerialise (SerialisationArgs args)
		{
			if (args.Peek().Token != ParseToken.TEXT) return;
			args.OutToken(BinaryToken.REFERENCE);
			args.OutBytes(GetNameBytes(args.Peek().Text, args.IsDebug));
			args.Skip();
		}

		private static void LiteralSerialise (SerialisationArgs args)
		{
			switch (args.Peek().Token) {
				case ParseToken.NUMBER:
					if (args.Peek().Text.Contains('.')) {
						args.OutToken(BinaryToken.LIT_FLO);
						args.OutBytes(BitConverter.GetBytes(float.Parse(args.Next().Text)));
					} else {
						args.OutToken(BinaryToken.LIT_INT);
						args.OutBytes(BitConverter.GetBytes(int.Parse(args.Next().Text)));
					}
					break;
				case ParseToken.STRING:
					args.OutToken(BinaryToken.LIT_STR);
					args.OutBytes(GetStringBytes(args.Next().Text));
					break;
				case ParseToken.CHAR:
					args.OutToken(BinaryToken.LIT_CHR);
					args.OutByte((byte)char.Parse(args.Next().Text));
					break;
			}
		}

		private static void OperatorSerialise (SerialisationArgs args)
		{
			if (args.Peek().Token != ParseToken.OP) return;
			var opChar = Array.IndexOf(Dict.Operators, char.Parse(args.Peek().Text));
			if (opChar <= -1) return;
			args.OutToken(BinaryToken.OPERATOR);
			args.OutByte((byte)opChar);
			args.Skip();
		}

		private static void NewlineSerialise (SerialisationArgs args)
		{
			if (args.Peek().Token != ParseToken.ENDLIN) return;
			args.OutToken(BinaryToken.NEWLINE);
			args.Skip();
		}

		private static void IndentSerialise (SerialisationArgs args)
		{
			var isIndent = args.Peek().Token == ParseToken.INDENT;
			var isDedent = args.Peek().Token == ParseToken.DEDENT;
			if (!isIndent && !isDedent) return;
			args.OutToken(isIndent ? BinaryToken.INDENT : BinaryToken.DEDENT);
			args.OutByte(byte.Parse(args.Next().Text));
		}

		#endregion

        #region Serialisation Helper Methods

        private static byte[] GetNameBytes (string text, bool isDebug) =>
	        isDebug ? GetStringBytes(text) : BitConverter.GetBytes(text.GetHashCode());

        private static byte[] GetStringBytes (string text) =>
	        BitConverter.GetBytes(text.Length).Concat(Encoding.UTF8.GetBytes(text)).ToArray();

		#endregion

        #region SerialisationArgs

        private class SerialisationArgs
        {
			private readonly ParseAtom _nullAtom = new ParseAtom(0, 0, ParseToken.UNKNOWN, "");
            private readonly List<ParseAtom> _atoms;
	        private readonly List<byte> _bytes;
            public bool IsDebug;
            
            public SerialisationArgs (List<ParseAtom> atoms, List<byte> bytes, bool isDebug)
            {
                _atoms = atoms;
	            _bytes = bytes;
                IsDebug = isDebug;
            }

	        public int CurrentIndex;
	        public ParseAtom CurrentAtom => _atoms[CurrentIndex];
            public bool IsExhausted => CurrentIndex == _atoms.Count;

            public ParseAtom Next (int advance = 1)
            {
                var atom = _atoms[CurrentIndex];
                Skip(advance);
                return atom;
            }

            public ParseAtom Peek (int ahead = 0) => CurrentIndex + ahead < _atoms.Count ? _atoms[CurrentIndex + ahead] : _nullAtom;

            public bool CanPeek (int ahead = 0)
	        {
		        var peek = CurrentIndex + ahead;
                return peek < _atoms.Count && peek > 0;
	        }

	        public void Skip (int advance = 1) => CurrentIndex += advance;

	        public void OutToken (BinaryToken t)
	        {
				//Mark line & column before token
		        if (IsDebug) {
			        OutBytes(BitConverter.GetBytes(Peek().OriginalPos.Line));
			        OutBytes(BitConverter.GetBytes(Peek().OriginalPos.Column));
		        }
				//Output token
		        _bytes.Add((byte)t);
	        }

	        public void OutByte (byte b) => _bytes.Add(b);
	        public void OutBytes (byte[] bytes) => _bytes.AddRange(bytes);
        }

        #endregion
    }
}