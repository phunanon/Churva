using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ChurvaDotnet
{
	internal class ChurvaProgram
    {
	    private ChurvaEx[] Expressions;

	    public ChurvaProgram (IEnumerable<ChurvaEx> expressions)
	    {
		    Expressions = expressions.ToArray();
	    }
    }

    internal class ChurvaEx
    {
	    
    }
}
