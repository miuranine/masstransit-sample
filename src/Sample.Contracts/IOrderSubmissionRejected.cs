using System;

namespace Sample.Contracts
{
    public interface IOrderSubmissionRejected
    {
        Guid OrderId { get; }
        DateTimeOffset Timestamp { get; }

        string CustomerNumber { get; }
        string Reason { get; }
    }
}