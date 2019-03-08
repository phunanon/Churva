using System;
using System.Linq;
using System.Text.RegularExpressions;

namespace ChurvaDotnet
{
    class Log
    {
	    public static bool DoLog;

	    public static void Error (string message, (ushort Line, ushort Column) pos, string text)
	    {
			ColourOut($"ERR ({pos.Line}:{pos.Column}): {message}:", ConsoleColor.White, ConsoleColor.DarkRed);
			ColourOut($" {text} \n", ConsoleColor.Black, ConsoleColor.White);
	    }

	    public static void Step (string step, string info)
	    {
		    if (!DoLog) return;
		    ColourOut("LOG", ConsoleColor.Black, ConsoleColor.White);
		    Console.WriteLine($" {step}\t{EscapeChars(info)}");
	    }

	    public static string EscapeChars (string str) => new string(str.SelectMany(c => c < 32 ? Regex.Escape(c.ToString()).ToCharArray() : new []{c}).ToArray());

	    private static void ColourOut (string text, ConsoleColor fore, ConsoleColor back)
	    {
		    ConsoleColor prevFore = Console.ForegroundColor;
		    ConsoleColor prevBack = Console.BackgroundColor;
		    Console.ForegroundColor = fore;
		    Console.BackgroundColor = back;
			Console.Write(text);
		    Console.ForegroundColor = prevFore;
		    Console.BackgroundColor = prevBack;
	    }

	    public static void InfoHeader () => ColourOut("\nINFO", ConsoleColor.Black, ConsoleColor.White);
    }
}
