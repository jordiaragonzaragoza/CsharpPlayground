namespace HandleProperties
{
    using System;
    using System.Collections.Generic;

    public static class HandleProperties
    {
        public static void Start()
        {
            try
            {
                var person = new PersonWithHandledProperties()
                {
                    FirstName = string.Empty,
                    Value = 5
                };
            }
            
            catch (ArgumentException argumentException)
            {
                Console.Write($"An exception ocurred: {argumentException.Message}");
            }

            Console.ReadLine();
        }
    }


    public class PersonWithHandledProperties
    {
        private string _firstName;
        public string FirstName
        {
            get => _firstName;
            set
            {
                if (string.IsNullOrWhiteSpace(value))
                {
                    throw new ArgumentException("FistName can not be null");
                }
                _firstName = value;
            }
        }

        public int Value { get; set; }
    }
}
