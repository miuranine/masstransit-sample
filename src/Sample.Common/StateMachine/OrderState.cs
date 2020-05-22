using System;
using Automatonymous;

namespace Sample.Common.StateMachine
{
    public class OrderState : SagaStateMachineInstance
    {
        public Guid CorrelationId { get; set; }
        public string? CurrentState { get; set; }
        public string? CustomerNumber { get; set; }
        public DateTimeOffset? OrderDate { get; set; }
        public DateTimeOffset? SubmitDate { get; set; }

        public DateTimeOffset? StoreAcceptedDate { get; set; }
        public DateTimeOffset? DriverAcceptedDate { get; set; }
    }
}