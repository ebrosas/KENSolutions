using KenHRApp.TaskDataServices.Configurations;
using KenHRApp.TaskDataServices.Data;
using KenHRApp.TaskDataServices.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Serilog;

var builder = Host.CreateApplicationBuilder(args);

builder.Configuration
    .SetBasePath(AppContext.BaseDirectory)
    .AddJsonFile(
        "appsettings.json",
        optional: false,
        reloadOnChange: true);

Log.Logger = new LoggerConfiguration()
    .WriteTo.File(
        path: builder.Configuration["Serilog:LogPath"]!,
        rollingInterval: RollingInterval.Day,
        retainedFileCountLimit: 30)
    .CreateLogger();

builder.Logging.ClearProviders();
builder.Logging.AddSerilog();

builder.Services.Configure<EnvironmentSettings>(
    builder.Configuration.GetSection("EnvironmentSettings"));

builder.Services.Configure<AttendanceSettings>(
    builder.Configuration.GetSection("AttendanceSettings"));

builder.Services.Configure<PayrollSettings>(
    builder.Configuration.GetSection("PayrollSettings"));

builder.Services.Configure<EmailSettings>(
    builder.Configuration.GetSection("EmailSettings"));

var environmentSettings = builder.Configuration
    .GetSection("EnvironmentSettings")
    .Get<EnvironmentSettings>();

string connectionString;

switch(environmentSettings!.Environment )
{
    case "Production":
        connectionString = builder.Configuration
            .GetConnectionString("ProductionDB")!;
        break;

    case "Staging":
        connectionString = builder.Configuration
            .GetConnectionString("StagingDB")!;
        break;

    case "Development":
        connectionString = builder.Configuration
            .GetConnectionString("DevelopmentDB")!;
        break;

    default:
        throw new Exception($"Unsupported environment: {environmentSettings.Environment}");
}

builder.Services.AddDbContext<ApplicationDbContext>(options =>
{
    options.UseSqlServer(connectionString);
});

builder.Services.AddScoped<INotificationService, NotificationService>();
builder.Services.AddScoped<IJobService, AttendanceService>();
builder.Services.AddScoped<IJobService, PayrollService>();

var app = builder.Build();

using var scope = app.Services.CreateScope();

var services = scope.ServiceProvider;

var logger = services.GetRequiredService<ILogger<Program>>();

try
{
    if (args.Length == 0)
    {
        throw new Exception(
            "Execution key parameter is missing.");
    }

    string executionKey = args[0];

    logger.LogInformation(
        "Execution Key: {ExecutionKey}",
        executionKey);

    var jobServices = services.GetServices<IJobService>();

    var selectedService = jobServices.FirstOrDefault(x =>
        x.ExecutionKey.Equals(
            executionKey,
            StringComparison.OrdinalIgnoreCase));

    if (selectedService == null)
    {
        throw new Exception(
            $"No service found for execution key: {executionKey}");
    }

    await selectedService.ExecuteAsync();

    logger.LogInformation(
        "Job execution completed successfully.");
}
catch (Exception ex)
{
    logger.LogError(ex, "Application execution failed.");
}
finally
{
    Log.CloseAndFlush();
}

