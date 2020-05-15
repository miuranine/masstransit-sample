using System;
using System.Threading.Tasks;
using MassTransit.Common.Contracts;

namespace MassTransit.ProducerService.API.Services
{
    public class SubmitOrderConsumer : IConsumer<ISubmitOrder>
    {
        public async Task Consume(ConsumeContext<ISubmitOrder> context)
        {
            if (context.Message.CustomerNumber.Contains("TEST"))
            {
                await context.RespondAsync<IOrderSubmissionRejected>(new
                    {
                        context.Message.CreatedDate,
                        context.Message.OrderId,
                        context.Message.CustomerNumber,
                        Reason = $"Test Customer cannot submit orders: {context.Message.CustomerNumber}"
                    });

                return;
            }
            
            await context.RespondAsync<IOrderSubmissionAccepted>(
                new
                {
                    context.Message.CreatedDate,
                    context.Message.OrderId,
                    context.Message.CustomerNumber,
                });
        }
    }
}