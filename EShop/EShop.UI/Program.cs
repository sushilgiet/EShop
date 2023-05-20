using EShop.UI.Services;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Identity.Web;
using Microsoft.Identity.Web.UI;
using Polly;
using Microsoft.AspNetCore.Authentication;
using Azure.Identity;

var builder = WebApplication.CreateBuilder(args);
builder.Configuration.AddAzureAppConfiguration(options =>
               options.Connect(builder.Configuration["AppConfig:Endpoint"]).ConfigureKeyVault(kv =>
               {
                   kv.SetCredential(new DefaultAzureCredential());
               }));
builder.Services.AddControllersWithViews();
builder.Services.AddTransient<ICatalogService, CatalogService>();
builder.Services.AddTransient<IBasketService, BasketService>();
builder.Services.AddAuthentication(OpenIdConnectDefaults.AuthenticationScheme).AddMicrosoftIdentityWebApp(builder.Configuration.GetSection("EShop:AzureAd"));
   
builder.Services.AddAuthorization(options =>

{
    options.FallbackPolicy = options.DefaultPolicy;
});
builder.Services.AddRazorPages().AddMvcOptions(options =>
{
    var policy = new AuthorizationPolicyBuilder()
                  .RequireAuthenticatedUser()
                  .Build();
    options.Filters.Add(new AuthorizeFilter(policy));
}).AddMicrosoftIdentityUI();
builder.Services.Configure<MvcOptions>(options =>
{
    options.Filters.Add(new RequireHttpsAttribute());
});
builder.Services.AddHttpClient("EshopHttpClient")
    .AddPolicyHandler(Policy.TimeoutAsync<HttpResponseMessage>(TimeSpan.FromSeconds(60)))
    .AddTransientHttpErrorPolicy(builder => builder.WaitAndRetryAsync(1, retryAttempt => TimeSpan.FromSeconds(Math.Pow(2, retryAttempt))));
var app = builder.Build();
if (!app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
   
}
else
{
    app.UseDeveloperExceptionPage();
}
app.UseHsts();
app.UseForwardedHeaders(new ForwardedHeadersOptions
{
    ForwardedHeaders = ForwardedHeaders.XForwardedProto
});

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();
app.MapRazorPages();
app.MapControllers();
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Catalog}/{action=Index}/{id?}");

app.Run();