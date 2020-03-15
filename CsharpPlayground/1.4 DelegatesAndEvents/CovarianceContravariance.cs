using System;
using System.Collections.Generic;
using System.Text;

namespace CovarianceContravariance
{
    public static class CovarianceContravariance
    {
        public static void Start()
        {
            var persons = new List<Person>()
            {
                new Scholar(),
                new Boss()
            };

            PrintPersonInvariance(persons);

            var scholars = new List<Scholar>()
            {
                new Scholar(),
                new Scholar()
            };

            //PrintPersonInvariance(scholars); // Not allowed.
            PrintPersonCovariance(scholars);

            var actionScholar = new Action<Scholar>(z => Console.WriteLine("Make coffee"));
            MakeActionScholarContravariance(actionScholar);

            var actionBoss = new Action<Boss>(z => Console.WriteLine("Set a meeting"));
            //MakeActionScholarContravariance(actionBoss); // Not allowed.

            var actionEmployee = new Action<Employee>(z => Console.WriteLine("Hard work"));
            MakeActionScholarContravariance(actionEmployee);

            var actionPerson = new Action<Person>(z => Console.WriteLine("Go to work"));
            MakeActionScholarContravariance(actionPerson);
        }

        // Allows only of the specified type. 'in' as reserved keyword. List<in T>
        public static void PrintPersonInvariance(List<Person> persons)
        {
            foreach (var person in persons)
            {
                Console.WriteLine($"Actual element type is: {person.GetType()}");
            }
        }

        // It allows a type or those that inherit from it. 'out' as reserved keyword. IEnumerable<out T>
        // Example: List<Person>, List<Employee>, List<Boss> , List<Scholar>
        public static void PrintPersonCovariance(IEnumerable<Person> persons)
        {
            foreach (var person in persons)
            {
                Console.WriteLine($"Actual element type is: {person.GetType()}");
            }
        }

        // Allows a type, or types that implement, or parent types. 'in' as reserved keyword. Action<in T>
        public static void MakeActionScholarContravariance(Action<Scholar> scholarAction)
        {
            var scholar = new Scholar();
            scholarAction(scholar);
        }
    }

    public class Person
    {
        public string Name { get; set; }
    }

    public class Employee : Person
    {
        public int Id { get; set; }
    }

    public class Boss : Employee
    {
    }

    public class Scholar : Employee
    {
    }
}
