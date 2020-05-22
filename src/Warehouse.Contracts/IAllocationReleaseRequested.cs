using System;

namespace Warehouse.Contracts
{
    public interface IAllocationReleaseRequested
    {
        Guid AllocationId { get; }

        string Reason { get; }
    }
}