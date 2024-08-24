using System.Reflection;
using AsyncHandler.EventSourcing.AggregateRoot;
using AsyncHandler.EventSourcing.Events;
using AsyncHandler.EventSourcing.Extensions;
using OrderBooking.Commands;
using OrderBookingr.Events;

namespace OrderBooking;

public class OrderBookingAggregate(int orderId) : AggregateRoot(orderId)
{
    public OrderStatus Status { get; private set; }
    protected override void Apply(SourceEvent e)
    {
        var apply = GetType().GetApply(e);
        try
        {
            apply.Invoke(this, [e]);
        }
        catch(TargetInvocationException)
        {
            throw;
        }
        base.Apply(e);
    }
    private void Apply(OrderPlaced e)
    {
        Status = OrderStatus.Placed;
    }
    private void Apply(OrderConfirmed e)
    {
        Status = OrderStatus.Confirmed;
    }
    public void PlaceOrder(PlaceOrder placeOrder)
    {
        RaiseEvent(new OrderPlaced());
    }
}
