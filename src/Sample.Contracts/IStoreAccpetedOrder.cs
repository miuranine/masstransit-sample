using System;
namespace Sample.Contracts
{
    public interface IStoreAcceptedOrder
    {
        Guid OrderId { get; }
        DateTimeOffset TimeStamp { get; }
    }
}
