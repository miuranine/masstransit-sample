using System;

namespace Sample.Contracts
{
    public interface IOrderNotFound
    {
        Guid OrderId { get; }
    }
}