using System.Threading.Tasks;
using MassTransit;
using Microsoft.Extensions.Logging;
using Sample.Contracts;

namespace Sample.Common.Consumers
{
    public class SubmitOrderConsumer : IConsumer<ISubmitOrder>
    {
        private readonly ILogger<SubmitOrderConsumer> _logger;

        public SubmitOrderConsumer(ILogger<SubmitOrderConsumer> logger)
        {
            _logger = logger;
        }
        
        public async Task Consume(ConsumeContext<ISubmitOrder> context)
        {
            _logger.Log(LogLevel.Debug, "SubmitOrderConsomer: {CustomerNumber}", context.Message.CustomerNumber);
            if (context.Message.CustomerNumber.Contains("TEST"))
            {
                if (context.RequestId != null)
                {
                    await context.RespondAsync<IOrderSubmissionRejected>(new
                    {
                        InVar.Timestamp,
                        context.Message.OrderId,
                        context.Message.CustomerNumber,
                        Reason = $"Test Customer cannot submit orders: {context.Message.CustomerNumber}"
                    });
                
                    return;
                }
            }

            //var customer = Task.FromResult("Hello");
            await context.Publish<IOrderSubmitted>(new
            {
                context.Message.OrderId,
                context.Message.Timestamp,
                context.Message.CustomerNumber
            });

            if (context.RequestId != null)
            {
                await context.RespondAsync<IOrderSubmissionAccepted>(new
                {
                    InVar.Timestamp,
                    context.Message.OrderId,
                    context.Message.CustomerNumber
                });
            }
        }
    }
}