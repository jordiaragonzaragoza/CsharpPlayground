using System;

namespace DelegateActionPredicateFunc
{
    /***Summary***/

    //Delegate is a generic form to send a method as parameter to other method.

    //Action:    works as delegate but it returns void
    //Predicate: works as delegate but it returns a bool
    //Func:      works as delegate but with a required output of any type.
    
    //Declare a delegate. It is not required inside a class.
    public delegate void PrintDelegate(string value);

    //Now delegate using generics
    public delegate void PrintDelegateGenerics<T>(T value);

    public static class DelegateActionPredicateFunc
    {
        //Delegate with input and output in a class
        public delegate bool BoolDelegate(string text, int number);

        public static void Start()
        {
            InstantiateDelegate();
            InstantiateDelegateGenerics();
            DelegateUsingAction();
            DelegateUsingActionAnonymous();
            DelegateUsingPredicate();
            DelegateUsingFunc();

            UsingOtherClasses();

            Console.ReadLine();
        }

        public static bool AlwaysTrueConsolePrint(string text, int number)
        {
            Console.WriteLine($"I'm AlwaysTrueConsolePrint() Method. This are my parameters: text: {text}, number: {number}");

            return true;
        }

        //Same input and output as delegate to allow instantiate throw delegate.
        public static void ConsolePrint(string value)
        {
            Console.WriteLine($"I'm ConsolePrint() Method. My parameter string value is: {value}");
        }

        //This will be used for print delegate generics.
        public static void ConsolePrint(int value)
        {
            Console.WriteLine($"I'm ConsolePrint() Method. My parameter int value is: {value}");
        }

        public static void ConsolePrintHello()
        {
            Console.WriteLine("I'm ConsolePrintHello() Method.");
        }

        public static void InstantiateDelegate()
        {
            var printDelegate = new PrintDelegate(ConsolePrint);
            printDelegate("I'm a simple delegate.");

            PrintDelegate otherPrintDelegate = ConsolePrint;
            otherPrintDelegate("I'm other simple delegate");

            var trueDelegate = new BoolDelegate(AlwaysTrueConsolePrint);
            var returnedValue = trueDelegate("sample text", 100);
            Console.WriteLine($"BoolDelegate returned value is: {returnedValue}");
        }

        public static void InstantiateDelegateGenerics()
        {
            var printDelegateGenericsString = new PrintDelegateGenerics<string>(ConsolePrint);
            printDelegateGenericsString("I'm a delegate with generics");

            var printDelegateGenericsInt = new PrintDelegateGenerics<int>(ConsolePrint);
            printDelegateGenericsInt(5);

        }

        public static void DelegateUsingAction()
        {
            //Action works as delegate but it returns void. Allows to 16 input parameters.
            Action consolePrintHelloAction = ConsolePrintHello;
            consolePrintHelloAction();

            Action<string> consolePrintAction = ConsolePrint;
            consolePrintAction("I'm an action.");
        }
        
        public static void DelegateUsingActionAnonymous()
        {
            //Now using a new anonymous method to print on console.

            Action consolePrintAnonymousActionWithoutParameter = delegate { Console.WriteLine("I'm an action using an anonymous method without parameter."); };
            consolePrintAnonymousActionWithoutParameter();

            Action<string> consolePrintAnonymousAction = delegate(string value) { Console.WriteLine(value); };
            consolePrintAnonymousAction("I'm an action using an anonymous method");

            //Now using a new anonymous method with lambda to print on console.
            Action<string> consolePrintAnonymousLambdaAction = value => Console.WriteLine(value);
            consolePrintAnonymousLambdaAction("I'm an action using an anonymous method with lambda");
        }

        public static void DelegateUsingPredicate()
        {
            //Predicate works as delegate but it returns a bool. At least one input parameter is required. Allows to 16 input parameters.
            Predicate<int> adult = value => value > 17;
            var isAdult = adult(25);

            Console.WriteLine(isAdult ? "is adult" : "is not adult");
        }

        public static void DelegateUsingFunc()
        {
            //Func works as delegate but with a required output of any type. Allows to 16 input parameters. Latest parameter is the output.
            //Func will be used massive on LINQ
            Func<string, string> consolePrintAnonymousFunc = delegate(string value)
            {
                Console.WriteLine(value);
                return string.Empty;
            };

            consolePrintAnonymousFunc("I'm a func using an anonymous method");

            Func<string, string> consolePrintAnonymousLambdaFunc = value =>
            {
                Console.WriteLine(value);
                return "consolePrintAnonymousLambdaFunc() Executed.";
            };

            var returnedValue = consolePrintAnonymousLambdaFunc("I'm a func using an anonymous method with lambda");
            Console.WriteLine(returnedValue);
        }

        public static void UsingOtherClasses()
        {
            //Passing Delegate
            var trueDelegate = new BoolDelegate(AlwaysTrueConsolePrint);
            var boolDelegateReturnedValue = OtherClass.PassingDelegate(trueDelegate);

            //Passing Action
            Action<string> consolePrintAction = ConsolePrint;
            OtherClass.PassingAction(consolePrintAction);

            //Passing Func. Output string
            Func<string, string> consolePrintAnonymousFunc = delegate (string value)
            {
                Console.WriteLine(value);
                return string.Empty;
            };

            var funcReturnedValue = OtherClass.PassingFunc(consolePrintAnonymousFunc);

            //Passing Predicate
            Predicate<int> adult = value => value > 17;
            var predicateReturnedValue = OtherClass.PassingPredicate(adult);
        }
    }

    //Delegates allow send a method as parameter in other classes
    public static class OtherClass
    {
        public static bool PassingDelegate(DelegateActionPredicateFunc.BoolDelegate @delegate)
        {
            bool returnedValue = @delegate("sample text", 100);
            //Do some logic...
            Console.WriteLine("Im a delegate executed from other class!");
            return true;
        }
    
        public static void PassingAction(Action<string> action)
        {
            action("Im an action executed from other class!");
            //Do some logic...
        }

        public static bool PassingPredicate(Predicate<int> @predicate)
        {
            bool returnedValue = @predicate(25);
            //Do some logic...
            Console.WriteLine("Im a predicate executed from other class!");
            return true;
        }

        public static string PassingFunc(Func<string, string> @func)
        {
            string returnedValue = @func("I'm a func using an anonymous method from other class!");
            //Do some logic...
            return string.Empty;
        }
    }
}
