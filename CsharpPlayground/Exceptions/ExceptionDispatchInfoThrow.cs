namespace ExceptionDispatchInfoThrow
{
    using System;
    using System.Runtime.ExceptionServices;

    public static class ExceptionDispatchInfoThrow
    {
        public static void Start()
        {
            ExceptionDispatchInfo possibleException = null;

            try
            {
                string s = Console.ReadLine();
                int.Parse(s);
            }
            catch (FormatException ex)
            {
                possibleException = ExceptionDispatchInfo.Capture(ex); //Used to rethrow exceptions between threads.
            }

            if (possibleException != null)
            {
                possibleException.Throw();
            }

            Console.ReadLine();
        }
    }
}
