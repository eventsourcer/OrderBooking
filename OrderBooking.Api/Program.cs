using EventStorage;
using EventStorage.Configurations;
using EventStorage.Projections;
using EventStorage.Repositories;
using OrderBooking;
using OrderBooking.Commands;
using OrderBooking.Models;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var connectionString = builder.Configuration["postgresqlsecret"]??
    throw new Exception("No connection defined");

builder.Services.AddEventStorage(eventstorage =>
{
    eventstorage.Schema = "es";
    
    eventstorage.AddEventSource(eventsource =>
    {
        eventsource.SelectEventStorage(EventStore.PostgresSql, connectionString)
        .Project<OrderProjection>(ProjectionMode.Runtime);
    });
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.MapPost("api/placeorder", 
async(IEventStorage<OrderBookingAggregate> eventSource, PlaceOrder command) =>
{
    var aggregate = await eventSource.CreateOrRestore();
    aggregate.PlaceOrder(command);

    await eventSource.Commit(aggregate);

    return Results.Ok(aggregate.SourceId);
});
app.MapPost("api/confirmorder/{orderId}", 
async(IEventStorage<OrderBookingAggregate> eventSource, string orderId, ConfirmOrder command) =>
{
    var aggregate = await eventSource.CreateOrRestore(orderId);
    aggregate.ConfirmOrder(command);

    await eventSource.Commit(aggregate);
});

app.MapGet("api/getorder/{orderId}",
async(IEventStorage<OrderBookingAggregate> eventStorage, string orderId) =>
{
    var order = await eventStorage.Project<OrderProjection>(orderId);
    return Results.Ok(order);
});
app.MapGet("api/getorder/{orderId}",
async(IEventStorage<OrderBookingAggregate> eventStorage, string orderId) =>
{
    var order = await eventStorage.Project<Order>(orderId);
    return Results.Ok(order);
});

app.Run();