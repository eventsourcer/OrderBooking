namespace OrderBooking.Models;

public record Order
{
    public object? SourceId { get; set; }
    public OrderStatus Status { get; set; }
    public long Version { get; set; }
}