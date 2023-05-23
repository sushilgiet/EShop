using Azure.Identity;
using Microsoft.Identity.Web;
var builder = WebApplication.CreateBuilder(args);


builder.Services.AddMicrosoftIdentityWebApiAuthentication(builder.Configuration, "NotificationAPI:AzureAd");
builder.Services.AddControllers();
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
//builder.Configuration.AddAzureKeyVault(
//     new Uri(builder.Configuration["KeyVault:VaultUri"]),
//     new ClientSecretCredential(builder.Configuration["AzureAd:TenantId"], builder.Configuration["AzureAd:ClientId"], builder.Configuration["AzureAd:ClientSecret"]));
builder.Configuration.AddAzureAppConfiguration(options =>
                 options.Connect(builder.Configuration["AppConfig:Endpoint"]).ConfigureKeyVault(kv =>
                 {
                     kv.SetCredential(new ClientSecretCredential(
    tenantId: "47f6b62e-32c6-49c5-b373-6243a85929c0",
    clientId: "60a39ad8-1928-462a-934d-ea248f795538",
    clientSecret: "ywh8Q~EXVTCnfeoClH~FOtu1M_OZrA0dobN3fdr-")
);
                 }));

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

//app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
