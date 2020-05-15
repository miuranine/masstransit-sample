using System;

namespace MassTransit.Common.Events
{
    public class OrderEvent
    {
        public Guid Id { get; private set; }
        public string CustomerNumber { get; private set; }
        public DateTimeOffset CreatedDate { get; private set; }

        public OrderEvent(Guid id, string customerNumber, DateTimeOffset createdDate)
        {
            Id = id;
            CustomerNumber = customerNumber;
            CreatedDate = createdDate;
        }
    }
}