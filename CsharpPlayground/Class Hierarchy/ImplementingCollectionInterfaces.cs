using System;
using System.Collections;
using System.Collections.Generic;

namespace ImplementingCollectionInterfaces
{
    public static class ImplementingCollectionInterfaces
    {
        public static void Start()
        {
            var orders = new List<Order>
            {
                new Order { Id = 1, Created = new DateTime(2012, 12, 1 )},
                new Order { Id = 2, Created = new DateTime(2012, 1, 6 )},
                new Order { Id = 3, Created = new DateTime(2012, 7, 8 )},
                new Order { Id = 4, Created = new DateTime(2012, 2, 20 )},
            };
            orders.Sort();

            Console.WriteLine("Orders created sorted by date.");
            foreach (var order in orders)
            {
                Console.WriteLine($"Id: {order.Id}, Date: {order.Created}");
            }

            Console.WriteLine("Example using GetEnumerator for List<T>.");
            SugarForeachStatement();

            Console.ReadLine();
        }
        
        // How works a foreach statement.
        private static void SugarForeachStatement()
        {
            var numbers = new List<int> { 1, 2, 3, 5, 7, 9 };
            using (List<int>.Enumerator enumerator = numbers.GetEnumerator())
            {
                while (enumerator.MoveNext())
                {
                    Console.WriteLine(enumerator.Current);
                }
            }
        }
    }

    public class Order : IComparable
    {
        public int Id { get; set; }

        public DateTime Created { get; set; }

        public int CompareTo(object @object)
        {
            if (@object == null)
            {
                return 1;
            }

            if (@object is not Order order)
            {
                throw new ArgumentException("Object is not an Order");
            }

            return this.Created.CompareTo(order.Created); //Calls to DateTime CompareTo()
        }
    }

    public class Person
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }

        public Person(string firstName, string lastName)
        {
            FirstName = firstName;
            LastName = lastName;
        }

        public override string ToString()
        {
            return FirstName + " " + LastName;
        }
    }

    public class People : IEnumerable<Person>
    {
        private Person[] people;

        public People(Person[] people)
        {
            this.people = people;
        }
        
        public IEnumerator<Person> GetEnumerator()
        {
            for (var index = 0; index < people.Length; index++)
            {
                yield return people[index];
            }
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}
