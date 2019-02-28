using System;
using Churva.Interpreter.BluePrints;
using Churva.Interpreter.BluePrints.Attributes;
using Churva.Interpreter.BluePrints.Interfaces;

namespace Churva.Interpreter.Core.I
{
    [Keyword(Words=new []{"i08"})]
    public class I08: ValueType<Int16>, IKeyword
    {
        //There is no int8 in c#
        public String InstanceName { get; set; }

        public override Boolean SetValueByObject(Object obj)
        {
            throw new NotImplementedException();
        }

        public override Boolean Validate(String[] strings)
        {
            throw new NotImplementedException();
        }

        private Int16 _value;

        /// <summary>
        /// Gets or sets the value of an 8 bit number;
        /// </summary>
        /// <exception cref="RuntimeException"></exception>
        public override Int16 Value
        {
            get => _value;
            set
            {
                if (value < -128 || value > 127)
                    throw new RuntimeException($"Value is out of the max range for {nameof(I08)}");
                _value = value;
            }
        }
    }
}