using System;

namespace Churva.Interpreter.BluePrints.Interfaces
{
    public interface IKeyword
    {
        String InstanceName { get; set; }
        Boolean Validate(String[] strings);
    }
}