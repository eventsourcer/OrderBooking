using AsyncHandler.EventSourcing.Events;

namespace OrderBooking.Events;

public record OrderConfirmed() : SourcedEvent;