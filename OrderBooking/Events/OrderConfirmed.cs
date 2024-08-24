using AsyncHandler.EventSourcing.Events;

namespace OrderBookingr.Events;

public record OrderConfirmed() : SourceEvent;