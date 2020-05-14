using System;
using Automatonymous;

namespace MassTransit.PaymentService.API.States
{
    public class OrderState : SagaStateMachineInstance
    {
        public Guid CorrelationId { get; set; }
        public int CurrentState { get; set; }
    }

    public class OrderStateMachine : MassTransitStateMachine<OrderState>
    {
        public State Submitted { get; private set; }
        public State Accepted { get; private set; }
        
        public OrderStateMachine()
        {
            InstanceState(x => x.CurrentState, Submitted, Accepted);
        }
    }
}