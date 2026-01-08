using DevSkill.Blog.Infrastructure.Data;
using DevSkill.Blog.Infrastructure.Extensions;
using DevSkill.Blog.Worker;
using Serilog;
using System.Reflection;

var configuration = new ConfigurationBuilder()
                       .AddJsonFile("appsettings.json",false)
                       .AddEnvironmentVariables()
                       .Build();

Log.Logger = new LoggerConfiguration()
                .MinimumLevel.Debug()
                .MinimumLevel.Override("Microsoft", Serilog.Events.LogEventLevel.Warning)
                .Enrich.FromLogContext()
                .ReadFrom.Configuration(configuration)
                .CreateLogger();

try
{
    //var builder = Host.CreateApplicationBuilder(args);
    //builder.Services.AddHostedService<Worker>();

    //var host = builder.Build();
    //host.Run();

    var connectionString = configuration.GetConnectionString("DefaultConnection") ??
                                        throw new InvalidOperationException("Connection string 'DefaultConnection' not found");
    var migrationAssembly = Assembly.GetAssembly(typeof(ApplicationDbContext));

    IHost host = Host.CreateDefaultBuilder(args)
                     .UseWindowsService()
                     .UseSerilog()
                     .ConfigureServices(services =>
                     {
                         services.AddHostedService<Worker>();
                         services.AddDependencyInjections();
                         services.AddApplicationDbContext(connectionString,migrationAssembly);
                     })
                     .Build();
}
catch(Exception ex)
{
    Log.Fatal(ex, "Worker service crashed");
}
finally
{
    Log.CloseAndFlush();
}
