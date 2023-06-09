
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using ProductCatalog.Application;
using ProductCatalog.Persistance;
using Microsoft.Identity.Web;
using ProductCatalog.API;
using Microsoft.Extensions.Azure;
using Azure.Identity;
using System.Net;
using System.Reflection;
using MediatR;
using ProductCatalog.API.Extensions;
using Microsoft.AspNetCore.Hosting;

var builder = WebApplication.CreateBuilder(args);

builder.Configuration.AddAzureAppConfiguration(options =>
               options.Connect(builder.Configuration["AppConfig:EndPoint"]).ConfigureKeyVault(kv =>
               {
                   kv.SetCredential(new ClientSecretCredential(
    tenantId: "47f6b62e-32c6-49c5-b373-6243a85929c0",
    clientId: "60a39ad8-1928-462a-934d-ea248f795538",
    clientSecret: "ywh8Q~EXVTCnfeoClH~FOtu1M_OZrA0dobN3fdr-")
);
               }));
builder.Services.AddMicrosoftIdentityWebApiAuthentication(builder.Configuration, "Catalog:AzureAd");


builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new Microsoft.OpenApi.Models.OpenApiInfo()
    {
        Title = "Catalog API",
        Version = "v1",
        Description = "Swagger in Catalog API"
    });
});
builder.Services.ConfigurePersistenceServices(builder.Configuration);
builder.Services.ConfigureApplicationServices(builder.Configuration);



builder.Services.AddMediatR(Assembly.GetExecutingAssembly());
var app = builder.Build();
app.ConfigureExceptionHandler(app.Services.GetService<ILogger>());
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
}
app.UseSwagger().UseSwaggerUI(c => {
    c.SwaggerEndpoint("/swagger/v1/swagger.json", "My API V1");
});
app.UseAuthentication();
app.UseAuthorization();
//app.UseHttpsRedirection();
app.MapControllers();




























//using (IServiceScope scope = app.Services.CreateScope())
//{
//   var services = scope.ServiceProvider;
//   var context = services.GetRequiredService<ProductCatalogContext>();
//   context.Database.EnsureCreated();
//   context.Database.Migrate();
//   ProductCatalogSeed.SeedAsync(context).Wait();
//}


app.Run();
