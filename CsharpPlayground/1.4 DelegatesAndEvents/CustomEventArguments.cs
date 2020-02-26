using System;

namespace CustomEventArguments
{
    public static class CustomEventArguments
    {
        public static void Start()
        {
            var publisher = new PublisherCustomEventArguments();
            var subscriber = new SubscriberCustomEventArguments(publisher);
            publisher.MethodWhoRaiseOnChangeEvent();


            Console.ReadLine();
        }
    }

    public class CustomArgs : EventArgs
    {
        public CustomArgs(int value)
        {
            Value = value;
        }
        public int Value { get; set; }
    }

    public class PublisherCustomEventArguments
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

        public void MethodWhoRaiseOnChangeEvent()
        {
            _onChange(this, new CustomArgs(42));
        }
    }
    
    public class SubscriberCustomEventArguments : IDisposable
    {
        private readonly PublisherCustomEventArguments _publisher;

        public SubscriberCustomEventArguments(PublisherCustomEventArguments publisher)
        {
            _publisher = publisher;
            _publisher.OnChange += _publisher_OnChange;
        }

        private void _publisher_OnChange(object sender, CustomArgs e)
        {
            Console.WriteLine("Event raised: {0}", e.Value);
        }

        public void Dispose()
        {
            _publisher.OnChange -= _publisher_OnChange;
        }
    }
}
