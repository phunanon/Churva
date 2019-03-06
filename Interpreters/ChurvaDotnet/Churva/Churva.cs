using System;
using System.Collections.Generic;
using System.Text;

namespace ChurvaDotnet
{
	internal class Churva
    {
	    private ChurvaContext _context = new ChurvaContext();

	    internal void Interpret (ChurvaOptions options)
	    {
		    var atoms = LexicalParser.ParseSourceLines(options.Source);
		    Log.DoLog = options.LogSerialise;
		    var bytes = TokenSerialiser.Serialise(atoms, options.IsDebug);
		    Log.DoLog = options.LogDeserialise;
		    var program = TokenDeserialiser.Deserialise(bytes);
		    Log.DoLog = options.LogExecute;
		    ChurvaEngine.Execute(program, _context);

		    if (options.ListAtoms) {
			    Log.InfoHeader();
			    Console.WriteLine($" {atoms.Count} atoms:");
			    foreach (var atom in atoms) Console.WriteLine(atom);
		    }

		    if (options.EchoBytes) {
				Log.InfoHeader();
				Console.Write(" ");
			    foreach (var b in bytes) Console.Write($"{b:X2}");
			    Console.WriteLine($" ({bytes.Length}B)");
		    }
	    }
    }

	internal class ChurvaOptions
	{
		public IEnumerable<string> Source;
		public bool IsDebug;
		public bool ListAtoms;
		public bool LogSerialise;
		public bool EchoBytes;
		public bool LogDeserialise;
		public bool LogExecute;
		public bool SuppressErrors;
	}
}
