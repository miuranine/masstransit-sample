using System;
using System.Threading.Tasks;
using Common.Events;

namespace MassTransit.ConsumerService.Consumers
{
    public class OrderConsumer : IConsumer<OrderEvent>
    {
        public async Task Consume(ConsumeContext<OrderEvent> context)
        {
            var orderEvent = context.Message;
            
            await Console.Out.WriteLineAsync($"Updating order: {context.Message.Id}");
        }
    }
}