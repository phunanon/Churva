using System;
using Churva.Interpreter.BluePrints.Attributes;
using Churva.Interpreter.BluePrints.Interfaces;

namespace Churva.Interpreter.Core
{
    [Keyword(Word = "i16")]
    public class I16:IKeyword
    {
        public bool Validate()
        {
            throw new NotImplementedException();
        }
    }
}