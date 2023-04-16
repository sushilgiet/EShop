//using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using ProductCatalog.API.Extensions;
using ProductCatalog.Application;
using ProductCatalog.Persistance;
using Microsoft.Identity.Web;
using ProductCatalog.API;
using Microsoft.Extensions.Azure;
using Azure.Identity;
using System.Net;
using Azure.Security.KeyVault.Secrets;
using System.Reflection;
using MediatR;
var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new Microsoft.OpenApi.Models.OpenApiInfo()
    {
        Title = "Catalog API Demo",
        Version = "v1",
        Description = "The is a demo of how to use Swagger in Catalog API"
    });
});
builder.Services.ConfigurePersistenceServices(builder.Configuration);
builder.Services.ConfigureApplicationServices(builder.Configuration);
builder.Services.AddMicrosoftIdentityWebApiAuthentication(builder.Configuration, "AzureAd");

builder.Configuration.AddAzureKeyVault(
new Uri(builder.Configuration["KeyVault:VaultUri"]),
new ClientSecretCredential(builder.Configuration["AzureAd:TenantId"], builder.Configuration["AzureAd:ClientId"], builder.Configuration["AzureAd:ClientSecret"]));

builder.Services.AddMediatR(Assembly.GetExecutingAssembly());
var app = builder.Build();

app.ConfigureExceptionHandler(app.Services.GetService<ILogger>());
// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
}
app.UseSwagger().UseSwaggerUI(c => {
    c.SwaggerEndpoint("/swagger/v1/swagger.json", "My API V1");
});
app.UseAuthentication();
app.UseAuthorization();
app.UseHttpsRedirection();
app.MapControllers();
using (IServiceScope scope = app.Services.CreateScope())
{
   var services = scope.ServiceProvider;
   var context = services.GetRequiredService<ProductCatalogContext>();
   context.Database.EnsureCreated();
   context.Database.Migrate();
   ProductCatalogSeed.SeedAsync(context).Wait();

   // var secretContext = services.GetRequiredService<SecretManager>();
   // SecretClient secretClient = new SecretClient(new Uri(builder.Configuration["KeyVault:VaultUri"]), new DefaultAzureCredential());
   // var dbconn = secretClient.GetSecretAsync("ProductCatalogContext").Result;
    //var dbconn= secretContext.GetSecretAsync("ProductCatalogContext").Result;
}


app.Run();
