using AsyncHandler.EventSourcing.Events;

namespace OrderBookingr.Events;

public record OrderPlaced() : SourceEvent;