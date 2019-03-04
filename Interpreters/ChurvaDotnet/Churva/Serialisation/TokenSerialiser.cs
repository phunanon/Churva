using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

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
                    Log.Error("End of serialisation competence", args.Peek().OriginalPosition, args.Peek().Text);
                    break;
                }
                prevAtom = args.CurrentIndex;
            }
            return bytes.ToArray();
        }

        #region Serialisation Rates

        private static void Rate1 (SerialisationArgs args)
        {
	        VarDeclSerialise(args);
	        AssignSerialise(args);
	        NewlineSerialise(args);
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

		private static void VarDeclSerialise (SerialisationArgs args)
		{
			if (args.Peek().Token == ParseToken.TEXT && args.Peek(1).Token == ParseToken.TEXT) {
				if (Dict.DataTypes.Contains(args.Peek().Text)) {
					args.OutToken(BinaryToken.DECL_VARIABLE);
					args.OutByte((byte) Enum.Parse<NativeDataType>(args.Peek().Text));
					args.OutBytes(GetNameBytes(args.Peek(1).Text, args.IsDebug));
					args.Eat(2);
				}
			}
		}

		private static void AssignSerialise (SerialisationArgs args)
		{
			var afterDecl = (args.Peek().Token == ParseToken.OP) && (args.Peek().Text == "=");
			var other = (args.Peek().Token == ParseToken.TEXT) && (args.Peek(1).Text == "=");
			if (!afterDecl && !other) return;
			MarkDebug(args);
			args.OutToken(BinaryToken.ASSIGN);
			if (afterDecl && args.CanPeek(-1)) {
				args.OutBytes(GetNameBytes(args.Peek(-1).Text, args.IsDebug));
				args.Eat();
			} else {
				args.OutBytes(GetNameBytes(args.Peek().Text, args.IsDebug));
				args.Eat(2);
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
			args.Eat();
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
			args.Eat();
		}

		private static void NewlineSerialise (SerialisationArgs args)
		{
			if (args.Peek().Token != ParseToken.ENDLIN) return;
			MarkDebug(args);
			args.OutToken(BinaryToken.NEWLINE);
			args.Eat();
		}

		#endregion

        #region Serialisation Helper Methods

        private static byte[] GetNameBytes (string text, bool isDebug) =>
	        isDebug ? GetStringBytes(text) : BitConverter.GetBytes(text.GetHashCode());

        private static byte[] GetStringBytes (string text) =>
	        BitConverter.GetBytes(text.Length).Concat(Encoding.UTF8.GetBytes(text)).ToArray();

        static void MarkDebug (SerialisationArgs args)
        {
	        if (!args.IsDebug) return;
	        args.OutBytes(BitConverter.GetBytes(args.Peek().OriginalPosition.Line));
	        args.OutBytes(BitConverter.GetBytes(args.Peek().OriginalPosition.Column));
        }

		#endregion

        #region SerialisationArgs

        private class SerialisationArgs
        {
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
                Eat(advance);
                return atom;
            }

            public ParseAtom Peek (int ahead = 0) => _atoms[CurrentIndex + ahead];

	        public bool CanPeek (int ahead = 0)
	        {
		        var peek = CurrentIndex + ahead;
                return peek < _atoms.Count && peek > 0;
	        }

	        public void Eat (int advance = 1) => CurrentIndex += advance;

	        public void OutToken (BinaryToken t) => _bytes.Add((byte)t);
	        public void OutByte (byte b) => _bytes.Add(b);
	        public void OutBytes (byte[] bytes) => _bytes.AddRange(bytes);
        }

        #endregion
    }
}
