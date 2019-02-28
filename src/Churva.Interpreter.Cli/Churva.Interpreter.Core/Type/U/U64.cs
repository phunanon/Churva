using System;
using Churva.Interpreter.BluePrints.Attributes;
using Churva.Interpreter.BluePrints.Interfaces;

namespace Churva.Interpreter.Core.U
{
    [Keyword(Words = new []{"u64"})]
    public class U64:ValueType<UInt64>,IKeyword
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

        public override UInt64 Value { get; set; }
    }
}