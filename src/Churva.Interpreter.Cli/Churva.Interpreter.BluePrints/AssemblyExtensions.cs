using System;
using System.IO;
using System.Reflection;

namespace Churva.Interpreter.BluePrints
{
    public static class AssemblyExtensions{
        public static String GetDirectory(this Assembly assembly)
        {
            var cb = assembly.CodeBase;
            var ds = Uri.UnescapeDataString(new UriBuilder(cb).Path);
            return Path.GetDirectoryName(ds);
        }
    }
}