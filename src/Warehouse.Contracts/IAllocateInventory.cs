using System;

namespace Warehouse.Contracts
{
    public interface IAllocateInventory
    {
        Guid AllocationId { get; }
        string ItemNumber { get; }
        decimal Quantity { get; }
    }
}