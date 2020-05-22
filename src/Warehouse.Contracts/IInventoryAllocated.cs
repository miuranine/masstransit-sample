using System;

namespace Warehouse.Contracts
{
    public interface IInventoryAllocated
    {
        Guid AllocationId { get; }
        
        string ItemNumber { get; }
        decimal Quantity { get; }
    }
}