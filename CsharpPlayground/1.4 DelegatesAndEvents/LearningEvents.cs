using System;

namespace LearningEvents
{
    public static class LearningEvents
    {
        public static void Start()
        {
            var subscriber = new Subscriber();

            Publisher.Sum(5,6);
            Publisher.Subtraction(1,1);

            Console.ReadLine();
        }
    }

    public static class Publisher
    {
        //An event is a encapsulated delegate
        public delegate void SumEventHandler();
        public static event SumEventHandler OnSumExecuted;
        
        public static void Sum(int a, int b)
        {
            if (OnSumExecuted != null)
            {
                Console.WriteLine($"Sum result is: {a + b}");
                OnSumExecuted();
            }
            else
            {
                Console.WriteLine($"Not subscribed to the events");
            }

        }

        public delegate bool SubtractionEventHandler (string text, int number);
        public static event SubtractionEventHandler OnSubtractionExecuted;

        public static void Subtraction(int a, int b)
        {
            if (OnSubtractionExecuted != null)
            {
                var operation = a - b;
                var returnedValue = OnSubtractionExecuted("Subtraction had been executed", operation);
                Console.WriteLine($"And returned value is: {returnedValue}");
            }
            else
            {
                Console.WriteLine($"Not subscribed to the events");
            }
        }
    }

    public class Subscriber: IDisposable
    {
        public Subscriber()
        {
            Publisher.OnSumExecuted += OnSumExecutedHandler;
            Publisher.OnSubtractionExecuted += OnSubtractionExecutedHandler;
        }
        
        private static void OnSumExecutedHandler()
        {
            Console.WriteLine("Sum had been executed");
        }

        private static bool OnSubtractionExecutedHandler(string text, int number)
        {
            Console.WriteLine($"Subtraction had been executed. Result is: {number}");

            return true;
        }

        public void Dispose()
        {
            Publisher.OnSumExecuted -= OnSumExecutedHandler;
            Publisher.OnSubtractionExecuted -= OnSubtractionExecutedHandler;
            System.GC.SuppressFinalize(this);
        }
    }
}