using EventStorage.Events;

namespace OrderBooking.Events;

public record OrderConfirmed() : SourcedEvent;