using EventStorage.Events;
using OrderBooking.Commands;

namespace OrderBooking.Events;

public record OrderRejected(PlaceOrder Command) : SourcedEvent;