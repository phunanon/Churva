using System;
using Churva.Interpreter.BluePrints.Attributes;
using Churva.Interpreter.BluePrints.Interfaces;

namespace Churva.Interpreter.Core
{
    [Keyword(Word = "u08")]
    public class U08 : IKeyword
    {
        public bool Validate()
        {
            throw new NotImplementedException();
        }
    }
}