using MassTransit;
using MassTransitDemo.Messages;

namespace MassTransitDemo.Consumer;

public class ProcessUploadedItemConsumer : IConsumer<ProcessItemCommand>
{
    public async Task Consume(ConsumeContext<ProcessItemCommand> context)
    {
        //do something
        var message = context.Message;
        Console.WriteLine($"item {message.Item.Name} proceed");
        // Forward each item to the next consumer
        await context.Publish(
            new CreateItemCommand
            {
                Item = message.Item,
                At = DateTime.Now
            });
    }
}