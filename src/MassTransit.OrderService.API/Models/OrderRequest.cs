using System;

namespace MassTransit.OrderService.API.Models
{
    public class OrderRequest
    {
        public string ProductName { get; set; }
        public int Qty { get; private set; }
    }
}