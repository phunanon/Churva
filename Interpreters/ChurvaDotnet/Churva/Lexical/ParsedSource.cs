using System;
using System.Text.RegularExpressions;

namespace ChurvaDotnet
{
	internal class ParseAtom
    {
	    public (ushort Line, ushort Column) OriginalPos;
	    public ParseToken Token;
	    public string Text;

        public ParseAtom(ushort line, ushort column, ParseToken token, string text)
        {
            OriginalPos = (line, column);
            Token = token;
            Text = text;
        }

	    public override string ToString ()
	    {
		    var text = Text.Length == 1 && Text[0] < 32 ? Regex.Escape(Text) : Text;
		    var tokenName = Dict.ParseTokens[(int)Token];
		    return $"{OriginalPos.Line,-5}{OriginalPos.Column,-4}{tokenName,-8}{text}";
	    }
    }
}
