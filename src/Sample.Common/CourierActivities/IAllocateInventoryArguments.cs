using System;

namespace Sample.Common.CourierActivities
{
    public interface IAllocateInventoryArguments
    {
        Guid OrderId { get; }
        string ItemNumber { get; }
        decimal Quantity { get; }
    }
}