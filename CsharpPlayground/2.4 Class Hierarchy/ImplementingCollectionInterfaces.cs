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
                new Order { Created = new DateTime(2012, 12, 1 )},
                new Order { Created = new DateTime(2012, 1, 6 )},
                new Order { Created = new DateTime(2012, 7, 8 )},
                new Order { Created = new DateTime(2012, 2, 20 )},
            };
            orders.Sort();
        }
        
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
        public DateTime Created { get; set; }
        public int CompareTo(object obj)
        {
            if (obj == null)
            {
                return 1;
            }

            if (!(obj is Order o))
            {
                throw new ArgumentException("Object is not an Order");
            }

            return Created.CompareTo(o.Created); //Calls to DateTime CompareTo()
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
