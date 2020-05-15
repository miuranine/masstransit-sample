using System;
using GreenPipes;
using MassTransit.ConsumerService.Consumers;
using MassTransit.ConsumerService.States;
using MassTransit.Saga;
using Microsoft.Extensions.DependencyInjection;

namespace MassTransit.ConsumerService.Extensions
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
                        h.Password("g4hPnkGbKj");
                    });

                    cfg.ReceiveEndpoint("pre-order", ep =>
                    {
                        ep.PrefetchCount = 16;
                        //Redelivery
                        ep.UseScheduledRedelivery(r => r.Intervals(TimeSpan.FromMinutes(5), TimeSpan.FromMinutes(15), TimeSpan.FromMinutes(30)));
                        ep.UseMessageRetry(r =>
                        {
                            //Immediate = Retry immediately, up to the retry limit.
                            r.Immediate(5);
                            //Interval = Retry after a fixed delay, up to the retry limit.
                            r.Interval(2, 100);
                        });
                        ep.UseInMemoryOutbox();
                        
                        ep.ConfigureConsumer<OrderConsumer>(context);
                        
                        //If the container registration is not being used, the InMemory saga repository can be created manually and specified on receive endpoint.
                        //ep.StateMachineSaga(new OrderStateMachine(), new InMemorySagaRepository<OrderState>());
                    });
                }));
            });

            services.AddMassTransitHostedService();
        }
    }
}