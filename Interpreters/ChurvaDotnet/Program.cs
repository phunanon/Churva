using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;

namespace ChurvaDotnet
{
    class Program
    {
		private static readonly Churva _churva = new Churva();

        static void Main(string[] args)
        {
	        ResetColour();

	        _churva.Interpret(
		        OptionsFromArgs(
			        ":absder",
			        "#!/usr/bin/churva\r\nvar file = File.readAll(\"names.txt\")\r\neach name, n: var names = file.delimit(\'\\n\')\r\n\tTerm.stamp(\"{n}/{names.Length}\\t{name}\")\r\nTerm.stamp(\"Complete.\")"
				        .Split('\n')
		        )
	        );

            if (args.Length == 0) {
				Console.WriteLine("Churva Interactive\n");
		        while (true) {
					Console.Write(">");
					Console.ForegroundColor = ConsoleColor.Cyan;
			        var input = Console.ReadLine();
					ResetColour();
			        if (input == null) continue;
			        var parameters = "";
			        if (input.StartsWith(':')) parameters = new string(input.TakeWhile(c => c != ' ').ToArray());
			        _churva.Interpret(OptionsFromArgs(parameters, new[] {input.Substring(parameters.Length)}));
		        }
	        } else {
	            ChurvaOptions options = args.Length == 1
		            ? OptionsFromArgs("", File.ReadAllLines(args[0]))
		            : OptionsFromArgs(args[0], File.ReadAllLines(args[1]));
	            _churva.Interpret(options);
            }
        }

        private static ChurvaOptions OptionsFromArgs (string args, IEnumerable<string> source)
	    {
		    var options = new ChurvaOptions {IsDebug = true, Source = source};
		    if (args.Length == 0)
			    return options;

			foreach (var arg in args) {
				switch (arg) {
					case 'r':
						options.IsDebug = false;
						break;
					case 'a':
						options.ListAtoms = true;
						break;
					case 'b':
						options.EchoBytes = true;
						break;
					case 's':
						options.LogSerialise = true;
						break;
					case 'd': options.LogDeserialise = true;
						break;
					case 'e': options.LogExecute = true;
						break;
				}
			}

		    return options;
	    }

        private static void ResetColour ()
        {
	        Console.ForegroundColor = ConsoleColor.White;
	        Console.BackgroundColor = ConsoleColor.Black;
        }
    }
}
