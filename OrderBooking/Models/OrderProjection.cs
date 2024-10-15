using EventStorage.Projections;
using OrderBooking.Events;

namespace OrderBooking.Models;

public class OrderProjection : IProjection<Order>
{
    public static Order Project(Order order, OrderPlaced orderPlaced) =>
        order with { SourceId = orderPlaced.SourceId, Status = OrderStatus.Placed };
    public static Order Project(Order order, OrderConfirmed orderConfirmed) =>
        order with { SourceId = orderConfirmed.SourceId, Status = OrderStatus.Confirmed };
}