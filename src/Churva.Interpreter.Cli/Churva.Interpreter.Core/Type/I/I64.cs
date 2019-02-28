using System;
using System.Linq;
using Churva.Interpreter.BluePrints;
using Churva.Interpreter.BluePrints.Attributes;
using Churva.Interpreter.BluePrints.Interfaces;
using static Churva.Interpreter.BluePrints.ChurvaEnvironment;

namespace Churva.Interpreter.Core.I
{
    [Keyword(Words = new[] {"i64"})]
    public class I64 : ValueType<Int64>, IKeyword
    {
        public String InstanceName { get; set; }

        public override Boolean SetValueByObject(Object obj)
        {
            if (String.IsNullOrWhiteSpace(obj.ToString()))
                throw new RuntimeException("ERROR: Expected a Value");
            if (!Int64.TryParse(obj.ToString(), out _value))
                throw new RuntimeException("ERROR: Not a number");
            return true;
        }

        public override Boolean Validate(String[] strings)
        {
            if (!strings.Any()) return false;
            var ret = ParseValueInstance(this, strings);
            InstanceName = ret[0];
            if (SetValueByObject(ret[1]))
                Console.WriteLine($"{InstanceName}={Value.ToString()}");
            Instances.Add(this);
            return true;
        }

        private Int64 _value;

        public override Int64 Value
        {
            get => _value;
            set => _value = +value;
        }
    }
}