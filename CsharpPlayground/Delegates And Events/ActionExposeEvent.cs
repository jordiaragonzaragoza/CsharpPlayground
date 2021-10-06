namespace ActionExposeEvent
{
    using System;

    public static class Publisher
    {
        //Action:    works as delegate but it returns void.
        public static Action OnChange { get; set; }
        public static void Raise()
        {
            if (OnChange != null)
            {
                OnChange();
            }

            // Also can be directly invoke.
            // OnChange?.Invoke();
        }
    }

    public static class ActionExposeEventSubscriber {
        public static void Start()
        {
            Publisher.OnChange += () => Console.WriteLine("Event raised to method 1");
            Publisher.OnChange += () => Console.WriteLine("Event raised to method 2");
            Publisher.Raise();

            Console.ReadLine();
        }
    }
}
