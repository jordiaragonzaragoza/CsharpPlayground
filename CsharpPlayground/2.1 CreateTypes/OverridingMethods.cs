using System;

namespace OverridingMethods
{
    public static class OverridingMethods
    {
        public static void Start()
        {
            var derived = new Derived().FirstMethod();
            Console.WriteLine($"Ouput override calling base class: {derived}");

            Console.ReadLine();
        }
    }

    public class Base //Base class can not be static
    {
        public virtual int FirstMethod()
        {
            return 42;
        }

        public virtual int SecondMethod()
        {
            return 0;
        }
    }
    public class Derived : Base
    {
        public override int FirstMethod()
        {
            return base.FirstMethod() * 2;
        }

        //This method will not be available for overrides
        public sealed override int SecondMethod()
        {
            return base.SecondMethod() + 1;
        }
    }
}
