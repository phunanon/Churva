using System;
using Churva.Interpreter.BluePrints.Attributes;
using Churva.Interpreter.BluePrints.Interfaces;

namespace Churva.Interpreter.Core.F
{
    [Keyword(Words = new []{"f32"})]
    public class F32 : ValueType<Single>, IKeyword
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

        public override Single Value { get; set; }
    }
}