namespace ChurvaInterpreter
{
    public enum BinaryToken : byte
    {
        HALT,
        DEBUG,
        DECL_VARIABLE, DECL_POINTER,
        ASSIGN,
        NEWLINE
    }

    public enum NativeDataType : byte
    {
        var,
        u08, u16, u32, u64,
        i08, i16, i32, i64
    }
}