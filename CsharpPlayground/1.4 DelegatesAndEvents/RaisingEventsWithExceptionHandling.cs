using System;
using System.Collections.Generic;
using System.Linq;
using CustomEventArguments;

namespace RaisingEventsWithExceptionHandlingSubscriber
{
    public static class RaisingEventsWithExceptionHandling
    {
        public static void Start()
        {
            var publisher = new PublisherRaisingEventsWithExceptionHandling();
            var subscriber = new SubscriberRaisingEventsWithExceptionHandling(publisher);
            
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

    public class PublisherRaisingEventsWithExceptionHandling 
    {
        private event EventHandler<CustomArgs> _onChange = delegate { };

        public event EventHandler<CustomArgs> OnChange
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
            foreach (var handler in _onChange.GetInvocationList())
            {
                try
                {
                    //Invoke events.
                    handler.DynamicInvoke(this, new CustomArgs(42));
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

    public class SubscriberRaisingEventsWithExceptionHandling : IDisposable
    {
        private readonly PublisherRaisingEventsWithExceptionHandling _publisher;
        public SubscriberRaisingEventsWithExceptionHandling(PublisherRaisingEventsWithExceptionHandling publisher)
        {
            _publisher = publisher;
            _publisher.OnChange += OnChangeFistHandler;
            _publisher.OnChange += OnChangeExceptionHandler;
            _publisher.OnChange += OnChangeSecondHandler;
        }
        private static void OnChangeExceptionHandler(object sender, CustomArgs e)
        {
            throw new Exception();
        }

        private static void OnChangeFistHandler(object sender, CustomArgs e)
        {
            Console.WriteLine("Subscriber 1 called. Event value: {0}", e.Value);
        }

        private static void OnChangeSecondHandler(object sender, CustomArgs e)
        {
            Console.WriteLine("Subscriber 3 called. Event value: {0}", e.Value);
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
