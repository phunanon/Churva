using System;
using Churva.Interpreter.BluePrints.Attributes;
using Churva.Interpreter.BluePrints.Interfaces;

namespace Churva.Interpreter.Core.U
{
    [Keyword(Words = new []{"u16"})]
    public class U16:ValueType<UInt16>,IKeyword
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

        public override UInt16 Value { get; set; }
    }
}