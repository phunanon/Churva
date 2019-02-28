using System;
using System.Runtime.InteropServices;

namespace Churva.Interpreter.BluePrints
{
    public class RuntimeException: Exception
    {
        public RuntimeException(String message) : base(message)
        {
        }
    }
}