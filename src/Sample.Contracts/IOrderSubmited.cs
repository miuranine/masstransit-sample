using System;

namespace Sample.Contracts
{
    public interface IOrderSubmitted
    {
        Guid OrderId { get; }
        DateTimeOffset TimeStamp { get; }
        string CustomerNumber { get; }
    }
}