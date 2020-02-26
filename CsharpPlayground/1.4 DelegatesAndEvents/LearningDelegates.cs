using System;

namespace LearningDelegates
{
    //Declare a delegate. It is not required inside a class.
    public delegate void PrintDelegate(string value);

    //Now delegate using generics
    public delegate void PrintDelegateGenerics<T>(T value);

    public static class LearningDelegates
    {
        public delegate bool BoolDelegate(string text, int number);

        public static void Start()
        {
            InstantiateDelegate();
            InstantiateDelegateGenerics();
            DelegateUsingAction();
            DelegateUsingActionAnonymous();
            DelegateUsingFunc();
            DelegateUsingPredicate();

            UsingOtherClasses();

            Console.ReadLine();
        }

        public static bool AlwaysTrueConsolePrint(string text, int number)
        {
            Console.WriteLine($"Hello. I'm AlwaysTrueConsolePrint() Method. This are my parameters: text: {text}, number: {number}");
            return true;
        }

        //Same input and output as delegate to allow instantiate throw delegate.
        public static void ConsolePrint(string value)
        {
            Console.WriteLine($"Hello. I'm ConsolePrint() Method. {value}");
        }

        //This will be used for print delegate generics
        public static void ConsolePrint(int value)
        {
            Console.WriteLine($"Hello. I'm ConsolePrint() Method. {value}");
        }

        public static void InstantiateDelegate()
        {
            var printDelegate = new PrintDelegate(ConsolePrint);
            printDelegate("I'm a simple delegate.");

            var trueDelegate = new BoolDelegate(AlwaysTrueConsolePrint);
            var returnedValue = trueDelegate("sample text", 100);
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
            //Action always return void. Allows to 16 parameters.
            Action<string> consolePrintAction = ConsolePrint;
            consolePrintAction("I'm an action.");
        }
        
        public static void DelegateUsingActionAnonymous()
        {
            //Now using a new anonymous method to print on console.
            Action<string> consolePrintAnonymousAction = delegate(string value) { Console.WriteLine(value); };
            consolePrintAnonymousAction("I'm an action using an anonymous method");

            //Now using a new anonymous method to print on console.
            Action<string> consolePrintAnonymousLambdaAction = value => Console.WriteLine(value);
            consolePrintAnonymousLambdaAction("I'm an action using an anonymous method with lambda");

        }

        public static void DelegateUsingFunc()
        {
            //Func is similar to Action. But latest parameter is the output.
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
                return string.Empty;
            };

            consolePrintAnonymousLambdaFunc("I'm a func using an anonymous method with lambda");
        }

        public static void DelegateUsingPredicate()
        {
            //Similar to Func but Predicate always return true or false

            Predicate<int> adult = value => value > 17;
            var isAdult = adult(25);

            Console.WriteLine(isAdult ? "is adult": "is not adult");
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
        public static bool PassingDelegate(LearningDelegates.BoolDelegate @delegate)
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

        public static string PassingFunc(Func<string, string> @func)
        {
            string returnedValue = @func("I'm a func using an anonymous method from other class!");
            //Do some logic...
            return string.Empty;
        }

        public static bool PassingPredicate(Predicate<int> @predicate)
        {
            bool returnedValue = @predicate(25);
            //Do some logic...
            Console.WriteLine("Im a predicate executed from other class!");
            return true;
        }
    }
}
