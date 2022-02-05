using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.Identity.Web;
using Microsoft.Identity.Web.UI;
using XtremeIdiots.Portal.RepositoryApiClient;

var builder = WebApplication.CreateBuilder(args);

var config = builder.Configuration;

builder.Services.AddLogging();
builder.Services.AddApplicationInsightsTelemetry();

// Add services to the container.
builder.Services.AddAuthentication(OpenIdConnectDefaults.AuthenticationScheme)
    .AddMicrosoftIdentityWebApp(
        options => { config.Bind("AzureAd", options); },
        options =>
        {
            options.Cookie.SameSite = SameSiteMode.None;
            options.Cookie.SecurePolicy = CookieSecurePolicy.Always;
            options.Cookie.IsEssential = true;
        }
    )
    .EnableTokenAcquisitionToCallDownstreamApi()
    .AddDownstreamWebApi("portal-repository-api-box", options =>
    {
        options.BaseUrl = config["apim-base-url"];
        options.Scopes = config["web-api-repository-scope"];
    })
    .AddInMemoryTokenCaches();

builder.Services.AddAuthorization(options => { options.FallbackPolicy = options.DefaultPolicy; });

builder.Services.AddRazorPages(options =>
    {
        options.Conventions.AllowAnonymousToPage("/Index");
        options.Conventions.AllowAnonymousToPage("/Privacy");
    }).AddMicrosoftIdentityUI()
    .AddRazorRuntimeCompilation();

builder.Services.AddRepositoryApiClient(options =>
{
    options.ApimBaseUrl = config["apim-base-url"];
    options.ApimSubscriptionKey = config["apim-subscription-key"];
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapRazorPages();
app.MapControllers();

app.Run();