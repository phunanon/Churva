using System;
using Churva.Interpreter.BluePrints.Attributes;
using Churva.Interpreter.BluePrints.Interfaces;

namespace Churva.Interpreter.Core
{
    [Keyword(Word = "i64")]
    public class I64:IKeyword
    {
        public bool Validate()
        {
            throw new NotImplementedException();
        }
    }
}