using EventBus.Abstractions;
using EventBus.Implementations;
using Microsoft.Extensions.Hosting;
using ShoppingBasket.API.IntegrationEventHandlers;
using ShoppingBasket.IntegrationEvents;
using ShoppingBasketAPI.Repository;
using StackExchange.Redis;
using Microsoft.Identity.Web;
using Azure.Identity;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Azure;
using System.Security.Cryptography.X509Certificates;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddMicrosoftIdentityWebApiAuthentication(builder.Configuration, "AzureAd");
// Add services to the container.
builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();


//builder.Configuration.AddAzureKeyVault(new Uri($"https://{builder.Configuration["KeyVaultName"]}.vault.azure.net/"),
//        new DefaultAzureCredential());
//builder.Configuration.AddAzureKeyVault(
//    new Uri($"https://{builder.Configuration["KeyVaultName"]}.vault.azure.net/"),
//    new DefaultAzureCredential(new DefaultAzureCredentialOptions
//    {
//        ManagedIdentityClientId = builder.Configuration["AzureADManagedIdentityClientId"]
//    }));
builder.Configuration.AddAzureKeyVault(
        new Uri($"https://{builder.Configuration["KeyVaultName"]}.vault.azure.net/"),
     new ClientSecretCredential(builder.Configuration["AzureAd:TenantId"], builder.Configuration["AzureAd:ClientId"], builder.Configuration["AzureAd:ClientSecret"]));

builder.Services.AddSingleton<ConnectionMultiplexer>(sp =>
{
    var configuration = ConfigurationOptions.Parse(builder.Configuration["RedisConnectionString"], true);
    //configuration.ResolveDns = true;
    // OR
    configuration.ResolveDns = false; // If Azure Redis Cache is used.
    configuration.AbortOnConnectFail = false;
    return ConnectionMultiplexer.Connect(configuration);
});
builder.Services.AddSingleton<IEventBus>(x =>
{
    Console.WriteLine(builder.Configuration["TopicName"]);
    var bus = new AzureEventBus(builder.Configuration["ServiceBusConnectionString"], builder.Configuration["TopicName"], builder.Configuration["SubscriptionName"]);
    ProductChangedIntegrationEventHandlers handler = new ProductChangedIntegrationEventHandlers(x.GetRequiredService<IBasketRepository>());
    bus.Subscribe<ProductPriceUpdatedIntegrationEvent, ProductChangedIntegrationEventHandlers>(handler);
    return bus;
});
builder.Services.AddTransient<IBasketRepository,RedisBasketRepository>();
var app = builder.Build();

// Configure the HTTP request pipeline.


app.UseSwagger().UseSwaggerUI(c => {
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

