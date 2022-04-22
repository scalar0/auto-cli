global using Serilog;
global using System.CommandLine;
using Sentry;
using Serilog.Core;
using Serilog.Events;

namespace autocli;

internal static class Parser
{
    /// <summary>
    /// Sets and returns a new configured instance of a logger
    /// </summary>
    /// <param name="verbose">Output verbosity of the application</param>
    internal static ILogger BuildLogger(string verbose = null!)
    {
        var levelSwitch = new LoggingLevelSwitch
        {
            MinimumLevel = verbose switch
            {
                ("v") => LogEventLevel.Verbose,
                ("d") => LogEventLevel.Debug,
                _ => LogEventLevel.Information,
            }
        };
        return new LoggerConfiguration()
            .MinimumLevel.ControlledBy(levelSwitch)
            .WriteTo.Console(outputTemplate:
                    "[{Timestamp:HH:mm:ss:ff} {Level:u4}] {Message:1j}{NewLine}{Exception}")
            .WriteTo.File("./logs/autocli.log.txt", rollingInterval: RollingInterval.Minute, restrictedToMinimumLevel: LogEventLevel.Verbose)
            .CreateLogger();
    }

    internal static string config = string.Format("C:\\Users\\matte\\src\\repos\\{0}\\Properties\\Architecture.json", typeof(Parser).Namespace);

    /// <summary>
    /// Async task to parse the array of args as strings
    /// </summary>
    /// <param name="args">Type : string[]</param>
    internal static async Task Main(string[] args)
    {
        Log.Logger = (args.Length is not 0) ? BuildLogger(args[^1]) : BuildLogger();
        using (SentrySdk.Init(Sentry =>
    {
        Sentry.Dsn = "https://5befa8f2131e4d55b57193308225770e@o1213812.ingest.sentry.io/6353266";
        // Set Sentry logger verbosity
        Sentry.Debug = false;
        // Percentage of captured transactions for performance monitoring.
        Sentry.TracesSampleRate = 1.0;
    }))
        {
            Interface.IJsonApp Interface = new(config);
            Functionnals.Handlers.CallHandlers(Interface);

            SentrySdk.CaptureMessage("Issue testing.");

            Log.Verbose("Invoking args parser.");
            Log.CloseAndFlush();
            await Interface.GetRootCommand().InvokeAsync(args);
        }
    }
}