using MassTransitDemo.Model;

namespace MassTransitDemo.Messages;

public class ItemSavedEvent
{
    public bool IsSaved { get; set; }
    public string Message { get; set; }
    public Item Item { get; set; }
    public DateTime At { get; set; }
}