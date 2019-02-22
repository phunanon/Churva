using System;
using Churva.Interpreter.BluePrints.Attributes;
using Churva.Interpreter.BluePrints.Interfaces;

namespace Churva.Interpreter.Core
{
    [Keyword(Word = "i32")]
    public class I32:IKeyword
    {
        public bool Validate()
        {
            throw new NotImplementedException();
        }
    }
}