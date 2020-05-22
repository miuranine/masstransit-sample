using System;
using System.Threading.Tasks;
using MassTransit;
using Microsoft.Extensions.Logging;
using Sample.Contracts;

namespace Sample.Common.Consumers
{
    public class SubmitOrderConsumer : IConsumer<ISubmitOrder>
    {
        private readonly ILogger<SubmitOrderConsumer> _logger;

        public SubmitOrderConsumer(ILogger<SubmitOrderConsumer> logger)
        {
            _logger = logger;
        }
        
        public async Task Consume(ConsumeContext<ISubmitOrder> context)
        {
            _logger?.Log(LogLevel.Debug, "SubmitOrderConsumer: {CustomerNumber}", context.Message.CustomerNumber);

            if (context.Message.CustomerNumber.Contains("TEST"))
            {
                if (context.RequestId != null)
                    await context.RespondAsync<IOrderSubmissionRejected>(new
                    {
                        InVar.Timestamp,
                        context.Message.OrderId,
                        context.Message.CustomerNumber,
                        Reason = $"Test Customer cannot submit orders: {context.Message.CustomerNumber}"
                    });

                return;
            }

            MessageData<string> notes = context.Message.Notes;
            if (notes.HasValue)
            {
                string notesValue = await notes.Value;

                Console.WriteLine("NOTES: {0}", notesValue);
            }

            /*await context.Publish<IOrderSubmitted>(new
            {
                context.Message.OrderId,
                context.Message.Timestamp,
                context.Message.CustomerNumber,
                context.Message.PaymentCardNumber,
                context.Message.Notes
            });*/

            if (context.RequestId != null)
                await context.RespondAsync<IOrderSubmissionAccepted>(new
                {
                    InVar.Timestamp,
                    context.Message.OrderId,
                    CustomerNumber = "Test Response"
                });
        }
    }
}