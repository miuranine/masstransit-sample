using System;

namespace MassTransit.ProducerService.API.Models
{
    public class OrderRequest
    {
        public string ProductName { get; set; }
        public int Qty { get; private set; }
    }
}