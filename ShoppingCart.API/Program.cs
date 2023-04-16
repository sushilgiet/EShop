using EventBus.Abstractions;
using EventBus.Implementations;
using Microsoft.Extensions.Hosting;
using ShoppingBasket.API.IntegrationEventHandlers;
using ShoppingBasket.IntegrationEvents;
using ShoppingBasketAPI.Repository;
using StackExchange.Redis;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddSingleton<ConnectionMultiplexer>(sp =>
{
    var configuration = ConfigurationOptions.Parse(builder.Configuration["RedisConnectionString"], true);
    configuration.ResolveDns = true;
    // OR
    //configuration.ResolveDns = false; // If Azure Redis Cache is used.
    configuration.AbortOnConnectFail = false;
    return ConnectionMultiplexer.Connect(configuration);
});
builder.Services.AddSingleton<IEventBus>(x =>
{
    var bus = new AzureEventBus(builder.Configuration["ServiceBusConnectionString"], "eshop-topic", "basket");
    ProductChangedIntegrationEventHandlers handler = new ProductChangedIntegrationEventHandlers(x.GetRequiredService<IBasketRepository>());
    bus.Subscribe<ProductPriceUpdatedIntegrationEvent, ProductChangedIntegrationEventHandlers>(handler);
    return bus;
});
builder.Services.AddTransient<IBasketRepository,RedisBasketRepository>();
var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseAuthorization();
app.UseHttpsRedirection();
app.MapControllers();

using (IServiceScope scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    var context = services.GetRequiredService<IEventBus>();
    
}


app.Run();

