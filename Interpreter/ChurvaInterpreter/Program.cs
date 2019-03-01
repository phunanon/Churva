using System;
using System.IO;

namespace ChurvaInterpreter
{
    class Program
    {
        static void Main(string[] args)
        {
	        Interpret("#!/usr/bin/churva\r\nvar file = File.readAll(\"names.txt\")\r\neach name, n: var names = file.delimit(\'\\n\')\r\n\tTerm.stamp(\"{n}/{names.Length}\\t{name}\")\r\nTerm.stamp(\"Complete.\")".Split('\n'));
            if (args.Length == 0) {
				Console.WriteLine("Churva Interactive\n");
		        while (true) {
					Console.Write(">");
			        var input = Console.ReadLine();
			        Interpret(new[] {input});
		        }
	        } else {
		        Interpret(File.ReadAllLines(args[0]));
	        }
        }

	    private static void Interpret (string[] source)
	    {
		    var atoms = LexicalParser.ParseSourceLines(source);
		    var bytes = TokenSerialiser.Serialise(atoms, true);
		    //var context = 

			Console.WriteLine($"{atoms.Count} atoms.");
		    foreach (var atom in atoms) Console.WriteLine(atom);
		    foreach (var b in bytes) Console.Write($"{b:X2}");
		    Console.WriteLine();
	    }
    }
}
