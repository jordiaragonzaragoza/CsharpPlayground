namespace LambdaSimpleFunc
{

    using System;
    using System.Collections.Generic;
    using System.Linq;

    public static class LambdaSimpleFunc
    {
        //Func:      works as delegate but with a required output of any type.
        private static readonly Func<int, bool> AdultsFunc = value => value >= 18;

        public static void Start()
        {
            var persons = new List<Person>() {
                new Person() { Age = 14, Name = "Javier" },
                new Person() { Age = 18, Name = "Ivan" },
                new Person() { Age = 25, Name = "Manuel" },
                new Person() { Age = 7, Name = "Lucas" }
            };

            // Where is an extension method for Enumerable
            // Double Lambda expression
            var adults = persons.Where(p => AdultsFunc(p.Age)).ToList();
            
            Console.WriteLine($"We have {adults.Count} adults on our collection");

            Console.ReadLine();
        }
    }

    public class Person
    {
        public string Name { get; set; }
        public int Age { get; set; }
    }
}

