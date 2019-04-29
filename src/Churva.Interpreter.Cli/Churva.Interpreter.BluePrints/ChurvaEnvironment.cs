using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using Churva.Interpreter.BluePrints.Attributes;
using Churva.Interpreter.BluePrints.Interfaces;

namespace Churva.Interpreter.BluePrints
{
    public static class ChurvaEnvironment
    {
        public static Dictionary<String[], Type> Keywords { get; set; } = new Dictionary<String[], Type>();

        public static readonly Dictionary<String,SingleOperator> SingleOperators = new Dictionary<String, SingleOperator>
        {
            {"!", SingleOperator.LogicalNegate},
            {"~",  SingleOperator.BitwiseNegate},
            {"+", SingleOperator.IntegerPromotion},
            {"-", SingleOperator.AdditiveInverse},
            {"++", SingleOperator.Increment},
            {"--", SingleOperator.Decrement},
            {"*", SingleOperator.PointerDereference},
            {"&", SingleOperator.VariableAddress},
            {"[", SingleOperator.ArrayAccess}
        };
        
        public static readonly Dictionary<String, MultiOperator> MultiOperators = new Dictionary<String, MultiOperator>
        {
            {"=", MultiOperator.VariableAssignment},
            {"+", MultiOperator.ArithmeticalAddition},
            {"-", MultiOperator.ArithmeticalSubtraction},
            {"*", MultiOperator.ArithmeticalMultiplication},
            {"/", MultiOperator.ArithmeticalDivision},
            {"%", MultiOperator.ArithmeticalModulo},
            {"&", MultiOperator.BitwiseAnd},
            {"|", MultiOperator.BitwiseOr},
            {"^", MultiOperator.BitwiseXor},
            {">>", MultiOperator.BitwiseRightShift},
            {"<<", MultiOperator.BitwiseLeftShift},
            {"||", MultiOperator.Or},
            {"&&", MultiOperator.And},
            {"<", MultiOperator.LessThan},
            {">", MultiOperator.GreaterThan},
            {"==", MultiOperator.EqualTo},
            {"<=", MultiOperator.LessThanOrEqualTo},
            {">=", MultiOperator.GreaterThanOrEqualTo}
        };

        public static List<IKeyword> Instances { get; set; } = new List<IKeyword>();

        public static Boolean ValidNewInstanceName(this String name)
        {
            if(IsInstance(name))
                throw new RuntimeException($"ERROR: There is already an instance with the name '{name}'");
            if (name.Contains(' '))
                throw new RuntimeException("ERROR: Instance name cannot contain a space");
            foreach (var key in SingleOperators.Keys)
                if (name.Contains(key)) return false;

            foreach (var key in MultiOperators.Keys)
                if (name.Contains(key)) return false;
            foreach (var key in Keywords.Keys)
            {
                foreach (var str in key)
                {
                    if (name.Equals(str))
                        throw new RuntimeException($"ERROR: {str} is a reserved keyword.");
                }
            }

            return true;
        }

        public static Boolean IsInstance(String name)
        {
            return Instances.Any(itm => itm.InstanceName == name);
        }

        public static Boolean GetInstance(String instanceName, out IValueType type)
        {
            foreach (var instance in Instances)
            {
                if (!instanceName.Equals(instance.InstanceName)) continue;
                if (!(instance is IValueType iv)) continue;
                type = iv;
                return true;
            }

            type = null;
            return false;
        }

        private static string GetFirstKeyword(IValueType valueType)
        {
            var usedKeywords = valueType.GetType().GetAttributeValue((KeywordAttribute itm) => itm.Words);
            var firstKeyword = usedKeywords.FirstOrDefault();
            return firstKeyword;
        }

        public static String[] ValidateValueInstance(IValueType valueType, IEnumerable<String> instructions)
        {
            var firstInstruction = GetFirstInstruction(instructions);
            var firstKeyword = GetFirstKeyword(valueType);
            var reg = firstInstruction.IndexOf(firstKeyword, StringComparison.InvariantCulture);
            firstInstruction = firstInstruction.Remove(reg, firstKeyword.Length);
            if(String.IsNullOrWhiteSpace(firstInstruction))
                throw new RuntimeException("ERROR: Expected an instance name");
            return ExtractVariableInstruction(firstInstruction, firstKeyword);
        }

        private static String[] ExtractVariableInstruction(String firstInstruction, String firstKeyword)
        {
            var returns = new String[2];
            if (firstInstruction.Contains('='))
            {
                try
                {
                    var split = firstInstruction.Split('=', 2);
                    var inst = split[0].Trim();
                    CheckValidName(inst, returns);
                    var val = split[1].Trim();
                    val = ValidateAndAssignFromInstance(val, firstKeyword);

                    if (String.IsNullOrWhiteSpace(val))
                        throw new RuntimeException("ERROR: Expected Value");
                    returns[1] = val;
                }
                catch (RuntimeException)
                {
                    throw;
                }
                catch (Exception)
                {
                    throw new RuntimeException("ERROR: Expected Value");
                }
            }
            else
            {
                var instanceName = firstInstruction.Trim();
                if (!instanceName.ValidNewInstanceName())
                    throw new RuntimeException($"ERROR: {instanceName} is an invalid instance name");
                returns[0] = instanceName;
            }

            return returns;
        }

        private static String GetFirstInstruction(IEnumerable<String> instructions)
        {
            var iteratedInstructions = instructions.ToList();
            if (!iteratedInstructions.Any())
                throw new RuntimeException("Error: Internal failure.");
            var str = iteratedInstructions.FirstOrDefault();
            return str;
        }

        private static String ValidateAndAssignFromInstance(String val, String word)
        {
            var inner = val.Split('=', 2);
            var innerInst = inner[0].Trim();
            
            if (Instances.Any(itm => itm.InstanceName == innerInst))
            {
                val = GetInstanceValue(innerInst);
            }
            else if (innerInst.StartsWith(word))
            {
                Type kw = null;
                foreach (var keywordsKey in Keywords.Keys)
                {
                    if (keywordsKey.Any(s => s == word))
                    {
                        kw = Keywords[keywordsKey];
                    }

                    if (kw != null)
                        break;
                }

                if (kw == null)
                {
                    throw new RuntimeException("ERROR: Couldn't determine the type of");
                }

                var newInstance = Activator.CreateInstance(kw);
                if (!(newInstance is IValueType ivfNew)) return val;
                ivfNew.Validate(new[] {val});
                val = ivfNew.Value.ToString();
            }

            return val;
        }

        private static String GetInstanceValue(String instance)
        {
            String val = null;
            var @ref = Instances.FirstOrDefault(itm => itm.InstanceName == instance);
            if (@ref is IValueType refType)
            {
                val = refType.Value.ToString();
            }

            return val;
        }

        private static void CheckValidName(String inst, String[] returnStrs)
        {
            if (!inst.ValidNewInstanceName())
                throw new RuntimeException("ERROR: Invalid Instance Name");
            returnStrs[0] = inst;
        }
    }
}