using AsyncHandler.EventSourcing.Events;
using OrderBooking.Commands;

namespace OrderBooking.Events;

public record OrderPlaced(PlaceOrder Command) : SourceEvent
{
    public string OrderId => Guid.NewGuid().ToString();
    public string ProductName => Command.ProductName;
    public int Quantity => Command.Quantity;
    public string UserId => Command.UserId;
}