using System;
using System.Collections.Generic;
using System.Text;

namespace ChurvaInterpreter
{
	internal class ParseAtom
    {
	    public (ushort Line, int Column) OriginalPosition;
	    public ParseToken Token;
	    public string Text;

        public ParseAtom(ushort line, int column, ParseToken token, string text)
        {
            OriginalPosition = (line, column);
            Token = token;
            Text = text;
        }

	    public override string ToString () => $"{OriginalPosition.Line,-5}{OriginalPosition.Column,-4}{Enum.GetName(typeof(ParseToken), Token),-8}{Text}";
    }

	internal enum ParseToken
	{
		UNKNOWN,
		TEXT, NL, OP, NUMBER, STRING, CHAR, INDENT, DEDENT
	}
}
