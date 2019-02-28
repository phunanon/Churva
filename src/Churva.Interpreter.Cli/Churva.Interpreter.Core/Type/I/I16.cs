using System;
using System.Linq;
using Churva.Interpreter.BluePrints;
using Churva.Interpreter.BluePrints.Attributes;
using Churva.Interpreter.BluePrints.Interfaces;

namespace Churva.Interpreter.Core.Type.I
{
    [Keyword(Words = new[] {"i16"})]
    public class I16 : ValueType<Int16>
    {
        public override Boolean SetValueByObject(Object obj)
        {
            if (String.IsNullOrWhiteSpace(obj.ToString()))
                throw new RuntimeException("ERROR: Expected a Value");
            if (!Int16.TryParse(obj.ToString(), out _value))
                throw new RuntimeException("ERROR: Not a number");
            return true;
        }

        private Int16 _value;

        public override Int16 Value
        {
            get => _value;
            set => _value = value;
        }
    }
}