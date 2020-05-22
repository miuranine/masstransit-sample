using System;

namespace Sample.Contracts
{
    public interface ICustomerAccountClosed
    {
        Guid CustomerId { get; }
        string CustomerNumber { get; }
    }
}