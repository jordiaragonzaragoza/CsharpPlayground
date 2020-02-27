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


        public delegate void SubtractionEventHandler();
        public static event SubtractionEventHandler OnSubtractionExecuted;

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

        public static void Subtraction(int a, int b)
        {
            if (OnSubtractionExecuted != null)
            {
                Console.WriteLine($"Subtraction result is: {a - b}");
                OnSubtractionExecuted();
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

        private static void OnSubtractionExecutedHandler()
        {
            Console.WriteLine("Subtraction had been executed");
        }

        private static void OnSumExecutedHandler()
        {
            Console.WriteLine("Sum had been executed");
        }

        public void Dispose()
        {
            Publisher.OnSumExecuted -= OnSumExecutedHandler;
            Publisher.OnSubtractionExecuted -= OnSubtractionExecutedHandler;
            System.GC.SuppressFinalize(this);
        }
    }
}