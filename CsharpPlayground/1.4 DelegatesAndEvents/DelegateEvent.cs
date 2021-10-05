using System;

namespace DelegateEvent
{
    public static class DelegateEvent
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

        //1. Declare the delegate and its inputs and output parameters.
        public delegate bool MultiplyEventHandler (object sender, MultiplyEventArgs e);

        //2. Declare the event supported by the delegate.
        //3. Assigning the method default delegate to avoid NULL if no one is subscribed. 
        private static event MultiplyEventHandler _onMultiplyDone = defaultOnMultiplyDoneHandler;

        //4. Create a method for the delegate.
        private static bool defaultOnMultiplyDoneHandler(object sender, MultiplyEventArgs e)
        {
            //In addition you will always receive true by default.
            return true;
        }

        //5. Optional. Customize addition and removal of subscribers.

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

        private static void RaiseMultiplyDone(int operation, string message)
        {
             //6. Raise the event with its custom args. Not need it to null check.
             var returnedValue = _onMultiplyDone(null, new MultiplyEventArgs(operation, message));

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
        
        public static void Multiply(int a, int b)
        {
            var operation = a * b;
            RaiseMultiplyDone(operation, "Multiply had been executed");
        }
    }

    public class Subscriber: IDisposable
    {
        public Subscriber()
        {
            Publisher.OnSumDone += OnSumDoneEventHandler;
            Publisher.OnSubtractionDone += OnSubtractionDoneEventHandler;

            //7. Subscribe to the event.
            Publisher.OnMultiplyDone += OnMultiplyDoneEventHandler;
        }
        
        private static void OnSumDoneEventHandler()
        {
            Console.WriteLine("Sum had been executed");
        }

        private static bool OnSubtractionDoneEventHandler(string text, int number)
        {
            //Make some logic with passed parameters
            //and return the result.
            Console.WriteLine($"Subtraction had been executed. Result is: {number}");
            
            return true;
        }

        private bool OnMultiplyDoneEventHandler(object sender, MultiplyEventArgs e)
        {
            //8. Manage the event.
            Console.WriteLine($"{e.Message}. Result is: {e.Value}");

            return true;
        }

        public void Dispose()
        {
            Publisher.OnSumDone -= OnSumDoneEventHandler;
            Publisher.OnSubtractionDone -= OnSubtractionDoneEventHandler;
            Publisher.OnMultiplyDone -= OnMultiplyDoneEventHandler;

            GC.SuppressFinalize(this);
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