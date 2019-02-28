using System;
using System.Runtime.CompilerServices;

namespace Churva.Interpreter.BluePrints.Interfaces
{
    public abstract class ValueType<T>: IValueType<T>
    {
        public ValueType()
        {
        }

        public ValueType(String instanceName, T value)
        {
            InstanceName = instanceName;
            Value = value;
        }

        Object IValueType.Value
        {
            get => Value;
            set
            {
                if (value is T val)
                    Value = val;
            }
        }

        public abstract Boolean SetValueByObject(Object obj);

        public virtual T Value { get; set; }
        public virtual String InstanceName { get; set; }
        public abstract Boolean Validate(String[] strings);
    }
}