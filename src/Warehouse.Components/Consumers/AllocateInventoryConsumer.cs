using System.Threading.Tasks;
using MassTransit;
using Warehouse.Contracts;

namespace Warehouse.Components.Consumers
{
    public class AllocateInventoryConsumer : IConsumer<IAllocateInventory>
    {
        public async Task Consume(ConsumeContext<IAllocateInventory> context)
        {
            await context.Publish<IAllocationCreated>(new
            {
                context.Message.AllocationId,
                HoldDuration = 15000,
            });

            await context.RespondAsync<IInventoryAllocated>(new
            {
                context.Message.AllocationId,
                context.Message.ItemNumber,
                context.Message.Quantity
            });
        }
    }
}