using System;

namespace Sample.Contracts
{
    public interface IFulfillOrder
    {
        Guid OrderId { get; }
        string CustomerNumber { get; }
        string PaymentCardNumber { get; }
    }
}