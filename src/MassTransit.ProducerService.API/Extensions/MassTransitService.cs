
using GreenPipes;
using Microsoft.Extensions.DependencyInjection;

namespace MassTransit.ProducerService.API.Extensions
{
    public static class MassTransitService
    {
        public static void AddMassTransitService(this IServiceCollection services)
        {
            // Consumer dependencies should be scoped
            //services.AddScoped<SomeConsumerDependency>();

            services.AddMassTransit(x =>
            {
                //x.AddConsumer<OrderConsumer>();

                x.AddBus(context => Bus.Factory.CreateUsingRabbitMq(cfg =>
                {
                    // configure health checks for this bus instance
                    cfg.UseHealthCheck(context);

                    cfg.Host("amqp://127.0.0.1:5672", h =>
                    {
                        h.Username("user");
                        h.Password("g4hPnkGbKj");
                    });
                }));
                
                //x.AddSagaStateMachine<OrderStateMachine, OrderState>().InMemoryRepository();
            });

            services.AddMassTransitHostedService();
        }
    }
}