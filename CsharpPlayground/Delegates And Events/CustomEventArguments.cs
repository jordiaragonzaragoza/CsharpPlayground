using System;

namespace CustomEventArguments
{
    public static class CustomEventArguments
    {
        public static void Start()
        {
            var publisher = new Publisher();
            var subscriber = new Subscriber(publisher);
            publisher.DoSomething();

            Console.ReadLine();
        }
    }

    public class CustomEventArgs : EventArgs
    {
        public CustomEventArgs (string message)
        {
            Message = message;
        }

        public string Message { get; set; }
    }

    public class Publisher
    {
        private event EventHandler<CustomEventArgs> _onChange = delegate { };

        // Declare the event using EventHandler<T>
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

        public void DoSomething()
        {
            // Write some code that does something useful here
            // then raise the event. You can also raise an event
            // before you execute a block of code.
            OnRaiseCustomEvent(new CustomEventArgs("Triggered"));
        }

        // Wrap event invocations inside a protected virtual method
        // to allow derived classes to override the event invocation behavior
        protected virtual void OnRaiseCustomEvent(CustomEventArgs e)
        {
            // Make a temporary copy of the event to avoid possibility of
            // a race condition if the last subscriber unsubscribes
            // immediately after the null check and before the event is raised.
            EventHandler<CustomEventArgs> raiseEvent = _onChange;

            // Event will be null if there are no subscribers
            if (raiseEvent != null)
            {
                // Format the string to send inside the CustomEventArgs parameter
                e.Message += $" at {DateTime.Now}";

                // Call to raise the event.
                raiseEvent(this, e);
            }
        }

    }
    
    public class Subscriber : IDisposable
    {
        private readonly Publisher _publisher;

        public Subscriber(Publisher publisher)
        {
            _publisher = publisher;
            _publisher.OnChange += PublisherOnChangeEventHandler;
        }

        private void PublisherOnChangeEventHandler(object sender, CustomEventArgs  e)
        {
            Console.WriteLine("Event raised: {0}", e.Message);
        }

        public void Dispose()
        {
            _publisher.OnChange -= PublisherOnChangeEventHandler;
            System.GC.SuppressFinalize(this);
        }
    }
}
