using System;
using Churva.Interpreter.BluePrints;
using Churva.Interpreter.BluePrints.Attributes;
using Churva.Interpreter.BluePrints.Interfaces;

namespace Churva.Interpreter.Core.Type.F
{
    [Keyword(Words = new[] {"boo","true","false"})]
    public class Boo : ValueType<Boolean>
    {
        private Boolean _value;

        public override Boolean SetValueByObject(Object obj)
        {
            if (String.IsNullOrWhiteSpace(obj.ToString()))
                throw new RuntimeException("ERROR: Expected a Value");
            if (!Boolean.TryParse(obj.ToString(), out _value))
                throw new RuntimeException("ERROR: Not a number");
            return true;
        }

        public override Boolean Value
        {
            get => _value;
            set => _value = value;
        }
    }
}