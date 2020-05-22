using System;
using System.Threading;
using System.Threading.Tasks;
using MassTransit;
using Microsoft.Extensions.Logging;
using Sample.Contracts;

namespace Sameple.StoreService
{
    public class StoreOrderConsumer : IConsumer<IOrderSubmitted>
    {
        private readonly ILogger<StoreOrderConsumer> _logger;

        public StoreOrderConsumer(ILogger<StoreOrderConsumer> logger)
        {
            _logger = logger;
        }

        public async Task Consume(ConsumeContext<IOrderSubmitted> context)
        {
            _logger.Log(LogLevel.Debug, "Store received order: {OrderId}", context.Message.OrderId);

            Thread.Sleep(5000);

            await context.Publish<IStoreAcceptedOrder>(new
            {
                context.Message.OrderId,
                DateTimeOffset.Now
            });

            Thread.Sleep(3000);

            await context.Publish<IDriverAcceptedOrder>(new
            {
                context.Message.OrderId,
                DateTimeOffset.Now
            });
        }
    }
}
