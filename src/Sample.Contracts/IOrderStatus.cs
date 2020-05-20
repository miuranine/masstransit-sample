using System;

namespace Sample.Contracts
{
    public interface IOrderStatus
    {
        Guid OrderId { get; }

        string State { get; }
    }
}