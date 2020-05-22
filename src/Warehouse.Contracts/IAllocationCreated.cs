using System;

namespace Warehouse.Contracts
{
    public interface IAllocationCreated
    {
        Guid AllocationId { get; }
        TimeSpan HoldDuration { get; }
    }
}