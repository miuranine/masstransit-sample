
using GreenPipes;
using Microsoft.Extensions.DependencyInjection;

namespace MassTransit.OrderService.API.Extensions
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
                        h.Password("******");
                    });

                    /*cfg.ReceiveEndpoint("submit-order", ep =>
                    {
                        ep.PrefetchCount = 16;
                        ep.UseMessageRetry(r => r.Interval(2, 100));

                        ep.ConfigureConsumer<OrderConsumer>(context);
                    });*/
                }));
            });

            services.AddMassTransitHostedService();
        }
    }
}