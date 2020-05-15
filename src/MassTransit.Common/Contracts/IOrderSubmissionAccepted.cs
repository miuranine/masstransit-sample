using System;

namespace MassTransit.Common.Contracts
{
    public interface IOrderSubmissionAccepted
    {
        Guid OrderId { get; }
        DateTimeOffset CreatedDate { get; }

        string CustomerNumber { get; }
    }
}