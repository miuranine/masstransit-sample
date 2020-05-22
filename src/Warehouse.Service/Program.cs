using System;
using System.Threading.Tasks;
using MassTransit;
using MassTransit.Definition;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Sample.Common.Consumers;
using Sample.Common.CourierActivities;
using Sample.Common.StateMachine;
using Sample.Common.StateMachine.OrderStateMachineActivities;
using Serilog;
using Serilog.Events;
using Warehouse.Components.Consumers;
using Warehouse.Contracts;

namespace Warehouse.Service
{
    class Program
    {
        static async Task Main(string[] args)
        {
            Log.Logger = new LoggerConfiguration()
                .MinimumLevel.Debug()
                .MinimumLevel.Override("Microsoft", LogEventLevel.Information)
                .Enrich.FromLogContext()
                .WriteTo.Console()
                .CreateLogger();
            
            var builder = new HostBuilder()
                .ConfigureAppConfiguration((hostingContext, config) =>
                {
                    config.AddJsonFile("appsettings.json", true);
                    config.AddEnvironmentVariables();
                })
                .ConfigureServices((hostContext, services) =>
                {
                    services.AddScoped<AcceptOrderActivity>();
                    
                    services.TryAddSingleton(KebabCaseEndpointNameFormatter.Instance);
                    services.AddMassTransit(x =>
                    {
                        x.AddActivitiesFromNamespaceContaining<AllocateInventoryConsumer>();
    
                        x.AddBus(ConfigureBus);
                    });
                    
                    services.AddSingleton<IHostedService, MassTransitConsoleHostedService>();
                })
                .ConfigureLogging((hostingContext, logging) =>
                {
                    logging.AddSerilog(dispose: true);
                    logging.AddConfiguration(hostingContext.Configuration.GetSection("Logging"));
                });
            
            await builder.RunConsoleAsync();

            Log.CloseAndFlush();
        }

        private static IBusControl ConfigureBus(IRegistrationContext<IServiceProvider> provider)
        {
            return Bus.Factory.CreateUsingRabbitMq(cfg =>
            {
                cfg.ConfigureEndpoints(provider);
                cfg.Host(new Uri("amqp://localhost:5672"), host =>
                {
                    host.Username("user");
                    host.Password("g4hPnkGbKj");
                });
            });
        }
    }
}