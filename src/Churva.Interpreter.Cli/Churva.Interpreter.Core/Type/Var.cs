using System;
using Churva.Interpreter.BluePrints;
using Churva.Interpreter.BluePrints.Attributes;
using Churva.Interpreter.BluePrints.Interfaces;
using static Churva.Interpreter.BluePrints.ReflectionHelper;

namespace Churva.Interpreter.Core
{
    [Keyword(Words = new []{"var"})]
    public class Var : ValueType<Object>, IKeyword
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

        public Type Type { get; set; } = null;

        private Object _value;

        public override Object Value
        {
            get => _value;
            set
            {
                if (value is IValueType val)
                {
                    Type = value.GetType().GetGenericArguments()[0];
                    try
                    {
                        CreateGenericInstance(typeof(ValueType<>), Type, val.Value);
                    }
                    catch (Exception)
                    {
                        throw new RuntimeException("Couldn't determine type");
                    }

                    _value = val.Value;
                }
                else
                {
                    throw new RuntimeException("Expected a value type.");
                }
            }
        }
    }
}