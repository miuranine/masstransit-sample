using System;
using Automatonymous;
using MassTransit;
using Sample.Common.StateMachine.OrderStateMachineActivities;
using Sample.Contracts;

namespace Sample.Common.StateMachine
{
    public class OrderStateMachine : MassTransitStateMachine<OrderState>
    {
        public OrderStateMachine()
        {
            Event(() => OrderSubmitted, x => x.CorrelateById(m => m.Message.OrderId));
            Event(() => OrderAccepted, x => x.CorrelateById(m => m.Message.OrderId));
            Event(() => OrderStatusRequested, x =>
            {
                 x.CorrelateById(m => m.Message.OrderId);
                 x.OnMissingInstance(m => m.ExecuteAsync(async context =>
                 {
                     if (context.RequestId.HasValue)
                     {
                         await context.RespondAsync<IOrderNotFound>(new {context.Message.OrderId});
                     }
                 })); 
            });
            Event(() => AccountClosed,
                x => x.CorrelateBy((saga, context) => saga.CustomerNumber == context.Message.CustomerNumber));
            
            InstanceState(x => x.CurrentState);
            
            Initially(
                When(OrderSubmitted)
                    .Then(context =>
                    {
                        context.Instance.SubmitDate = context.Data.TimeStamp;
                        context.Instance.CustomerNumber = context.Data.CustomerNumber;
                        context.Instance.OrderDate = DateTimeOffset.UtcNow;
                    })
                    .TransitionTo(Submitted));
            
            During(Submitted, 
                Ignore(OrderSubmitted),
                When(AccountClosed)
                    .TransitionTo(Canceled),
                When(OrderAccepted)
                    .Activity(x => x.OfType<AcceptOrderActivity>())
                    .TransitionTo(Accepted));
            
            DuringAny(
                When(OrderStatusRequested)
                    .RespondAsync(x => x.Init<IOrderStatus>(new
                    {
                        OrderId = x.Instance.CorrelationId,
                        Sate = x.Instance.CurrentState
                    }))
                );
            
            DuringAny(
                When(OrderSubmitted)
                    .Then(context =>
                    {
                        context.Instance.SubmitDate = context.Data.TimeStamp;
                        context.Instance.CustomerNumber = context.Data.CustomerNumber;
                    })
                );
        }

        public State Submitted { get; private set; }
        public State Accepted { get; private set; }
        public State Canceled { get; private set; }
        public Event<IOrderSubmitted> OrderSubmitted { get; private set; }
        public Event<IOrderAccepted> OrderAccepted { get; private set; }
        public Event<ICheckOrder> OrderStatusRequested { get; private set; }
        public Event<ICustomerAccountClosed> AccountClosed { get; private set; }
    }
}