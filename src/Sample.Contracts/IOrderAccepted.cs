using System;

namespace Sample.Contracts
{
    public interface IOrderAccepted
    {
        Guid OrderId { get; }
        DateTimeOffset Timestamp { get; }
    }
}