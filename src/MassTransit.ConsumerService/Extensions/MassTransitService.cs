using Microsoft.Extensions.DependencyInjection;

namespace MassTransit.ConsumerService.Extensions
{
    public static class MassTransitService
    {
        public static void AddMassTransitService(this IServiceCollection services)
        {
            services.AddMassTransit(cfg =>
            {
            });
        }
    }
}