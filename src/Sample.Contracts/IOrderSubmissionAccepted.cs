using System;

namespace Sample.Contracts
{
    public interface IOrderSubmissionAccepted
    {
        Guid OrderId { get; }
        DateTimeOffset Timestamp { get; }

        string CustomerNumber { get; }
    }
}