using System;
using Churva.Interpreter.BluePrints.Attributes;
using Churva.Interpreter.BluePrints.Interfaces;

namespace Churva.Interpreter.Core
{
    [Keyword(Words = new[]{"sub"})]
    public class Sub : IFlowControl, IKeyword
    {
        public String InstanceName { get; set; }

        public Boolean Validate(String[] strings)
        {
            throw new System.NotImplementedException();
        }
    }
}