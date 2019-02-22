using System;
using Churva.Interpreter.BluePrints.Attributes;
using Churva.Interpreter.BluePrints.Interfaces;

namespace Churva.Interpreter.Core
{
    [Keyword(Word = "u16")]
    public class U16:IKeyword
    {
        public bool Validate()
        {
            throw new NotImplementedException();
        }
    }
}