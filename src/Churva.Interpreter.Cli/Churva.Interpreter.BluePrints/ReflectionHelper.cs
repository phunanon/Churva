using System;
using System.Linq;
using System.Reflection;
using Churva.Interpreter.BluePrints.Interfaces;

namespace Churva.Interpreter.BluePrints
{
    public static class ReflectionHelper
    {
        public static T GetPropertyValue<T>(this Object obj, String propertyName)
        {
            var objType = obj.GetType();
            return (T) objType.GetProperty(propertyName).GetValue(obj, null);
        }

        public static void SetPropertyValue<T>(this Object obj, String propertyName, T value)
        {
            var objType = obj.GetType();
            objType.GetProperty(propertyName).SetValue(obj, value);
        }

        public static T InvokeReturnMethod<T>(this Object obj, String methodName, params Object[] paras)
        {
            var objType = obj.GetType();
            var method = objType.GetMethod(methodName);
            var ret = method.Invoke(obj, paras);
            if (ret is T returnVal)
                return returnVal;
            return default;
        }

        public static void InvokeMethod(this Object obj, String methodName, params Object[] paras)
        {
            var objType = obj.GetType();
            var method = objType.GetMethod(methodName);
            var ret = method.Invoke(obj, paras);
        }

        public static Object CreateGenericInstance(Type type, Type genericParam, params Object[] paras)
        {
            type = type.MakeGenericType(type, genericParam);
            return Activator.CreateInstance(type, paras);
        }

        public static TValue GetAttributeValue<TAttribute, TValue>(
            this Type type,
            Func<TAttribute, TValue> valueSelector)
            where TAttribute : Attribute
        {
            var attribs = type.GetCustomAttributes(typeof(TAttribute), true);
            if (attribs.FirstOrDefault() is TAttribute att)
            {
                return valueSelector(att);
            }

            return default;
        }
    }
}