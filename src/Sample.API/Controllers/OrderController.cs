using System;
using System.Threading.Tasks;
using MassTransit;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Sample.API.Models;
using Sample.Contracts;

namespace Sample.API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class OrderController : ControllerBase
    {
        private readonly ILogger<OrderController> _logger;
        private readonly IRequestClient<ISubmitOrder> _submitOrderRequestClient;
        private readonly IRequestClient<ICheckOrder> _checkOrderRequestClient;
        private readonly ISendEndpointProvider _sendEndpointProvider;
        private readonly IPublishEndpoint _publishEndpoint;

        public OrderController(ILogger<OrderController> logger, 
            IRequestClient<ISubmitOrder> submitOrderRequestClient,
            IRequestClient<ICheckOrder> checkOrderRequestClient,
            ISendEndpointProvider sendEndpointProvider,
            IPublishEndpoint publishEndpoint)
        {
            _logger = logger;
            _submitOrderRequestClient = submitOrderRequestClient;
            _checkOrderRequestClient = checkOrderRequestClient;
            _sendEndpointProvider = sendEndpointProvider;
            _publishEndpoint = publishEndpoint;
        }
        
        [HttpGet]
        public async Task<IActionResult> Get(Guid id)
        {
            var (status, notFound) = await _checkOrderRequestClient.GetResponse<IOrderStatus, IOrderNotFound>(new {OrderId = id});

            if (status.IsCompletedSuccessfully)
            {
                var response = await status;
                return Ok(response.Message);
            }
            else
            {
                var response = await notFound;
                return NotFound(response.Message);
            }
        }
        
        [HttpPost]
        public async Task<IActionResult> Post([FromBody]OrderViewModel model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var (accepted, rejected) = await _submitOrderRequestClient.GetResponse<IOrderSubmissionAccepted, IOrderSubmissionRejected>(new
            {
                OrderId = model.Id,
                InVar.Timestamp,
                model.CustomerNumber,
                model.PaymentCardNumber,
                model.Notes
            });

            if (accepted.IsCompletedSuccessfully)
            {
                var response = await accepted;

                return Accepted(response);
            }

            if (accepted.IsCompleted)
            {
                await accepted;

                return Problem("Order was not accepted");
            }
            else
            {
                var response = await rejected;

                return BadRequest(response.Message);
            }
        }
        
        [HttpPatch]
        public async Task<IActionResult> Patch(Guid id)
        {
            await _publishEndpoint.Publish<IOrderAccepted>(new
            {
                OrderId = id,
                InVar.Timestamp,
            });

            return Accepted();
        }

        [HttpPut]
        public async Task<IActionResult> Put([FromBody]OrderViewModel model)
        {
            var endpoint = await _sendEndpointProvider.GetSendEndpoint(new Uri("exchange:submit-order"));
            await endpoint.Send<ISubmitOrder>(new
            {
                OrderId = model.Id,
                InVar.Timestamp,
                model.CustomerNumber,
                model.PaymentCardNumber,
                model.Notes
            });

            return Accepted();
        }
    }
}