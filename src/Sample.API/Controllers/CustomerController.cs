using System;
using System.Threading.Tasks;
using MassTransit;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Sample.Contracts;

namespace Sample.API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class CustomerController : ControllerBase
    {
        private readonly ILogger<CustomerController> _logger;
        private readonly IPublishEndpoint _publishEndpoint;

        public CustomerController(ILogger<CustomerController> logger, IPublishEndpoint publishEndpoint)
        {
            _logger = logger;
            _publishEndpoint = publishEndpoint;
        }
        
        [HttpDelete]
        public async Task<IActionResult> Delete(Guid id, string customerNumber)
        {
            await _publishEndpoint.Publish<ICustomerAccountClosed>(new
            {
                CustomerId = id,
                CustomerNumber = customerNumber
            });

            return Ok();
        }
    }
}