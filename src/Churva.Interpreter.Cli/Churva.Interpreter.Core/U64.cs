using System;
using Churva.Interpreter.BluePrints.Attributes;
using Churva.Interpreter.BluePrints.Interfaces;

namespace Churva.Interpreter.Core
{
    [Keyword(Word = "u64")]
    public class U64:IKeyword
    {
        public bool Validate()
        {
            throw new NotImplementedException();
        }
    }
}