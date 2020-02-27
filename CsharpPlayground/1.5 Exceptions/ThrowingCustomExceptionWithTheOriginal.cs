using System;
using System.Runtime.Serialization;

namespace ThrowingCustomExceptionWithTheOriginal
{
    public static class ThrowingCustomExceptionWithTheOriginal
    {
        public static void Start()
        {
            try
            {
                var order = new Order(25);
                order.Process();
            }
            catch (MessageQueueException ex)
            {
                throw new OrderProcessingException(ex.Id, "Error while processing order", ex);
            }
        }
    }

    public class Order
    {
        public int Id { get; }

        public Order(int id)
        {
            Id = id;
        }

        public bool Process()
        {
            throw new MessageQueueException(Id);
        }
    }

    public class MessageQueueException : Exception
    {
        public int Id { get; protected set; }

        public MessageQueueException()
        {
        }

        public MessageQueueException(int id)
            : this(id, null, null)
        {   
        }

        public MessageQueueException(int id, string message) 
            : this (id, message, null)
        {   
        }

        public MessageQueueException(int id, string message, Exception innerException) 
            : base(message, innerException)
        {
            Id = id;
        }
    }
    
    public sealed class OrderProcessingException : MessageQueueException
    {
        public OrderProcessingException(int orderId) 
            : this(orderId, null)
        {
            
        }
        public OrderProcessingException(int orderId, string message)
            : this(orderId, message, null)
        {
            
        }
        public OrderProcessingException(int orderId, string message, Exception innerException)
            : base(orderId, message, innerException)
        {
            HelpLink = "http://www.mydomain.com/infoaboutexception";
        }

        //makes sure that your exception can be serialized and works correctly across application domains
        private OrderProcessingException(SerializationInfo info, StreamingContext context)
        {
            if (info == null)
            {
                throw new ArgumentNullException("info");
            }

            Id = (int)info.GetValue("Id", typeof(int));
        }

        //makes sure that your exception can be serialized and works correctly across application domains
        public override void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            if (info == null)
            {
                throw new ArgumentNullException("info");
            }

            info.AddValue("Id", Id, typeof(int));
        }
    }
}
