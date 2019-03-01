using System;
using System.Collections.Generic;
using System.Text;

namespace ChurvaInterpreter
{
    class Log
    {
	    public static void Error (string message, (ushort Line, ushort Column) pos, string text)
	    {
		    var prevBack = Console.BackgroundColor;
		    var prevFore = Console.ForegroundColor;
		    Console.BackgroundColor = ConsoleColor.DarkRed;
		    Console.ForegroundColor = ConsoleColor.White;
            Console.Write($" ERR ({pos.Line}:{pos.Column}): {message}:");
		    Console.BackgroundColor = ConsoleColor.White;
		    Console.ForegroundColor = ConsoleColor.Black;
			Console.WriteLine($" {text} ");
		    Console.BackgroundColor = prevBack;
		    Console.ForegroundColor = prevFore;
	    }
    }
}
