using System;
using Churva.Interpreter.BluePrints;
using Churva.Interpreter.BluePrints.Attributes;
using Churva.Interpreter.BluePrints.Interfaces;

namespace Churva.Interpreter.Core.Type.U
{
    [Keyword(Words = new[] {"u32"})]
    public class U32 : ValueType<UInt32>
    {
        public override Boolean SetValueByObject(Object obj)
        {
            if (String.IsNullOrWhiteSpace(obj.ToString()))
                throw new RuntimeException("ERROR: Expected a Value");
            if (!UInt32.TryParse(obj.ToString(), out _value))
                throw new RuntimeException("ERROR: Not a number");
            return true;
        }

        private UInt32 _value;

        public override UInt32 Value
        {
            get => _value;
            set => _value = value;
        }
    }
}