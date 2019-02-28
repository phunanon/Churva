using System;

namespace Churva.Interpreter.BluePrints.Interfaces
{
    public interface IValueType: IKeyword
    {
        Object Value { get; set; }
        Boolean SetValueByObject(Object obj);
        Boolean Validate(String[] strings);
    }
    
    public interface IValueType<T>: IValueType
    {
        T Value { get; set; }
    }
}