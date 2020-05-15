using System;

namespace MassTransit.ProducerService.API.Models
{
    public class OrderRequest
    {
        public Guid Id { get; set; }
        public string CustomerNumber { get; set; }
    }
}