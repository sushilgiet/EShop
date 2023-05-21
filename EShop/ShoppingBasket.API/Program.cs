using EventBus.Abstractions;
using EventBus.Implementations;
using StackExchange.Redis;
using Microsoft.Identity.Web;
using Azure.Identity;
using ShoppingBasket.API.Core.Interfaces;
using ShoppingBasket.API.Handlers;
using System.Reflection;
using MediatR;
using ShoppingBasket.API.Events;
using ShoppingBasket.API.Infrastructure;

var builder = WebApplication.CreateBuilder(args);
builder.Configuration.AddAzureAppConfiguration(options =>
                  options.Connect(builder.Configuration["AppConfig:Endpoint"]).ConfigureKeyVault(kv =>
                  {
                      kv.SetCredential(new ClientSecretCredential(
    tenantId: "47f6b62e-32c6-49c5-b373-6243a85929c0",
    clientId: "60a39ad8-1928-462a-934d-ea248f795538",
    clientSecret: "ywh8Q~EXVTCnfeoClH~FOtu1M_OZrA0dobN3fdr-")
);
                  }));
builder.Services.AddMicrosoftIdentityWebApiAuthentication(builder.Configuration, "BasketAPI:AzureAd");
// Add services to the container.
builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new Microsoft.OpenApi.Models.OpenApiInfo()
    {
        Title = "Basket API",
        Version = "v1",
        Description = "Basket API"
    });
});


builder.Services.AddSingleton<ConnectionMultiplexer>(sp =>
{
    var configuration = ConfigurationOptions.Parse(builder.Configuration["RedisConnectionString"], true);
    //configuration.ResolveDns = true;
    // OR
    configuration.ResolveDns = false; // If Azure Redis Cache is used.
                                      // configuration.AbortOnConnectFail = false;
    return ConnectionMultiplexer.Connect(configuration);
});
builder.Services.AddSingleton<IEventBus>(x =>
{
    Console.WriteLine(builder.Configuration["TopicName"]);
    var bus = new AzureEventBus(builder.Configuration["ServiceBusConnectionString"], builder.Configuration["TopicName"], builder.Configuration["SubscriptionName"]);
    ProductPriceChangedEventHandlers handler = new ProductPriceChangedEventHandlers(x.GetRequiredService<IBasketRepository>());
    bus.Subscribe<ProductPriceChangedEvent, ProductPriceChangedEventHandlers>(handler);
    return bus;
});
builder.Services.AddTransient<IBasketRepository, RedisBasketRepository>();
builder.Services.AddMediatR(Assembly.GetExecutingAssembly());
var app = builder.Build();

// Configure the HTTP request pipeline.


app.UseSwagger().UseSwaggerUI(c =>
{
    c.SwaggerEndpoint("/swagger/v1/swagger.json", "Basket API V1");
});
app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

using (IServiceScope scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    var context = services.GetRequiredService<IEventBus>();

}


app.Run();

