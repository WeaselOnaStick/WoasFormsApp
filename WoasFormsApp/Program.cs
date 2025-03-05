using Blazored.LocalStorage;
using Duende.IdentityModel.Client;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using MudBlazor;
using MudBlazor.Services;
using WoasFormsApp;
using WoasFormsApp.Components;
using WoasFormsApp.Data;
using WoasFormsApp.Services;
using static WoasFormsApp.Utils.Localization;

var builder = WebApplication.CreateBuilder(args);

builder.Configuration.AddEnvironmentVariables();

builder.Services.AddLocalization();
builder.Services.AddControllers();

builder.Services.AddScoped<IUserPrefService<bool>,      ThemePrefsService>(factory =>
{
    return new ThemePrefsService(factory.GetRequiredService<ILocalStorageService>());
});

builder.Services.AddFluentValidationAutoValidation();

builder.Services.AddMudServices(config =>
{
    config.SnackbarConfiguration.PositionClass = MudBlazor.Defaults.Classes.Position.BottomStart;
});
builder.Services.AddMudMarkdownServices();

// Add services to the container.
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents()
    .AddInteractiveWebAssemblyComponents()
    .AddAuthenticationStateSerialization();

builder.Services.AddCascadingAuthenticationState();

builder.Services.AddAuthentication(options =>
    {
        options.DefaultScheme = IdentityConstants.ApplicationScheme;
        options.DefaultSignInScheme = IdentityConstants.ExternalScheme;
    })
    .AddIdentityCookies();
builder.Services.AddAuthorization();

builder.Services.ConfigureApplicationCookie(options =>
{
    options.Events.OnRedirectToLogin = context =>
    {
        context.Response.StatusCode = StatusCodes.Status401Unauthorized;
        return Task.CompletedTask;
    };

    options.Events.OnRedirectToAccessDenied = context =>
    {
        context.Response.StatusCode = StatusCodes.Status403Forbidden;
        return Task.CompletedTask;
    };
});

builder.Services.Configure<SecurityStampValidatorOptions>(options =>
{
    options.ValidationInterval = TimeSpan.Zero;
});


builder.Services.AddDistributedMemoryCache();
builder.Services.AddClientCredentialsTokenManagement()
    .AddClient("salesforce", client =>
    {
        client.TokenEndpoint = "https://woascom-dev-ed.develop.my.salesforce.com/services/oauth2/token";
        client.ClientId = builder.Configuration["SalesForce:CLEINT_ID"];
        client.ClientSecret = builder.Configuration["SalesForce:CLIENT_SECRET"];
        client.Parameters.Add("grant_type", "client_credentials");
    });

builder.Services.AddHttpClient("salesforce", client =>
{
    client.BaseAddress = new Uri(builder.Configuration["SalesForceClient:BaseAddress"]??"");
});

builder.Services.AddHttpClient("jira", client =>
{
    client.BaseAddress = new Uri("https://woasforms.atlassian.net/rest/api/latest/");
    client.DefaultRequestHeaders.Authorization = new BasicAuthenticationHeaderValue(builder.Configuration["Jira:LOGIN"]??"", builder.Configuration["Jira:PASSWORD"]??"");
});

builder.Services.AddScoped<ISFContactManager, SFContactManager>();

var connectionString = builder.Configuration.GetConnectionString("DefaultConnection") ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");

builder.Services.AddDbContext<WoasFormsDbContext>(options =>
{
    options.UseSqlite(connectionString);
});

builder.Services.AddScoped<IDatabaseAccessorService, DatabaseAccessorService>();

builder.Services.AddDatabaseDeveloperPageExceptionFilter();

builder.Services.AddScoped<IRoleStore<IdentityRole>, RoleStore<IdentityRole, WoasFormsDbContext>>();

builder.Services.AddIdentityCore<WoasFormsAppUser>(options =>
{
    options.SignIn.RequireConfirmedAccount = false;
    options.SignIn.RequireConfirmedEmail = false;
    options.Lockout.AllowedForNewUsers = false;
    options.User.RequireUniqueEmail = true;
    options.Password.RequireDigit = false;
    options.Password.RequiredLength = 3;
    options.Password.RequireLowercase = false;
    options.Password.RequireUppercase = false;
    options.Password.RequireNonAlphanumeric = false;

})
    .AddEntityFrameworkStores<WoasFormsDbContext>()
    .AddRoles<IdentityRole>()
    .AddRoleManager<RoleManager<IdentityRole>>()
    .AddRoleStore<RoleStore<IdentityRole, WoasFormsDbContext>>()
    .AddUserStore<UserStore<WoasFormsAppUser, IdentityRole, WoasFormsDbContext>>()
    .AddSignInManager()
    .AddDefaultTokenProviders();

builder.Services.AddBlazoredLocalStorage();
var app = builder.Build();


var supportedCultures = SupportedLocales.Select(l => l.CultureInfo.Name).ToArray();

var localizationOptions = new RequestLocalizationOptions()
    .AddSupportedCultures(supportedCultures)
    .AddSupportedUICultures(supportedCultures)
    .SetDefaultCulture(supportedCultures[0]);

app.UseRequestLocalization(localizationOptions);

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseWebAssemblyDebugging();
    app.UseMigrationsEndPoint();
}
else
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

//app.UseHttpsRedirection();

app.UseStatusCodePagesWithRedirects("/404");


app.UseAntiforgery();

app.MapControllers();

app.MapStaticAssets();
app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode()
    .AddInteractiveWebAssemblyRenderMode();

app.MapAdditionalIdentityEndpoints();

using (var scope = app.Services.CreateScope())
{
    await Seeder.SeedRoles(scope.ServiceProvider);
    await Seeder.SeedFieldTypes(scope.ServiceProvider);
}

app.Run();
