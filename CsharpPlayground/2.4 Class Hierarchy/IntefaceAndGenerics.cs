using System;
using System.Collections.Generic;
using System.Linq;

namespace IntefaceAndGenerics
{
    public static class IntefaceAndGenerics
    {
        public static void Start()
        {
            var order = new Order(12, 25);
            var orders = new List<Order> {order}; 

            var orderRepository = new OrderRepository(orders);

            const int amount = 10;
            var result = orderRepository.FilterOrdersOnAmountGreaterOrEqual(amount);

            Console.WriteLine($"Orders filtered on amount: {amount} are: {result.Count()}");

            var orderFinded = orderRepository.FindById(12);
            Console.WriteLine($"Order find by id: {orderFinded.Id} has amount: {orderFinded.Amount}");

            Console.ReadLine();
        }
    }

    public interface IEntity
    {
        int Id { get; }
    }

    public class Repository<T> where T : IEntity
    {
        protected IEnumerable<T> elements;

        public Repository(IEnumerable<T> elements)
        {
            this.elements = elements;
        }

        public T FindById(int id)
        {
            return elements.SingleOrDefault(e => e.Id == id);
        }
    }

    public class Order : IEntity
    {
        public int Id { get; }

        public int Amount { get; set; }
        public Order(int id, int amount)
        {
            Id = id;
            Amount = amount;
        }
    }

    public class OrderRepository : Repository<Order>
    {
        public OrderRepository(IEnumerable<Order> orders)
            : base(orders)
        {
        }

        public IEnumerable<Order> FilterOrdersOnAmountGreaterOrEqual(int amount)
        {
            return elements.Where(e => e.Amount >= amount);
        }
    }
}
