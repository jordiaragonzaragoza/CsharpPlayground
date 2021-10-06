using System;

namespace MulticastDelegate
{
    public static class MulticastDelegate
    {
        public static void MethodOne()
        {
            Console.WriteLine("MethodOne");
        }
        public static void MethodTwo()
        {
            Console.WriteLine("MethodTwo");
        }
        public delegate void Delegate();

        public static void Start()
        {
            Delegate @delegate = MethodOne;
            @delegate += MethodTwo;
            @delegate();

            Console.ReadLine();
        }
    }
}