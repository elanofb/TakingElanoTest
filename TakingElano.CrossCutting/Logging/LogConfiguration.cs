using Serilog;
using Serilog.Events;
using Serilog.Sinks.File;


namespace TakingElano.CrossCutting.Logging;

public static class LogConfiguration
{
    public static void ConfigureLogging()
    {
        Log.Logger = new LoggerConfiguration()
            .WriteTo.File("logs/log-.txt", rollingInterval: RollingInterval.Day)
            .CreateLogger();

    }
}
