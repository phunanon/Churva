using System;
using System.IO;

namespace ChurvaDotnet
{
    class Program
    {
        static void Main(string[] args)
        {
	        bool isDebug;
	        Interpret("#!/usr/bin/churva\r\nvar file = File.readAll(\"names.txt\")\r\neach name, n: var names = file.delimit(\'\\n\')\r\n\tTerm.stamp(\"{n}/{names.Length}\\t{name}\")\r\nTerm.stamp(\"Complete.\")".Split('\n'));
            if (args.Length == 0) {
				Console.WriteLine("Churva Interactive\n");
		        while (true) {
					Console.Write(">");
			        var input = Console.ReadLine();
			        isDebug = IsDebug(ref input);
			        Interpret(new[] {input}, isDebug);
		        }
	        } else {
	            isDebug = IsDebug(ref args[0]);
		        Interpret(File.ReadAllLines(args[0]), isDebug);
	        }
        }

	    private static bool IsDebug (ref string input)
	    {
		    if (!input.StartsWith("release ", StringComparison.InvariantCultureIgnoreCase)) return true;
		    input = input.Remove(0, 8);
		    return false;
	    }

	    private static void Interpret (string[] source, bool isDebug = true)
	    {
		    var atoms = LexicalParser.ParseSourceLines(source);
		    var bytes = TokenSerialiser.Serialise(atoms, isDebug);
		    //var context = 

			Console.WriteLine($"\n{atoms.Count} atoms.");
		    foreach (var atom in atoms) Console.WriteLine(atom);
		    foreach (var b in bytes) Console.Write($"{b:X2}");
		    Console.WriteLine($"\n{bytes.Length}B");
	    }
    }
}
