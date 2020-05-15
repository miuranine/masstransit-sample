using System;

namespace MassTransit.Common.Contracts
{
    public interface IOrderSubmissionRejected
    {
        Guid OrderId { get; }
        DateTimeOffset CreatedDate { get; }

        string CustomerNumber { get; }
        string Reason { get; }
    }
}