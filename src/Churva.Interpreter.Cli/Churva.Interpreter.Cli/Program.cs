using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using Churva.Interpreter.BluePrints;
using Churva.Interpreter.BluePrints.Attributes;
using Churva.Interpreter.BluePrints.Interfaces;
using static Churva.Interpreter.BluePrints.ChurvaEnvironment;

namespace Churva.Interpreter.Cli
{
    /// <summary>
    /// The main class of the application
    /// </summary>
    internal class Program
    {
        private const String CoreName = "Churva.Interpreter.Core.dll";
        private static Assembly _coreAsm;
        private static String _quit = "::quit";


        /// <summary>
        /// The main entry point of the application
        /// </summary>
        /// <param name="args">The parameters provided by the user</param>
        private static void Main(String[] args)
        {
            Console.WriteLine("Loading keywords.");
            var dir = Assembly.GetExecutingAssembly().GetDirectory();
            _coreAsm = Assembly.LoadFile(Path.Combine(dir,CoreName));
            LoadKeywords();
            var line = string.Empty;
            while (line != _quit)
            {
                try
                {
                    Console.Write("> ");
                    line = Console.ReadLine() ?? "";
                    if (line == _quit)
                        break;
                    var matched = false;
                    foreach (var key in Keywords.Keys)
                    {
                        var match = key?.Where(itm => line.StartsWith(itm)).ToList();
                        if (!match.Any()) continue;
                        matched = true;
                        var use = match.FirstOrDefault();
                        var type = Keywords[key];
                        Console.WriteLine($"Started with: {use}");
                        var instance = Activator.CreateInstance(type);
                        var val = instance.InvokeReturnMethod<Boolean>($"{nameof(IKeyword.Validate)}",
                            new Object[] {new[] {line}});
                        Console.WriteLine(val);
                    }

                    if (matched) continue;

                    var instanceExist = false;
                    if (!matched)
                    {
                        instanceExist = GetInstance(line.Trim(), out var instance);
                        if(instanceExist)
                            Console.WriteLine(instance.Value.ToString());
                    }
                    if(!instanceExist)
                    {
                        Console.WriteLine($"No instance called: {line}");
                    }
                }
                catch (TargetInvocationException e)
                {
                    Console.WriteLine($"{e.InnerException.Message}");
                }
                catch (RuntimeException e)
                {
                    Console.WriteLine($"{e.Message}");
                }
            }
        }

        private static void LoadKeywords()
        {
            var coreTypes = _coreAsm.GetTypes();
            
            foreach (var type in coreTypes)
            {
                var at = type.GetCustomAttribute<KeywordAttribute>();
                if (at == null) continue;
                Keywords.Add(at.Words, type);
                Console.WriteLine($"{type.Name} Loaded - {string.Join(",", at.Words)}");
            }
        }
    }
}