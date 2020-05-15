using System;
using MassTransit.ConsumerService.Extensions;
using Microsoft.Extensions.DependencyInjection;

namespace MassTransit.ConsumerService
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Service started.");

            var services = new ServiceCollection();
            services.AddMassTransitService();

            Console.ReadLine();
        }
    }
}