var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Redis configure
builder.Services.AddSingleton<ConnectionMultiplexer>(sp =>
{
    var configuration = ConfigurationOptions.Parse("localhost", true);
    return ConnectionMultiplexer.Connect(configuration);
});

// Add MediatR
builder.Services.AddMediatR(Assembly.GetExecutingAssembly());

// Add Rabbit MQ
builder.Services.AddSingleton<IRabbitMQPersistentConnection>(sp =>
{
    var logger = sp.GetRequiredService<ILogger<DefaultRabbitMQPersistentConnection>>();

    var factory = new ConnectionFactory()
    {
        HostName = builder.Configuration["EventBusConnection"],
        DispatchConsumersAsync = true
    };

    var retryCount = 5;

    return new DefaultRabbitMQPersistentConnection(factory, logger, retryCount);
});

builder.Services.AddSingleton<IEventBus, EventBusRabbitMQ>(sp =>
{
    var subscriptionClientName = builder.Configuration["SubscriptionClientName"];
    var rabbitMQPersistentConnection = sp.GetRequiredService<IRabbitMQPersistentConnection>();
    var iLifetimeScope = sp.GetRequiredService<ILifetimeScope>();
    var logger = sp.GetRequiredService<ILogger<EventBusRabbitMQ>>();
    var eventBusSubcriptionsManager = sp.GetRequiredService<IEventBusSubscriptionsManager>();

    var retryCount = 5;

    return new EventBusRabbitMQ(rabbitMQPersistentConnection, logger, iLifetimeScope, eventBusSubcriptionsManager, subscriptionClientName, retryCount);
});

builder.Services.AddSingleton<IEventBusSubscriptionsManager, InMemoryEventBusSubscriptionsManager>();

// Autofac configure
builder.Host.UseServiceProviderFactory(new AutofacServiceProviderFactory());

builder.Services.AddCors(options =>
{
    options.AddPolicy("CorsPolicy",
        builder => builder
        .AllowAnyOrigin()
        .AllowAnyMethod()
        .AllowAnyHeader());
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseCors("CorsPolicy");

#region Basket API
const string Basket_Tag = "Basket";

app.MapGet("/api/basket/{customerId}", async (string customerId, [FromServices] IMediator mediator) =>
{
    var basket = await mediator.Send(new GetBasketByBuyerIdQuery { BuyerId = customerId });
    return Results.Ok(basket ?? new CustomerBasket(customerId));
}).WithTags(Basket_Tag);

app.MapPost("/api/basket", async (CustomerBasket basket, [FromServices] IMediator mediator) =>
{ 
    return Results.Ok(await mediator.Send(new UpdateBasketCommand { Basket = basket }));
}).WithTags(Basket_Tag);

app.MapDelete("/api/basket/{customerId}", async (string customerId, [FromServices] IMediator mediator) =>
{
    var deleted = await mediator.Send(new DeleteBasketByBuyerIdCommand { BuyerId = customerId });
    if (!deleted) return Results.NotFound();
    return Results.NoContent();
}).WithTags(Basket_Tag);
#endregion

app.MapPost("/api/checkout", async (BasketCheckout basketCheckout, [FromHeader(Name = "x-requestid")] string requestId, [FromServices] IMediator mediator) =>
{
    var checkout = await mediator.Send(new CheckoutCommand
    {
        BasketCheckout = basketCheckout,
        Id = requestId,
    });

    if (!checkout) return Results.BadRequest();

    return Results.Accepted();
}).WithTags(Basket_Tag);

app.Run();