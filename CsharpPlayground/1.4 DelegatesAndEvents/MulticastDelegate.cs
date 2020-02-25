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
        public delegate void Del();

        public static void Start()
        {
            Del d = MethodOne;
            d += MethodTwo;
            d();

            Console.ReadLine();
        }
    }
}