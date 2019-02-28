using System;
using Churva.Interpreter.BluePrints.Attributes;
using Churva.Interpreter.BluePrints.Interfaces;

namespace Churva.Interpreter.Core.F
{
    [Keyword(Words = new []{"f64"})]
    public class F64: ValueType<Decimal>, IKeyword
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

        public override Decimal Value { get; set; }
    }
}