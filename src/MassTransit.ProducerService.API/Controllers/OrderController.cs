using System;
using System.Threading.Tasks;
using MassTransit.Common.Contracts;
using MassTransit.Common.Events;
using MassTransit;
using MassTransit.ProducerService.API.Models;
using Microsoft.AspNetCore.Mvc;

namespace MassTransit.ProducerService.API.Controllers
{
    [Route("api/[controller]")]
    public class OrderController : Controller
    {
        private readonly IBus _bus;
        private readonly IRequestClient<ISubmitOrder> _submitOrderRequestClient;

        public OrderController(IRequestClient<ISubmitOrder> submitOrderRequestClient)
        {
            //_bus = bus;
            _submitOrderRequestClient = submitOrderRequestClient;
        }
        
        // GET
        [HttpPost("place-order")]
        public async Task<ActionResult> Index([FromBody]OrderRequest request)
        {
            var order = new OrderEvent(Guid.NewGuid(), request.CustomerNumber, DateTimeOffset.UtcNow);
            var endpoint = await _bus.GetSendEndpoint(new Uri("amqp://127.0.0.1:5672/pre-order"));
            await endpoint.Send(order);

            return Ok("success");
        }

        [HttpPost("mediator-event")]
        public async Task<ActionResult> PostMediator([FromBody] OrderRequest request)
        {
            var (accepted, rejected) = await _submitOrderRequestClient.GetResponse<IOrderSubmissionAccepted, IOrderSubmissionRejected>(new
            {
                OrderId = request.Id,
                CreatedDate = DateTimeOffset.UtcNow,
                CustomerNumber = request.CustomerNumber
            });

            if (accepted.IsCompletedSuccessfully)
            {
                var response = await accepted;
                return Ok(response);
            }

            var rejectedResponse = await rejected;
            return BadRequest(rejectedResponse.Message);

        }
    }
}