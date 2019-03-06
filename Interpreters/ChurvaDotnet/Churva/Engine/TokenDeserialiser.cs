using System;
using System.Collections.Generic;

namespace ChurvaDotnet
{
    internal class TokenDeserialiser
    {
	    public static ChurvaProgram Deserialise (byte[] bytes)
	    {
		    var expressions = new List<ChurvaEx>();

		    for (var b = 0; b < bytes.Length; ++b) {
			    if (!Enum.IsDefined(typeof(NativeToken), bytes[b])) continue;
			    switch ((NativeToken)bytes[b]) {
				    case NativeToken.HALT:
					    b = bytes.Length;
					    break;
				    case NativeToken.DEBUG: break;
				    case NativeToken.NEWLINE: break;
				    case NativeToken.INDENT: break;
				    case NativeToken.DEDENT: break;
				    case NativeToken.DECL_NATVAR: break;
				    case NativeToken.DECL_USERVAR: break;
				    case NativeToken.DECL_NATPOI: break;
				    case NativeToken.DECL_USERPOI: break;
				    case NativeToken.DECL_SUB: break;
				    case NativeToken.ASSIGN: break;
				    case NativeToken.REFERENCE: break;
				    case NativeToken.LIT_INT: break;
				    case NativeToken.LIT_FLO: break;
				    case NativeToken.LIT_CHR: break;
				    case NativeToken.LIT_STR: break;
				    case NativeToken.OPERATOR: break;
				    case NativeToken.SCOPE: break;
				    case NativeToken.ST_EACHIT: break;
				    case NativeToken.ST_EACH: break;
				    default: throw new ArgumentOutOfRangeException();
			    }
		    }

		    return new ChurvaProgram(expressions);
	    }
    }
}