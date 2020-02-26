using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;

namespace LearningAttributes
{
    public static class LearningAttributes
    {
        public static void Start()
        {
            CheckAttributeDefined(typeof(Person), typeof(SerializableAttribute));
            SearchConditionStringOnConditionalAttribute(typeof(Person), typeof(ConditionalAttribute));
            ReflectionToExecuteAMethod();

            Console.ReadLine();
        }

        private static void ReflectionToExecuteAMethod()
        {
            var i = 42;
            var compareToMethod = i.GetType().GetMethod("CompareTo",new Type[] { typeof(int) });

            var result = (int)compareToMethod.Invoke(i, new object[] { 41 });
        }

        public static bool CheckAttributeDefined(Type element, Type attributeType)
        {
            if (Attribute.IsDefined(element, attributeType))
            {
                Console.WriteLine($"Class {element.Name} has {attributeType.Name}");
                return true;
            }

            return false;
        }

        public static void SearchConditionStringOnConditionalAttribute(Type element, Type attributeType)
        {
            var members = element.GetMembers();

            foreach (var member in members)
            {
                var conditionalAttributes = Attribute.GetCustomAttributes(member, attributeType).ToList();
                foreach (var attribute in conditionalAttributes)
                {
                    if (attribute is ConditionalAttribute conditionalAttribute)
                    {
                        var condition = conditionalAttribute.ConditionString;
                        Console.WriteLine($"Class {element.Name} has {attributeType.Name} with value: {condition}");
                    }

                    else
                    {
                        Console.WriteLine($"Class {element.Name} has {attributeType.Name}");
                    }
                }
            }
        }

        //Getting the value of a field through reflection

    }

    [Serializable]
    public class Person
    {
        [Conditional("CONDITION1"), Conditional("CONDITION2")]
        public static void MethodOne()
        {

        }

        [Custom("Description")]
        public static void MethodTwo()
        {  
        }

    }

    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, Inherited = false, AllowMultiple = true)]
    public class CustomAttribute : Attribute
    {
        public CustomAttribute(string description)
        {
            Description = description;
        }
        public string Description { get; set; }
    }
}
