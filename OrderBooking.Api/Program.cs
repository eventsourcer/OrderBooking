using AsyncHandler.EventSourcing;
using AsyncHandler.EventSourcing.Configuration;
using AsyncHandler.EventSourcing.Projections;
using AsyncHandler.EventSourcing.Repositories;
using OrderBooking;
using OrderBooking.Commands;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var connectionString = builder.Configuration["mssqlsecret"] ?? throw new Exception("No connection defined");

builder.Services.AddAsyncHandler(asynchandler =>
{
    asynchandler.AddEventSourcing(source =>
    {
        source.SelectEventSource(EventSources.SqlServer, connectionString)
        .AddProjection<Projection>(ProjectionMode.Async);
    })
    .EnableTransactionalOutbox(MessageBus.Kafka, "");
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
async(IEventSource<OrderBookingAggregate> eventSource, PlaceOrder command) =>
{
    var aggregate = await eventSource.CreateOrRestore();
    aggregate.PlaceOrder(command);

    await eventSource.Commit(aggregate);

    return Results.Ok(aggregate.SourceId);
});
app.MapPost("api/confirmorder/{orderId}", 
async(IEventSource<OrderBookingAggregate> eventSource, string orderId, ConfirmOrder command) =>
{
    var aggregate = await eventSource.CreateOrRestore(orderId);
    aggregate.ConfirmOrder(command);

    await eventSource.Commit(aggregate);
});

app.Run();