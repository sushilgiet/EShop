using Azure.Identity;
using Microsoft.Identity.Web;
var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddMicrosoftIdentityWebApiAuthentication(builder.Configuration, "AzureAd");

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new Microsoft.OpenApi.Models.OpenApiInfo()
    {
        Title = "Notification API",
        Version = "v1",
        Description = "Swagger in Notification API"
    });
});
builder.Configuration.AddAzureKeyVault(
     new Uri(builder.Configuration["KeyVault:VaultUri"]),
     new ClientSecretCredential(builder.Configuration["AzureAd:TenantId"], builder.Configuration["AzureAd:ClientId"], builder.Configuration["AzureAd:ClientSecret"]));
var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
app.UseSwagger().UseSwaggerUI(c => {
    c.SwaggerEndpoint("/swagger/v1/swagger.json", "Notification API");
});

app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
