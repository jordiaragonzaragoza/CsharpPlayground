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
            Publisher.Multiply(5, 5);

            Console.ReadLine();
        }
    }

    public static class Publisher
    {
        //An event is a encapsulated delegate
        public delegate void SumEventHandler();
        public static event SumEventHandler OnSumDone;
        
        public static void Sum(int a, int b)
        {
            var operation = a + b;
            if (OnSumDone != null)
            {
                Console.WriteLine($"Sum result is: {operation}");
                OnSumDone();
            }
            else
            {
                Console.WriteLine($"Not subscribed to the events");
            }

        }

        public delegate bool SubtractionEventHandler (string text, int number);
        public static event SubtractionEventHandler OnSubtractionDone;

        public static void Subtraction(int a, int b)
        {
            var operation = a - b;

            if (OnSubtractionDone != null)
            {
                var returnedValue = OnSubtractionDone("Subtraction had been executed", operation);
                Console.WriteLine($"And subscriber returned value is: {returnedValue}");
            }
            else
            {
                Console.WriteLine($"Not subscribed to the events");
            }
        }

        public delegate bool MultiplyEventHandler (object sender, MultiplyEventArgs e);
        //Assigning the default delegate to avoid null if no one is subscribed. In addition you will always receive true by default.
        private static event MultiplyEventHandler _onMultiplyDone = defaultOnMultiplyDoneHandler;
        private static bool defaultOnMultiplyDoneHandler(object sender, MultiplyEventArgs e)
        {
            return true;
        }

        //The event implementation uses a public field, you can still customize addition and removal of subscribers.
        //This is called a custom event accessor
        //It’s important to put a lock around adding and removing
        //subscribers to make sure that the operation is thread safe.
        public static event MultiplyEventHandler OnMultiplyDone
        {
            add
            {
                lock (_onMultiplyDone)
                {
                    _onMultiplyDone += value;
                }
            }

            remove
            {
                lock (_onMultiplyDone)
                {
                    _onMultiplyDone -= value;
                }
            }
        }
        
        public static void Multiply(int a, int b)
        {
            var operation = a * b;
            var returnedValue = _onMultiplyDone(null, new MultiplyEventArgs(operation, "Multiply had been executed"));

            if (_onMultiplyDone == defaultOnMultiplyDoneHandler)
            {
                //Default value
                Console.WriteLine($"Multiply done. But no subscribers.");
            }
            else
            {
                //Has subscribers.
                //The return value will be the last one that was subscribed.
                Console.WriteLine($"And last subscriber returned value is: {returnedValue}");
            }
        }
    }

    public class Subscriber: IDisposable
    {
        public Subscriber()
        {
            Publisher.OnSumDone += OnSumDoneHandler;
            Publisher.OnSubtractionDone += OnSubtractionDoneHandler;
            Publisher.OnMultiplyDone += OnMultiplyDoneHandler;
        }
        
        private static void OnSumDoneHandler()
        {
            Console.WriteLine("Sum had been executed");
        }

        private static bool OnSubtractionDoneHandler(string text, int number)
        {
            //Make some logic with passed parameters
            //and return the result.
            Console.WriteLine($"Subtraction had been executed. Result is: {number}");
            
            return true;
        }

        private bool OnMultiplyDoneHandler(object sender, MultiplyEventArgs e)
        {
            Console.WriteLine($"{e.Message}. Result is: {e.Value}");

            return true;
        }

        public void Dispose()
        {
            Publisher.OnSumDone -= OnSumDoneHandler;
            Publisher.OnSubtractionDone -= OnSubtractionDoneHandler;
            Publisher.OnMultiplyDone -= OnMultiplyDoneHandler;

            System.GC.SuppressFinalize(this);
        }
    }

    public class MultiplyEventArgs : EventArgs
    {
        public int Value { get; set; }

        public string Message { get; set; }
        public MultiplyEventArgs(int value, string message)
        {
            Value = value;
            Message = message;
        }
        
    }
}