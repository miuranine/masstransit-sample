using System;
using MassTransit;

namespace Sample.Contracts
{
    public interface ISubmitOrder
    {
        Guid OrderId { get; }
        DateTimeOffset Timestamp { get; }

        string CustomerNumber { get; }
        string PaymentCardNumber { get; }

        MessageData<string> Notes { get; }
    }
}