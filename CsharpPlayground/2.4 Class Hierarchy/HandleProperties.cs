using System;

namespace HandleProperties
{
    public static class HandleProperties
    {
        public static void Start()
        {
            var person = new PersonWithHandledProperties()
            {
                FirstName = string.Empty,
                Value = 5
            };
        }
    }


    public class PersonWithHandledProperties
    {
        private string _firstName;
        public string FirstName
        {
            get { return _firstName; }
            set
            {
                if (string.IsNullOrWhiteSpace(value))
                {
                    throw new ArgumentException();
                }
                _firstName = value;
            }
        }

        public int Value { get; set; }
    }
}
