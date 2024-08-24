using AsyncHandler.EventSourcing.AggregateRoot;

namespace OrderBooking;

public class OrderBookingAggregate(int orderId) : AggregateRoot(orderId)
{
    public OrderStatus Status { get; private set; }
}
