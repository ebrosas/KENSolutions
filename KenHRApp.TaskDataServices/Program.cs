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

    builder.Services.Configure<AttendanceSettings>(
        builder.Configuration.GetSection("AttendanceSettings"));

    builder.Services.Configure<EmailSettings>(
        builder.Configuration.GetSection("EmailSettings"));

    builder.Services.AddDbContext<ApplicationDbContext>(options =>
    {
        options.UseSqlServer(
            builder.Configuration.GetConnectionString("DefaultConnection"));
    });

    builder.Services.AddScoped<IAttendanceService, AttendanceService>();
    builder.Services.AddScoped<INotificationService, NotificationService>();

    var app = builder.Build();

    using var scope = app.Services.CreateScope();

    var services = scope.ServiceProvider;

    var logger = services.GetRequiredService<ILogger<Program>>();
    var attendanceService = services.GetRequiredService<IAttendanceService>();
    var notificationService = services.GetRequiredService<INotificationService>();

    try
    {
    //DateTime attendanceDate = DateTime.Today.AddDays(-1);

    #region Set the attendance date to process
    var attendanceSettings = services
    .GetRequiredService<
        Microsoft.Extensions.Options.IOptions<AttendanceSettings>>()
    .Value;

    DateTime attendanceDate;

    if (!string.IsNullOrWhiteSpace(attendanceSettings.AttendanceDate))
    {
        if (!DateTime.TryParse(
                attendanceSettings.AttendanceDate,
                out attendanceDate))
        {
            throw new Exception(
                "Invalid AttendanceDate format in appsettings.json");
        }

        attendanceDate = attendanceDate.Date;
    }
    else
    {
        attendanceDate = DateTime.Today
            .AddDays(attendanceSettings.ExecutionDateOffset)
            .Date;
    }
    #endregion

    logger.LogInformation(
            "Application started at {Time}",
            DateTime.Now);

        await attendanceService.ExecuteAttendanceGenerationAsync(attendanceDate);

        await notificationService
            .SendSuccessNotificationAsync(attendanceDate);

        logger.LogInformation(
            "Application completed successfully.");
    }
    catch (Exception ex)
    {
        logger.LogError(ex, "Application execution failed.");

        await notificationService
            .SendFailureNotificationAsync(ex);
    }
    finally
    {
        Log.CloseAndFlush();
    }