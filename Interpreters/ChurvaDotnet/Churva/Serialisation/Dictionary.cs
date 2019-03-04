using System;
using System.Linq;

namespace ChurvaDotnet
{
	internal enum ParseToken
	{
		UNKNOWN,
		TEXT, ENDLIN, ENDBLK, OP, NUMBER, STRING, CHAR, INDENT, DEDENT
	}

    public enum BinaryToken : byte
    {
        HALT,
        DEBUG,
        DECL_VARIABLE, DECL_POINTER,
        ASSIGN,
        NEWLINE,
		REFERENCE,
		LIT_INT, LIT_FLO, LIT_CHR, LIT_STR,
		OPERATOR, SCOPE
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
		public static string[] DataTypes = Enum.GetNames(typeof(NativeDataType)).ToArray();
		public static string[] LongOps = {"==", "!!", "&&", "||"};
		public static char[] Operators = {'=', '+', '-', '*', '/'};
	}
}