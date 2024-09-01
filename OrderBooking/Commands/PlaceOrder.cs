namespace OrderBooking.Commands;

public record PlaceOrder(string ProductName, int Quantity, string UserId);