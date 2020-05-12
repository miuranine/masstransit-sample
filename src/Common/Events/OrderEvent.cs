using System;

namespace Common.Events
{
    public class OrderEvent
    {
        public Guid Id { get; private set; }
        public string ProductName { get; private set; }
        public int Qty { get; private set; }

        public OrderEvent(Guid id, string productName, int qty)
        {
            Id = id;
            ProductName = productName;
            Qty = qty;
        }
    }
}