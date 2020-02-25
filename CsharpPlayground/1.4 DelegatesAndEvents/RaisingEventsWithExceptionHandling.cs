using System;
using System.Collections.Generic;
using System.Linq;
using CustomEventArgumentsSubscriber;

namespace RaisingEventsWithExceptionHandlingSubscriber
{
    public static class RaisingEventsWithExceptionHandlingSubscriber
    {
        public static void Start()
        {
            var p = new RaisingEventsWithExceptionHandlingPublisher();
            p.OnChange += (sender, e) => Console.WriteLine("Subscriber 1 called. Event value: {0}", e.Value);
            p.OnChange += (sender, e) => throw new Exception();
            p.OnChange += (sender, e) => Console.WriteLine("Subscriber 3 called. Event value: {0}", e.Value);
            try
            {
                p.Raise();
            }
            catch (AggregateException ex)
            {
                Console.WriteLine(ex.InnerExceptions.Count);
            }

            Console.ReadLine();
        }
    }

    public class RaisingEventsWithExceptionHandlingPublisher 
    {
        private event EventHandler<CustomArgs> onChange = delegate { };

        public event EventHandler<CustomArgs> OnChange
        {
            add
            {
                lock (onChange)
                {
                    onChange += value;
                }
            }
            remove
            {
                lock (onChange)
                {
                    onChange -= value;
                }
            }
        }

        public void Raise()
        {
            var exceptions = new List<Exception>();
            foreach (var handler in onChange.GetInvocationList())
            {
                try
                {
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
}
