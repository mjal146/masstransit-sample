using MassTransit;
using MassTransitDemo.Messages;

namespace MassTransitDemo.Consumer;

public class SuccessConsumer : IConsumer<ItemSavedEvent>
{
    public Task Consume(ConsumeContext<ItemSavedEvent> context)
    {
        // Log the success message
        Console.WriteLine(context.Message.Message);
        return Task.CompletedTask;
    }
}