using System;

namespace ChurvaDotnet
{
	internal class ParseAtom
    {
	    public (ushort Line, ushort Column) OriginalPosition;
	    public ParseToken Token;
	    public string Text;

        public ParseAtom(ushort line, ushort column, ParseToken token, string text)
        {
            OriginalPosition = (line, column);
            Token = token;
            Text = text;
        }

	    public override string ToString () => $"{OriginalPosition.Line,-5}{OriginalPosition.Column,-4}{Enum.GetName(typeof(ParseToken), Token),-8}{Text}";
    }
}
