using System;

namespace ExceptionHandlingInspecting
{
    public static class ExceptionHandlingInspecting
    {
        public static void Start()
        {
            try
            {
                int i = ReadAndParse();
                Console.WriteLine("Parsed: {0}", i);
            }
            catch (FormatException e)
            {
                Console.WriteLine("Message: {0}", e.Message);
                Console.WriteLine("StackTrace: {0}", e.StackTrace);
                Console.WriteLine("HelpLink: {0}", e.HelpLink);
                Console.WriteLine("InnerException: {0}", e.InnerException);
                //Console.WriteLine("TargetSite: {0}", e.TargetSite);
                Console.WriteLine("Source: {0}", e.Source);
            }

            finally
            {
                Console.ReadLine();
            }


            //try
            //{
            //    SomeOperation();
            //}
            //catch (Exception logEx)
            //{
            //    Log(logEx);
            //    throw; // rethrow the original exception. Never mask with a new exception on rethrow
            //}
        }
        private static int ReadAndParse()
        {
            string s = Console.ReadLine();
            int i = int.Parse(s);
            return i;
        }
    }
}
