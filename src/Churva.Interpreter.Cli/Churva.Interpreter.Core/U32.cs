using System;
using Churva.Interpreter.BluePrints.Attributes;
using Churva.Interpreter.BluePrints.Interfaces;

namespace Churva.Interpreter.Core
{
    [Keyword(Word = "u32")]
    public class U32:IKeyword
    {
        public bool Validate()
        {
            throw new NotImplementedException();
        }
    }
}