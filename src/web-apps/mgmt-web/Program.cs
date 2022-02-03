using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.Identity.Web;
using Microsoft.Identity.Web.UI;
using XtremeIdiots.Portal.RepositoryApiClient.GameServerApi;
using XtremeIdiots.Portal.RepositoryApiClient.GameServersApi;
using XtremeIdiots.Portal.RepositoryApiClient.GameServerSecretApi;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddLogging();
builder.Services.AddApplicationInsightsTelemetry();

// Add services to the container.
builder.Services.AddAuthentication(OpenIdConnectDefaults.AuthenticationScheme)
    .AddMicrosoftIdentityWebApp(
        options => { builder.Configuration.Bind("AzureAd", options); },
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
        options.BaseUrl = builder.Configuration["apim-base-url"];
        options.Scopes = builder.Configuration["web-api-repository-scope"];
    })
    .AddInMemoryTokenCaches();

builder.Services.AddAuthorization(options => { options.FallbackPolicy = options.DefaultPolicy; });

builder.Services.AddRazorPages(options =>
    {
        options.Conventions.AllowAnonymousToPage("/Index");
        options.Conventions.AllowAnonymousToPage("/Privacy");
    }).AddMicrosoftIdentityUI()
    .AddRazorRuntimeCompilation();

builder.Services.AddSingleton<IGameServersApiClient, GameServersApiClient>(_ =>
    new GameServersApiClient(builder.Configuration["apim-base-url"], builder.Configuration["apim-subscription-key"]));
builder.Services.AddSingleton<IGameServerApiClient, GameServerApiClient>(_ =>
    new GameServerApiClient(builder.Configuration["apim-base-url"], builder.Configuration["apim-subscription-key"]));
builder.Services.AddSingleton<IGameServerSecretApiClient, GameServerSecretApiClient>(_ =>
    new GameServerSecretApiClient(builder.Configuration["apim-base-url"],
        builder.Configuration["apim-subscription-key"]));

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