using System;

namespace InterfaceExplicitly
{
    public static class InterfaceExplicitly
    {
        public static void Start()
        {
            var test = new ExplicitImplementation();
            var result = ((IInterfaceExplicit)test).MyMethod();
            
            Console.WriteLine($"Result from method from interface explicit: {result}");

            var implicitExplicitImplementation = new ImplicitExplicitImplementation();
            var @implicit = implicitExplicitImplementation.Move();//Implicit. Default implementation. 
            var @explicit = ((IRight) implicitExplicitImplementation).Move();

            Console.WriteLine($"Result from method from interface implicit/explicit. Implicit: {@implicit}, Explicit: {@explicit}");

            Console.ReadLine();
        }
    }

    public interface IInterfaceExplicit
    {
        int MyMethod();
    }
    public class ExplicitImplementation : IInterfaceExplicit
    {
        //Implementation and mark method as interface explicit including interface name.
        //This force cast to the interface to use the method.
        int IInterfaceExplicit.MyMethod() 
        {
            return 5;
        }
    }

    public interface ILeft
    {
        int Move();
    }
    public interface IRight
    {
        int Move();
    }
    public class ImplicitExplicitImplementation : ILeft, IRight
    {
        //This will be the default implementation
        public int Move()
        {
            return 100;
        }

        //Required as Interface Explicit because different interface with same method signatures.
        //Used only on certain cases. Force cast to the interface to use the method.
        int IRight.Move()
        {
            return 200;
        }
    }
}
