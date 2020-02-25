using System;

namespace ExtensionMethods
{
    public static class ExtensionMethods
    {
        public static void Start()
        {
            Console.WriteLine($"Price with discount {Calculator.CalculateDiscount(new Product() {Price = 100})}");

            Console.ReadLine();
        }
    }

    public class Product
    {
        public decimal Price { get; set; }
    }
    public static class CalculatorExtensions
    {
        //The method must be static and first parameter with this. And in same namespace.
        //An extension method cannot only be declared on a class or struct. It can also be declared on an interface
        public static decimal Discount(this Product product) 
        {
            return product.Price * .9M;
        }
    }
    public static class Calculator
    {
        public static decimal CalculateDiscount(Product p)
        {
            return p.Discount();
        }
    }
}
