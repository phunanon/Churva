using System;
using Churva.Interpreter.BluePrints.Attributes;
using Churva.Interpreter.BluePrints.Interfaces;

namespace Churva.Interpreter.Core
{
    [Keyword(Word="i08")]
    public class I08:IKeyword
    {
        public bool Validate()
        {
            throw new NotImplementedException();
        }
    }
}