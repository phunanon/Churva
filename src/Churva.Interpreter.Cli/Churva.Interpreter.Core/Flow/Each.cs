using System;
using Churva.Interpreter.BluePrints.Attributes;
using Churva.Interpreter.BluePrints.Interfaces;

namespace Churva.Interpreter.Core.Flow
{
    [Keyword(Words = new []{"each",",",":","skip","finish"})]
    public class Each: FlowControl, IKeyword
    {
        public String InstanceName { get; set; }
        public Boolean Validate(String[] strings)
        {
            throw new NotImplementedException();
        }
    }
}