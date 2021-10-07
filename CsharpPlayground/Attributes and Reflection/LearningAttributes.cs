namespace LearningAttributes
{
    using System;
    using System.Diagnostics;
    using System.Linq;

    public static class LearningAttributes
    {
        public static void Start()
        {
            CheckAttributeDefined(typeof(Person), typeof(SerializableAttribute));
            CheckAttributeDefined(typeof(Person), typeof(CustomAttribute));
            SearchCustomAttributesOnTypeMembers(typeof(Person), typeof(ConditionalAttribute));
            SearchCustomAttributesOnTypeMembers(typeof(Person), typeof(CustomAttribute));

            Console.ReadLine();
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

        public static void SearchCustomAttributesOnTypeMembers(Type element, Type attributeType)
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
                        Console.WriteLine($"Class {element.Name} has {attributeType.Name} with value: {condition} on member: {member.Name}");
                    }

                    if (attribute is CustomAttribute customAttribute)
                    {
                        var description = customAttribute.Description;
                        Console.WriteLine($"Class {element.Name} has {attributeType.Name} with value: {description} on member: {member.Name}");
                    }
                }
            }
        }

        //Getting the value of a field through reflection

    }

    [Serializable]
    [Custom("Hello")]
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
