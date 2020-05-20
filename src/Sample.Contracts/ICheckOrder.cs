using System;

namespace Sample.Contracts
{
    public interface ICheckOrder
    {
        Guid OrderId { get; }
    }
}