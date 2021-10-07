using System;
using System.IO;

namespace ImplicitExplicitConversion
{
    public static class ImplicitExplicitConversion
    {
        public static void Start()
        {
            var money = new Money(42.42M);

            decimal amount = money;

            Console.WriteLine($"Implicit Operator: {amount}");

            var truncatedAmount = (int)money;

            Console.WriteLine($"Explicit Operator: {truncatedAmount}");


            var truncatedAmountDouble = (double)money;

            Console.WriteLine($"Explicit Operator (double): {truncatedAmountDouble}");

            Console.ReadLine();
        
        }
    }

    public class Money
    {
        public decimal Amount { get; set; }

        public Money(decimal amount)
        {
            Amount = amount;
        }
        
        //Declare implicit operator
        public static implicit operator decimal(Money money)
        {
            return money.Amount;
        }

        //Declare explicit operator
        public static explicit operator int(Money money)
        {
            return (int)money.Amount;
        }

        //Declare explicit operator
        public static explicit operator double(Money money)
        {
            return (double)money.Amount;
        }

        public static void BuiltInConvertAndParse()
        {
            int value = Convert.ToInt32("42");

            value = int.Parse("42");

            bool success = int.TryParse("42", out value);
        }
    }
}
