using MassTransit;
using MassTransitDemo.Data;
using MassTransitDemo.Messages;

namespace MassTransitDemo.Consumer;

public class SaveToDatabaseConsumer : IConsumer<CreateItemCommand>
{
    private readonly ApplicationDbContext _context;
    private bool _isSaved;

    public SaveToDatabaseConsumer(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task Consume(ConsumeContext<CreateItemCommand> context)
    {
        _context.Items.Add(context.Message.Item);
        _isSaved = await _context.SaveChangesAsync() > 0;

        // Send a success event to the next consumer
        await context.Publish(new ItemSavedEvent
        {
            IsSaved = _isSaved,
            Message = $"Item {context.Message.Item.Name} with {context.Message.Item.Id} saved " + _isSaved,
            Item = context.Message.Item,
            At = DateTime.Now
        });
    }

}