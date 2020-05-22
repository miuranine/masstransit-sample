using System;
using System.Threading.Tasks;
using MassTransit;
using MassTransit.Courier;
using Warehouse.Contracts;

namespace Sample.Common.CourierActivities
{
    public class AllocateInventoryActivity : IActivity<IAllocateInventoryArguments, IAllocateInventoryLog>
    {
        readonly IRequestClient<IAllocateInventory> _client;
        
        public async Task<ExecutionResult> Execute(ExecuteContext<IAllocateInventoryArguments> context)
        {
            var orderId = context.Arguments.OrderId;

            var itemNumber = context.Arguments.ItemNumber;
            if (string.IsNullOrEmpty(itemNumber))
                throw new ArgumentNullException(nameof(itemNumber));

            var quantity = context.Arguments.Quantity;
            if (quantity <= 0.0m)
                throw new ArgumentNullException(nameof(quantity));

            var allocationId = NewId.NextGuid();

            var response = await _client.GetResponse<IInventoryAllocated>(new
            {
                AllocationId = allocationId,
                ItemNumber = itemNumber,
                Quantity = quantity
            });

            return context.Completed(new {AllocationId = allocationId});
        }

        public async Task<CompensationResult> Compensate(CompensateContext<IAllocateInventoryLog> context)
        {
            await context.Publish<IAllocationReleaseRequested>(new
            {
                context.Log.AllocationId,
                Reason = "Order Faulted"
            });

            return context.Compensated();
        }
    }
}