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

        public static readonly Dictionary<string,SingleOperator> SingleOperators = new Dictionary<String, SingleOperator>
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
        public static readonly Dictionary<string, MultiOperator> MultiOperators = new Dictionary<string, MultiOperator>
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
                throw new RuntimeException("ERROR: There is already an instance with this name");
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

        public static String[] ParseValueInstance(IValueType ivt, IEnumerable<String> strings)
        {
            var stringsList = strings.ToList();
            if (!stringsList.Any())
                throw new RuntimeException("Error: Internal failure.");
            String[] returns = new String[2];
            var str = stringsList.FirstOrDefault();
            var words = ivt.GetType().GetAttributeValue((KeywordAttribute itm) => itm.Words);
            var word = words.FirstOrDefault();
            var reg = str.IndexOf(word, StringComparison.InvariantCulture);
            str = str.Remove(reg, word.Length);
            if (str.Contains('='))
            {
                try
                {
                    var split = str.Split('=', 2);
                    var inst = split[0].Trim();
                    CheckValidName(inst, returns);
                    var val = split[1].Trim();
                    val = CheckValForOtherInst(val, word);
                    
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
                var instanceName = str.Trim();
                if (!instanceName.ValidNewInstanceName())
                    throw new RuntimeException($"ERROR: {instanceName} is an invalid instance name");
                returns[0] = instanceName;
            }
            return returns;
        }

        private static String CheckValForOtherInst(String val, String word)
        {
            //if (!val.Contains('=')) return val;
            
            var inner = val.Split('=', 2);
            var innerInst = inner[0].Trim();
            
            if (Instances.Any(itm => itm.InstanceName == innerInst))
            {
                val = GetOtherInstance(innerInst);
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

        private static String GetOtherInstance(String innerInst)
        {
            String val = null;
            var @ref = Instances.FirstOrDefault(itm => itm.InstanceName == innerInst);
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