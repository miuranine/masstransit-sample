using System;
using System.Threading.Tasks;
using MassTransit;
using MassTransit.Courier;
using Sample.Contracts;

namespace Sample.Common.Consumers
{
    public class FulfillOrderConsumer : IConsumer<IFulfillOrder>
    {
        public async Task Consume(ConsumeContext<IFulfillOrder> context)
        {
            var builder = new RoutingSlipBuilder(NewId.NextGuid());

            builder.AddActivity("AllocateInventory", new Uri("queue:allocate-inventory_execute"), new
            {
                ItemNumber = "ITEM123",
                Quantity = 10.0m
            });
            
            builder.AddVariable("OrderId", context.Message.OrderId);
            builder.Build();

            var routingSlip = builder.Build();
            await context.Execute(routingSlip);
        }
    }
}