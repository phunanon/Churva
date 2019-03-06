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
            if (isDebug) bytes.Add((byte) NativeToken.DEBUG);
            
            Rate1(new SerialisationArgs(atoms, bytes, isDebug));

            return bytes.ToArray();
        }

        #region Serialisation Rates

        private static void Rate1 (SerialisationArgs args)
        {
	        var prevAtom = 0;
	        while (!args.IsExhausted) {
		        StatementSerialise(args);
		        SubDeclSerialise(args);
		        VarDeclSerialise(args);
		        AssignSerialise(args);
		        NameRefSerialise(args);
		        NewlineSerialise(args);
		        IndentSerialise(args);
		        if (prevAtom == args.CurrentIndex) {
			        Log.Error("End of serialisation competence", args.Peek().OriginalPos, args.Peek().Text);
			        return;
		        }
		        prevAtom = args.CurrentIndex;
	        }
        }

        private static void Rate2 (SerialisationArgs args)
        {
	        var prevAtom = 0;
	        while (!args.IsExhausted) {
		        VarDeclSerialise(args);
		        AssignSerialise(args);
		        NameRefSerialise(args);
		        LiteralSerialise(args);
		        OperatorSerialise(args);
		        if (prevAtom == args.CurrentIndex)
			        return;
		        prevAtom = args.CurrentIndex;
	        }
        }

		#endregion

		#region Serialisation Methods

		private static void StatementSerialise (SerialisationArgs args)
		{
			var statement = Array.IndexOf(Dict.Statements, args.Peek().Text);
			if (statement == -1) return;
			var logInfo = $"{args.Peek(0).Text} ";
			switch (args.Next().Text) {
				case "each":
					void OutNextName () => args.OutBytes(GetNameBytes(args.Next().Text, args));
					//Determine if with or without iterator
					if (args.Peek(1).Text == ",") {
						logInfo += $"\"{args.Peek().Text}\"";
						args.OutToken(NativeToken.ST_EACHIT);
						OutNextName();
						args.Skip(); //,
					} else {
						args.OutToken(NativeToken.ST_EACH);
					}

					logInfo += $" \"{args.Peek().Text}\"";
					OutNextName();
					args.Skip(); //:
					break;
			}
			Log.Step("Serial: statement", logInfo);
		}

		/// <summary>Serialises type with native/user prefix</summary>
		private static void OutNextType (SerialisationArgs args)
		{
			if (IsNativeType(args.Peek().Text)) {
				args.OutByte(0x10);
				args.OutByte((byte)Enum.Parse<NativeDataType>(args.Next().Text));
			} else {
				args.OutByte(0x11);
				args.OutBytes(GetNameBytes(args.Next().Text, args));
			}
		}

		private static void OutNextTypeAndName (SerialisationArgs args)
		{
			OutNextType(args);
			args.OutBytes(GetNameBytes(args.Next().Text, args));
		}

		private static void SubDeclSerialise (SerialisationArgs args)
		{
			if (args.Peek().Text != "sub") return;
			var pos = args.Peek().OriginalPos;
			var subName = args.Peek(1).Text;
			Log.Step("Serial: subroutine", subName);
			args.OutToken(NativeToken.DECL_SUB);
			args.OutBytes(GetNameBytes(subName, args));
			args.Skip(2); //sub name
			//Next byte is: 4-bit scheme, 4-bit 0:native 1:user return type
			if (args.Peek().Token == ParseToken.ENDLIN || args.Peek().Text == "=>") {
				//No parameters, no returns
				args.OutByte(0x00);
				args.Skip();
			} else {
				if (args.Peek().Text == ":") {
					//No parameters, returns
					args.Skip(); //:
					OutNextType(args);
				} else if (args.Peek().Text == "(") {
					byte timeout = 0;
					while (args.Peek().Text != ")" && timeout++ < 255) {
						args.Skip(); //( or ,
						OutNextTypeAndName(args);
					}
					if (timeout == 255) Log.Error($"sub `{subName}` parameters unclosed", pos, "");
					else args.Skip(); //)
				}
			}
		}

		private static bool IsNativeType (string type) => Dict.DataTypes.Contains(type);

		private static void VarDeclSerialise (SerialisationArgs args)
		{
			if (args.Peek().Token == ParseToken.TEXT && args.Peek(1).Token == ParseToken.TEXT) {
				Log.Step("Serial: vardecl", $"{args.Peek().Text} \"{args.Peek(1).Text}\"");
				//Are we declaring native, or user type?
				if (IsNativeType(args.Peek().Text)) {
					args.OutToken(NativeToken.DECL_NATVAR);
					args.OutByte((byte) Enum.Parse<NativeDataType>(args.Next().Text)); //type
					args.OutBytes(GetNameBytes(args.Next().Text, args)); //name
				} else {
					args.OutToken(NativeToken.DECL_USERVAR);
					args.OutBytes(GetNameBytes(args.Next().Text, args)); //type
					args.OutBytes(GetNameBytes(args.Next().Text, args)); //name
				}
			}
		}

		private static void AssignSerialise (SerialisationArgs args)
		{
			var afterDecl = (args.Peek().Token == ParseToken.OP) && (args.Peek().Text == "=");
			var other = (args.Peek().Token == ParseToken.TEXT) && (args.Peek(1).Text == "=");
			if (!afterDecl && !other) return;

			args.OutToken(NativeToken.ASSIGN);
			if (afterDecl && args.CanPeek(-1)) {
				Log.Step("Serial: assign", $"{args.Peek(-1).Text}=");
				args.OutBytes(GetNameBytes(args.Peek(-1).Text, args));
				args.Skip();
			} else {
				Log.Step("Serial: assign", $"{args.Peek().Text}=");
				args.OutBytes(GetNameBytes(args.Peek().Text, args));
				args.Skip(2);
			}
			//Defer to 2nd Rate Serialiser
			Rate2(args);
		}

		private static void NameRefSerialise (SerialisationArgs args)
		{
			if (args.Peek().Token != ParseToken.TEXT) return;
			Log.Step("Serial: nameref", args.Peek().Text);
			args.OutToken(NativeToken.REFERENCE);
			args.OutBytes(GetNameBytes(args.Peek().Text, args));
			args.Skip();
			Rate2(args);
		}

		private static void LiteralSerialise (SerialisationArgs args)
		{
			var log = true;
			switch (args.Peek().Token) {
				case ParseToken.NUMBER:
					if (args.Peek().Text.Contains('.')) {
						args.OutToken(NativeToken.LIT_FLO);
						args.OutBytes(BitConverter.GetBytes(float.Parse(args.Next().Text)));
					} else {
						args.OutToken(NativeToken.LIT_INT);
						args.OutBytes(BitConverter.GetBytes(int.Parse(args.Next().Text)));
					}
					break;
				case ParseToken.STRING:
					args.OutToken(NativeToken.LIT_STR);
					args.OutBytes(GetStringBytes(args.Next().Text));
					break;
				case ParseToken.CHAR:
					args.OutToken(NativeToken.LIT_CHR);
					args.OutByte((byte)char.Parse(args.Next().Text));
					break;
				default:
					log = false;
					break;
			}
			if (log) Log.Step("Serial: literal", args.Peek(-1).Text);
		}

		private static void OperatorSerialise (SerialisationArgs args)
		{
			if (args.Peek().Token != ParseToken.OP) return;
			var opChar = Array.IndexOf(Dict.Operators, args.Peek().Text);
			if (opChar <= -1) return;
			Log.Step("Serial: operator", args.Peek().Text);
			args.OutToken(NativeToken.OPERATOR);
			args.OutByte((byte)opChar);
			args.Skip();
		}

		private static void NewlineSerialise (SerialisationArgs args)
		{
			if (args.Peek().Token != ParseToken.ENDLIN) return;
			Log.Step("Serial: newline", "");
			args.OutToken(NativeToken.NEWLINE);
			args.Skip();
		}

		private static void IndentSerialise (SerialisationArgs args)
		{
			var isIndent = args.Peek().Token == ParseToken.INDENT;
			var isDedent = args.Peek().Token == ParseToken.DEDENT;
			if (!isIndent && !isDedent) return;
			Log.Step("Serial: indent", args.Peek().Text);
			args.OutToken(isIndent ? NativeToken.INDENT : NativeToken.DEDENT);
			args.OutByte(byte.Parse(args.Next().Text));
		}

		#endregion

        #region Serialisation Helper Methods

        private static byte[] GetNameBytes (string text, SerialisationArgs args) =>
	        args.IsDebug ? GetStringBytes(text) : BitConverter.GetBytes(text.GetHashCode());

        private static byte[] GetStringBytes (string text) =>
	        BitConverter.GetBytes(text.Length).Concat(Encoding.UTF8.GetBytes(text)).ToArray();

		#endregion

        #region SerialisationArgs

        private class SerialisationArgs
        {
			private readonly ParseAtom _nullAtom = new ParseAtom(0, 0, ParseToken.UNKNOWN, "");
            private readonly List<ParseAtom> _atoms;
	        private readonly List<byte> _bytes;
            public readonly bool IsDebug;
            
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
	            if (!CanPeek(advance)) return _nullAtom;
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

	        public void OutToken (NativeToken t)
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