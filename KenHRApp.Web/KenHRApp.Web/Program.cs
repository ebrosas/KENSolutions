using KenHRApp.Web.Client.Pages;
using KenHRApp.Web.Components;
using KenHRApp.Web.Components.Account;
using KenHRApp.Web.Data;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Identity;
using KenHRApp.Application.Interfaces;
using KenHRApp.Application.Services;
using KenHRApp.Infrastructure.Data;
using KenHRApp.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using MudBlazor.Services;
using Microsoft.AspNetCore.StaticFiles.Infrastructure;
using KenHRApp.Domain.Entities;
using KenHRApp.Domain.Interfaces;
using KenHRApp.Application.Common.Interfaces;
using KenHRApp.Application.Common.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents()
    .AddInteractiveWebAssemblyComponents();

#region Initialize DI container
builder.Services.AddMudServices();
builder.Services.AddCascadingAuthenticationState();
builder.Services.AddMemoryCache();
//builder.Services.AddScoped<IAppCacheService, KenHRApp.Infrastructure.Services.AppCacheService>();
//builder.Services.AddScoped<IdentityUserAccessor>();
//builder.Services.AddScoped<IdentityRedirectManager>();
//builder.Services.AddScoped<AuthenticationStateProvider, PersistingRevalidatingAuthenticationStateProvider>();
builder.Services.AddScoped<IEmployeeRepository, EmployeeRepository>();
builder.Services.AddScoped<IRecruitmentRepository, RecruitmentRepository>();
builder.Services.AddScoped<IEmployeeService, EmployeeService>();
builder.Services.AddScoped<IRecruitmentService, RecruitmentService>();
builder.Services.AddScoped<IAppCacheRepository, AppCacheRepository>();
builder.Services.AddScoped<IAppCacheService, AppCacheService>();
builder.Services.AddScoped<IAppState, AppState>();
builder.Services.AddScoped<ISharedAction, SharedAction>();
builder.Services.AddScoped<ILookupCacheService, LookupCacheService>();
#endregion

builder.Services.AddAuthentication(options =>
    {
        options.DefaultScheme = IdentityConstants.ApplicationScheme;
        options.DefaultSignInScheme = IdentityConstants.ExternalScheme;
    })
.AddIdentityCookies();

#region Configure the DB Connection String
string connectionString = string.Empty;

// Get the application environment from the configuration
string? env = builder.Configuration["AppSettings:Environment"];
switch(env)
{
    case "Staging":
        // Use the development connection string
        connectionString = builder.Configuration.GetConnectionString("StagingDB")
            ?? throw new InvalidOperationException("Connection string 'StagingDB' not found.");
        break;

    case "Production":
        // Use the development connection string
        connectionString = builder.Configuration.GetConnectionString("ProductionDB")
            ?? throw new InvalidOperationException("Connection string 'ProductionDB' not found.");
        break;

    default:
        connectionString = builder.Configuration.GetConnectionString("DevelopmentDB")
            ?? throw new InvalidOperationException("Connection string 'DevelopmentDB' not found.");
        break;
}
#endregion

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(connectionString));

//builder.Services.AddDbContext<ApplicationDbContext>(options =>
//    options.UseSqlServer(connectionString));

// EF Core with InMemory for simplicity
//builder.Services.AddDbContext<AppDbContext>(options =>
//    options.UseInMemoryDatabase("KenHRDb"));

builder.Services.AddDatabaseDeveloperPageExceptionFilter();

//builder.Services.AddIdentityCore<ApplicationUser>(options => options.SignIn.RequireConfirmedAccount = true)
//    .AddEntityFrameworkStores<ApplicationDbContext>()
//    .AddSignInManager()
//    .AddDefaultTokenProviders();

//builder.Services.AddSingleton<IEmailSender<ApplicationUser>, IdentityNoOpEmailSender>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment() ||
    app.Environment.IsStaging())
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

app.UseHttpsRedirection();

app.UseStaticFiles();
app.UseAntiforgery();

app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode()
    .AddInteractiveWebAssemblyRenderMode()
    .AddAdditionalAssemblies(typeof(KenHRApp.Web.Client._Imports).Assembly);

// Add additional endpoints required by the Identity /Account Razor components.
//app.MapAdditionalIdentityEndpoints();

app.Run();
