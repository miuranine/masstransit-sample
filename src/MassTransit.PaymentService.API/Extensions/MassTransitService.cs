using GreenPipes;
using MassTransit.PaymentService.API.Consumers;
using Microsoft.Extensions.DependencyInjection;

namespace MassTransit.PaymentService.API.Extensions
{
    public static class MassTransitService
    {
        public static void AddMassTransitService(this IServiceCollection services)
        {
            // Consumer dependencies should be scoped
            services.AddScoped<OrderConsumer>();

            services.AddMassTransit(x =>
            {
                x.AddConsumer<OrderConsumer>();

                x.AddBus(context => Bus.Factory.CreateUsingRabbitMq(cfg =>
                {
                    // configure health checks for this bus instance
                    cfg.UseHealthCheck(context);

                    cfg.Host("amqp://127.0.0.1:5672", h =>
                    {
                        h.Username("user");
                        h.Password("*******");
                    });

                    cfg.ReceiveEndpoint("place-order", ep =>
                    {
                        ep.PrefetchCount = 16;
                        ep.UseMessageRetry(r => r.Interval(2, 100));

                        ep.ConfigureConsumer<OrderConsumer>(context);
                    });
                }));
            });

            services.AddMassTransitHostedService();
        }
    }
}