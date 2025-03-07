using System.Reflection;
using Serilog;
using Serilog.Exceptions;

namespace PurpleHackBackend.Logs;

public static class LoggingConfigurator
{
    public static void ConfigureLogging(){
        var environment = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") ?? "Production";
        var configuration = new ConfigurationBuilder()
            .AddJsonFile("appsettings.json",optional:false,reloadOnChange:true).Build();
        Console.WriteLine(environment);
        Console.WriteLine(configuration);
        Log.Logger = new LoggerConfiguration()
            .Enrich.FromLogContext()
            .Enrich.WithExceptionDetails()
            .WriteTo.Debug()
            .WriteTo.Console()
            .Enrich.WithProperty("Environment",environment)
            .ReadFrom.Configuration(configuration)
            .CreateLogger();
    }
}