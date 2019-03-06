using System;
using System.Linq;

namespace ChurvaDotnet
{
	internal enum ParseToken
	{
		UNKNOWN,
		TEXT, ENDLIN, ENDBLK, OP, NUMBER, STRING, CHAR, INDENT, DEDENT
	}

    public enum NativeToken : byte
    {
        HALT,
        DEBUG,
        NEWLINE, INDENT, DEDENT,
        DECL_NATVAR, DECL_USERVAR, DECL_NATPOI, DECL_USERPOI,
		DECL_SUB,
        ASSIGN,
		REFERENCE,
		LIT_INT, LIT_FLO, LIT_CHR, LIT_STR,
		OPERATOR, SCOPE,
		ST_EACHIT, ST_EACH,
    }

    public enum NativeDataType : byte
    {
        var,
        boo,
        u08, u16, u32, u64,
        i08, i16, i32, i64,
        dec,
        str
    }

	public static class Dict
	{
		public static string[] ParseTokens = Enum.GetNames(typeof(ParseToken));
		public static string[] DataTypes = Enum.GetNames(typeof(NativeDataType));
		public static string[] Statements = {"each"};
		public static string[] LongOps = {"==", "!!", "&&", "||", "=>"};
		public static string[] Operators = new []{"=", "+", "-", "*", "/", ".", "(", ")", ";"}.Concat(LongOps).ToArray();
	}
}