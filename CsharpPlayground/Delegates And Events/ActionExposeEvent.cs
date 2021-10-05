using System;

namespace ActionExposeEventPublisher
{
    public static class ActionExposeEventPublisher
    {
        public static Action OnChange { get; set; }
        public static void Raise()
        {
            if (OnChange != null)
            {
                OnChange();
            }
        }
    }

    public static class ActionExposeEventSubscriber {
        public static void Start()
        {
            ActionExposeEventPublisher.OnChange += () => Console.WriteLine("Event raised to method 1");
            ActionExposeEventPublisher.OnChange += () => Console.WriteLine("Event raised to method 2");
            ActionExposeEventPublisher.Raise();

            Console.ReadLine();
        }
    }
}
