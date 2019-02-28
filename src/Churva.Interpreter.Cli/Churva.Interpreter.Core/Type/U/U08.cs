using System;
using Churva.Interpreter.BluePrints;
using Churva.Interpreter.BluePrints.Attributes;
using Churva.Interpreter.BluePrints.Interfaces;

namespace Churva.Interpreter.Core.U
{
    [Keyword(Words = new []{"u08"})]
    public class U08 : ValueType<UInt16>, IKeyword
    {
        private UInt16 _value;
        public String InstanceName { get; set; }

        public override Boolean SetValueByObject(Object obj)
        {
            throw new NotImplementedException();
        }

        public override Boolean Validate(String[] strings)
        {
            Console.WriteLine("Wow, you made it");
            foreach (var t in strings)
                Console.WriteLine(t);

            return true;
        }

        public override UInt16 Value
        {
            get => _value;
            set
            {
                if (value > 255)
                    throw new RuntimeException($"Value is out of the max range for {nameof(U08)}");
                _value = value;
            }
        }
    }
}