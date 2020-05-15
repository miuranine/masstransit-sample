using System;

namespace MassTransit.Common.Contracts
{
    public interface ISubmitOrder
    {
        public Guid OrderId { get; }
        public DateTimeOffset CreatedDate { get; }
        public string CustomerNumber { get; }
    }
}