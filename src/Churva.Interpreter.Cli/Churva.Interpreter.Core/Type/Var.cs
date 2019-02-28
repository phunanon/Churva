using System;
using Churva.Interpreter.BluePrints;
using Churva.Interpreter.BluePrints.Attributes;
using Churva.Interpreter.BluePrints.Interfaces;

namespace Churva.Interpreter.Core.Type
{
    [Keyword(Words = new[] {"var"})]
    public class Var : ValueType<Object>, IKeyword
    {
        public String InstanceName { get; set; }

        public override Boolean SetValueByObject(Object obj)
        {
            Value = obj;
            return true;
        }

        public System.Type Type { get; set; } = null;

        private Object _value;

        public override Object Value
        {
            get => _value;
            set
            {
                Type = value.GetType();
                try
                {
                    ReflectionHelper.CreateGenericInstance(typeof(ValueType<>), Type, value);
                }
                catch (Exception e)
                {
                    throw new RuntimeException("ERROR: Couldn't determine type");
                }

                _value = value;
            }
        }
    }
}