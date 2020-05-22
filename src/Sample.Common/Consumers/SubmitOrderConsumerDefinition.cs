using GreenPipes;
using MassTransit;
using MassTransit.ConsumeConfigurators;
using MassTransit.Definition;

namespace Sample.Common.Consumers
{
    public class SubmitOrderConsumerDefinition :
        ConsumerDefinition<SubmitOrderConsumer>
    {
        public SubmitOrderConsumerDefinition()
        {
            ConcurrentMessageLimit = 20;
        }

        protected override void ConfigureConsumer(IReceiveEndpointConfigurator endpointConfigurator,
            IConsumerConfigurator<SubmitOrderConsumer> consumerConfigurator)
        {
            endpointConfigurator.UseMessageRetry(r => r.Interval(3, 1000));
            endpointConfigurator.UseInMemoryOutbox(); 
        }
    }
}