using System;
using System.Collections.Generic;
using System.Text;

namespace ChurvaInterpreter
{
	internal class ParseAtom
    {
	    public (ushort Line, int Column) OriginalPosition;
	    public ChurvaToken Token;
	    public string Text;

        public ParseAtom(ushort line, int column, ChurvaToken token, string text)
        {
            OriginalPosition = (line, column);
            Token = token;
            Text = text;
        }

	    public override string ToString () => $"{OriginalPosition.Line}:{OriginalPosition.Column}\t{Enum.GetName(typeof(ChurvaToken), Token)} \t{Text}";
    }

	internal enum ChurvaToken
	{
		UNKNOWN,
		TEXT, NL, OP, NUMBER, STRING, CHAR, INDENT, DEDENT
	}
}
