using System;
using System.Linq;
using Churva.Interpreter.BluePrints;
using Churva.Interpreter.BluePrints.Attributes;
using Churva.Interpreter.BluePrints.Interfaces;

namespace Churva.Interpreter.Core.Type.I
{
    [Keyword(Words = new[] {"i64"})]
    public class I64 : ValueType<Int64>
    {
        public override Boolean SetValueByObject(Object obj)
        {
            if (String.IsNullOrWhiteSpace(obj.ToString()))
                throw new RuntimeException("ERROR: Expected a Value");
            if (!Int64.TryParse(obj.ToString(), out _value))
                throw new RuntimeException("ERROR: Not a number");
            return true;
        }

        private Int64 _value;

        public override Int64 Value
        {
            get => _value;
            set => _value = +value;
        }
    }
}