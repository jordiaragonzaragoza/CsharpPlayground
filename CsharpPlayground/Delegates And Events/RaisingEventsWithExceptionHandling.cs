namespace RaisingEventsWithExceptionHandling
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using CustomEventArguments;

    public static class RaisingEventsWithExceptionHandling
    {
        public static void Start()
        {
            var publisher = new Publisher();
            var subscriber = new Subscriber(publisher);
            
            try
            {
                publisher.MethodWithRaiseEvents();
            }
            catch (AggregateException ex)
            {
                Console.WriteLine($"Number of catches Exceptions: {ex.InnerExceptions.Count}");
            }

            Console.ReadLine();
        }
    }

    public class Publisher 
    {
        private event EventHandler<CustomEventArgs> _onChange = delegate { };

        public event EventHandler<CustomEventArgs> OnChange
        {
            add
            {
                lock (_onChange)
                {
                    _onChange += value;
                }
            }
            remove
            {
                lock (_onChange)
                {
                    _onChange -= value;
                }
            }
        }

        public void MethodWithRaiseEvents()
        {
            //Do some logic...
            //...
            var exceptions = new List<Exception>();
            foreach (Delegate handler in _onChange.GetInvocationList())
            {
                try
                {
                    //Invoke events.
                    handler.DynamicInvoke(this, new CustomEventArgs ("42"));
                }
                catch (Exception ex)
                {
                    exceptions.Add(ex);
                }
            }

            if (exceptions.Any())
            {
                throw new AggregateException(exceptions);
            }
        }
    }

    public class Subscriber : IDisposable
    {
        private readonly Publisher _publisher;
        public Subscriber(Publisher publisher)
        {
            _publisher = publisher;
            _publisher.OnChange += OnChangeFistHandler;
            _publisher.OnChange += OnChangeSecondHandler;
            _publisher.OnChange += OnChangeExceptionHandler;
        }

        private static void OnChangeFistHandler(object sender, CustomEventArgs  e)
        {
            Console.WriteLine("Subscriber 1 called. Event value: {0}", e.Message);
        }

        private static void OnChangeSecondHandler(object sender, CustomEventArgs  e)
        {
            Console.WriteLine("Subscriber 3 called. Event value: {0}", e.Message);
        }
        private static void OnChangeExceptionHandler(object sender, CustomEventArgs e)
        {
            throw new Exception();
        }

        public void Dispose()
        {
            _publisher.OnChange -= OnChangeFistHandler;
            _publisher.OnChange -= OnChangeExceptionHandler;
            _publisher.OnChange -= OnChangeSecondHandler;

            System.GC.SuppressFinalize(this);
        }
    }
}
