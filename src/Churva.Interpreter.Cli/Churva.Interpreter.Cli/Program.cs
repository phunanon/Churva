using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using Churva.Interpreter.BluePrints.Attributes;

namespace Churva.Interpreter.Cli
{
    /// <summary>
    /// The main class of the application
    /// </summary>
    internal class Program
    {
        private const String CoreName = "Churva.Interpreter.Core.dll";
        private static Assembly _coreAsm;
        private static Dictionary<string, Type> _keywords = new Dictionary<string, Type>();

        /// <summary>
        /// The main entry point of the application
        /// </summary>
        /// <param name="args">The parameters provided by the user</param>
        private static void Main(string[] args)
        {
            Console.WriteLine("Loading keywords.");
            var dir = Assembly.GetExecutingAssembly().GetDirectory();
            _coreAsm = Assembly.LoadFile(Path.Combine(dir,CoreName));
            LoadKeywords();
        }

        private static void LoadKeywords()
        {
            var coreTypes = _coreAsm.GetTypes();
            
            foreach (var type in coreTypes)
            {
                var at = type.GetCustomAttribute<KeywordAttribute>();
                if (at == null) continue;
                _keywords.Add(at.Word, type);
                Console.WriteLine($"{type.Name} Loaded");
            }
        }

        private static void ValidateKeyword(IEnumerable<Attribute> attrs, Type type)
        {
            foreach (var attr in attrs)
            {
                var kw = attr as KeywordAttribute;
                if (kw == null) continue;

                _keywords.Add(kw.Word, type);
                Console.WriteLine($"Loaded {type.Name}");
            }
        }
    }
    
    public static class AssemblyExtensions{
        public static String GetDirectory(this Assembly assembly)
        {
            var cb = assembly.CodeBase;
            var ds = Uri.UnescapeDataString(new UriBuilder(cb).Path);
            return Path.GetDirectoryName(ds);
        }
    }
}