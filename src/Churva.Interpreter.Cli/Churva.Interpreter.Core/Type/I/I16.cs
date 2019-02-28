using System;
using Churva.Interpreter.BluePrints.Attributes;
using Churva.Interpreter.BluePrints.Interfaces;

namespace Churva.Interpreter.Core.I
{
    [Keyword(Words = new []{"i16"})]
    public class I16: ValueType<Int16>, IKeyword
    {
        public String InstanceName { get; set; }

        public override Boolean SetValueByObject(Object obj)
        {
            throw new NotImplementedException();
        }

        public override Boolean Validate(String[] strings)
        {
            throw new NotImplementedException();
        }

        public override Int16 Value { get; set; }
    }
}