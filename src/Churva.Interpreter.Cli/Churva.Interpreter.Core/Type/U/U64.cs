using System;
using Churva.Interpreter.BluePrints;
using Churva.Interpreter.BluePrints.Attributes;
using Churva.Interpreter.BluePrints.Interfaces;

namespace Churva.Interpreter.Core.Type.U
{
    [Keyword(Words = new[] {"u64"})]
    public class U64 : ValueType<UInt64>
    {
        public override Boolean SetValueByObject(Object obj)
        {
            if (String.IsNullOrWhiteSpace(obj.ToString()))
                throw new RuntimeException("ERROR: Expected a Value");
            if (!UInt64.TryParse(obj.ToString(), out _value))
                throw new RuntimeException("ERROR: Not a number");
            return true;
        }

        private UInt64 _value;

        public override UInt64 Value
        {
            get => _value;
            set => _value = value;
        }
    }
}