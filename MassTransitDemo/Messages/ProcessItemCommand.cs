using MassTransitDemo.Model;

namespace MassTransitDemo.Messages;

public class ProcessItemCommand
{
    public Item Item { get; set; }
    public DateTime At { get; set; }
}