using System;
using System.Linq;
using System.Runtime.CompilerServices;
using static Churva.Interpreter.BluePrints.ChurvaEnvironment;

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

        public virtual Boolean Validate(String[] strings)
        {
            if (!strings.Any()) return false;
            var ret = ParseValueInstance(this, strings);
            InstanceName = ret[0];
            if (SetValueByObject(ret[1]))
                Console.WriteLine($"{InstanceName}={Value.ToString()}");
            Instances.Add(this);
            return true;
        }
    }
}