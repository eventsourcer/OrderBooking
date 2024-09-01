using AsyncHandler.EventSourcing;
using AsyncHandler.EventSourcing.Events;
using AsyncHandler.EventSourcing.Extensions;
using OrderBooking.Commands;
using OrderBooking.Events;

namespace OrderBooking;

public class OrderBookingAggregate(long orderId) : AggregateRoot(orderId)
{
    public OrderStatus OrderStatus { get; private set; }
    protected override void Apply(SourceEvent e)
    {
        this.InvokeApply(e);
    }
    public void Apply(OrderPlaced e)
    {
        OrderStatus = OrderStatus.Placed;
    }
    public void Apply(OrderConfirmed e)
    {
        OrderStatus = OrderStatus.Confirmed;
    }
    public void PlaceOrder(PlaceOrder command)
    {
        if(OrderStatus == OrderStatus.Placed)
            return;
        RaiseEvent(new OrderPlaced(command));
    }
    public void ConfirmOrder(ConfirmOrder command)
    {
        if( OrderStatus == OrderStatus.Confirmed)
            return;
        RaiseEvent(new OrderConfirmed());
    }
}
