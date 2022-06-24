var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

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
builder.Host.ConfigureContainer<ContainerBuilder>(builder =>
{
    builder.RegisterModule(new MediatorModule());
});

// Mysql configure
builder.Services.AddDbContext<OrderingContext>(options =>
{
    var connectionString = builder.Configuration.GetConnectionString("OrderingDB");
    options.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString), options => 
    {
        options.MigrationsAssembly("OrderingApi");
    });
});

builder.Services.AddSingleton<IOrderRepository, OrderRepository>();

var app = builder.Build();

// Rabbit MQ subscribe configure
var eventBus = app.Services.GetRequiredService<IEventBus>();
eventBus.Subscribe<UserCheckoutAcceptedIntegrationEvent, IIntegrationEventHandler<UserCheckoutAcceptedIntegrationEvent>>();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.Run();