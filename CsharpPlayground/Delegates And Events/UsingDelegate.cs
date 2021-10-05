using System;

namespace UsingDelegate
{
    public static class UsingDelegate
    {
        public delegate int Calculate(int x, int y);
        
        public static void Start()
        {
            Calculate calculateDelegate = Add;
            Console.WriteLine(calculateDelegate(3, 4)); // Displays 7

            calculateDelegate = Multiply;
            Console.WriteLine(calculateDelegate(3, 4)); // Displays 12

            var otherCalculateDelegate = new Calculate(Add);
            Console.WriteLine(otherCalculateDelegate(3, 4)); // Displays 7

            Console.ReadLine();
        }

        public static int Add(int x, int y)
        {
            return x + y;
        }

        public static int Multiply(int x, int y)
        {
            return x * y;
        }
    }
}