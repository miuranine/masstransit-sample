using System;
using Automatonymous;
using MassTransit;
using Sample.Contracts;

namespace Sample.Common.StateMachine
{
    public class OrderStateMachine : MassTransitStateMachine<OrderState>
    {
        public OrderStateMachine()
        {
            Event(() => OrderSubmitted, x => x.CorrelateById(m => m.Message.OrderId));
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
            Event(() => StoreAcceptedOrder, x => x.CorrelateById(m => m.Message.OrderId));
            Event(() => DriverAcceptedOrder, x => x.CorrelateById(m => m.Message.OrderId));

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
                Ignore(OrderSubmitted));

            During(Submitted,
                When(StoreAcceptedOrder)
                    .Then(context =>
                    {
                        context.Instance.StoreAcceptedDate = context.Data.TimeStamp;
                    })
                    .TransitionTo(StoreAccepted),
                When(DriverAcceptedOrder)
                    .Then(context =>
                    {
                        context.Instance.DriverAcceptedDate = context.Data.TimeStamp;
                    })
                    .TransitionTo(HaveDriver));
            
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
        public State StoreAccepted { get; private set; }
        public State HaveDriver { get; private set; }

        public Event<IOrderSubmitted> OrderSubmitted { get; private set; }
        public Event<ICheckOrder> OrderStatusRequested { get; private set; }
        public Event<IStoreAcceptedOrder> StoreAcceptedOrder { get; private set; }
        public Event<IDriverAcceptedOrder> DriverAcceptedOrder { get; private set; }
    }
}