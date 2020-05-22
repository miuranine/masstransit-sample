using System;
using System.Threading.Tasks;
using Automatonymous;
using GreenPipes;
using MassTransit;
using Sample.Contracts;

namespace Sample.Common.StateMachine.OrderStateMachineActivities
{
    public class AcceptOrderActivity :
        Activity<OrderState, IOrderAccepted>
    {
        public void Probe(ProbeContext context)
        {
            context.CreateScope("accept-order");
        }

        public void Accept(StateMachineVisitor visitor)
        {
            visitor.Visit(this);
        }

        public async Task Execute(BehaviorContext<OrderState, IOrderAccepted> context, Behavior<OrderState, IOrderAccepted> next)
        {
            Console.WriteLine("Hello, World. Order is {0}", context.Data.OrderId);

            var consumeContext = context.GetPayload<ConsumeContext>();

            var sendEndpoint = await consumeContext.GetSendEndpoint(new Uri("queue:fulfill-order"));

            await sendEndpoint.Send<IFulfillOrder>(new
            {
                context.Data.OrderId,
                //context.Instance.CustomerNumber,
                //context.Instance.PaymentCardNumber,
            });

            await next.Execute(context).ConfigureAwait(false);
        }

        public Task Faulted<TException>(BehaviorExceptionContext<OrderState, IOrderAccepted, TException> context, Behavior<OrderState, IOrderAccepted> next)
            where TException : Exception
        {
            return next.Faulted(context);
        }
    }
}