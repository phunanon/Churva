using System;
using Churva.Interpreter.BluePrints.Attributes;
using Churva.Interpreter.BluePrints.Interfaces;

namespace Churva.Interpreter.Core.U
{
    [Keyword(Words = new []{"u32"})]
    public class U32:ValueType<UInt32>,IKeyword
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
        public override UInt32 Value { get; set; }
    }
}