namespace Generics
{
    using System.Collections.Generic;
    using System.Linq;

    public static class Generics
    {
        public static void Start()
        {
            var result = new OperationResult<Person>();
            var person = result.Content;

            var operationResultCar = new OperationResult<Car>();
            var car = new Car("Opel", "Vectra");
            operationResultCar.SetSuccesResponse(car);

            var content = operationResultCar.Content;

            var ej = new GenericsOnSimpleClass();
            GenericsOnSimpleClass.SimpleMethod();

            _ = GenericsOnSimpleClass.GenericsExampleMethod(car);
        }
    }

    public class OperationResult<T>
        where T : class, new()
    {
        public bool Success => !MessageList.Any();
        public List<string> MessageList { get; private set; }
        public T Content { get; set; }

        public OperationResult()
        {
            MessageList = new List<string>();
        }

        public void AddMessage(string message)
        {
            MessageList.Add(message);
        }

        public void SetSuccesResponse(T @object)
        {
            Content = @object;
        }
    }

    public class GenericsOnSimpleClass
    {
        public static T GenericsExampleMethod<T>(T x)
        {
            return x;
        }

        public static void SimpleMethod()
        {
        }
    }

    public interface IVehicle
    {
        string Brand { get; set; }
        string Model { get; set; }
    }

    public class Car : IVehicle
    {
        public string Brand { get; set; }
        public string Model { get; set; }
        public Car()
            : this (string.Empty, string.Empty)
        {
        }

        public Car(string brand, string model)
        {
            Brand = brand;
            Model = model;
        }
    }

    public class Person
    {
        public string Name { get; set; }
        public string SubName { get; set; }

        public Person()
            : this (string.Empty, string.Empty)
        {
        }

        public Person(string name, string subName)
        {
            Name = name;
            SubName = subName;
        }
    }
}
