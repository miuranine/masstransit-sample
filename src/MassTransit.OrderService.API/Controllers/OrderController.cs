using System;
using System.Threading.Tasks;
using Common.Events;
using MassTransit.OrderService.API.Models;
using Microsoft.AspNetCore.Mvc;

namespace MassTransit.OrderService.API.Controllers
{
    [Route("api/[controller]")]
    public class OrderController : Controller
    {
        private readonly IBus _bus;

        public OrderController(IBus bus)
        {
            _bus = bus;
        }
        
        // GET
        [HttpPost("place-order")]
        public async Task<ActionResult> Index([FromBody]OrderRequest request)
        {
            var order = new OrderEvent(Guid.NewGuid(), request.ProductName, request.Qty);
            var endpoint = await _bus.GetSendEndpoint(new Uri("amqp://127.0.0.1:5672/pre-order"));
            await endpoint.Send(order);

            return Ok("success");
        }
    }
}