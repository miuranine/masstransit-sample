using System;

namespace MassTransit.OrderService.API.Models
{
    public class Order
    {
        public string ProductName { get; set; }
        public int Qty { get; private set; }
    }
}