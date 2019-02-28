using System;
using Churva.Interpreter.BluePrints.Attributes;
using Churva.Interpreter.BluePrints.Interfaces;

namespace Churva.Interpreter.Core
{
    [Keyword(Words = new []{"nul"})]
    public class Nul: ValueType<Object>, IKeyword
    {
        public String InstanceName { get; set; }

        public override Boolean SetValueByObject(Object obj)
        {
            throw new NotImplementedException();
        }

        public override Boolean Validate(String[] strings)
        {
            throw new System.NotImplementedException();
        }

        public Boolean ValidateValue<T>()
        {
            throw new NotImplementedException();
        }

        public override Object Value { get; set; } = null;
    }
}