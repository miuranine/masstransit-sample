using System;
namespace Sample.Contracts
{
    public interface IDriverAcceptedOrder
    {
        Guid OrderId { get; }
        DateTimeOffset TimeStamp { get; }
    }
}
