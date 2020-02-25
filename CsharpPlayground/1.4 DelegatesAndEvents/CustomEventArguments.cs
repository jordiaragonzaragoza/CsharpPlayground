using System;

namespace CustomEventArgumentsSubscriber
{
    public static class CustomEventArgumentsSubscriber
    {
        public static void Start()
        {
            var p = new CustomEventArgumentsPublisher();
            p.OnChange += (sender, e) => Console.WriteLine("Event raised: {0}", e.Value);
            p.Raise();

            Console.ReadLine();
        }
    }
    
    public class CustomEventArgumentsPublisher
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
            onChange(this, new CustomArgs(42));
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
}
