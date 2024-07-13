using MassTransitDemo.Model;

namespace MassTransitDemo.Messages;

public class CreateItemCommand
{
    public Item Item { get; set; }
    public DateTime At { get; set; }
}