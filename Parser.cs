global using Serilog;
global using System.CommandLine;
using Sentry;
using Serilog.Core;
using Serilog.Events;

namespace autocli;

internal static class Parser
{
    #region Static strings

    internal static string config = "Properties\\Architecture.json";
    internal static string DsnToken = "https://5befa8f2131e4d55b57193308225770e@o1213812.ingest.sentry.io/6353266";
    internal static string LogFormat = "{Timestamp:yyyy-MM-dd HH:mm:ss.fff zzz} [{Level:u4}] {Message:1j}{NewLine}{Exception}";

    #endregion

    /// <summary>
    /// Sets and returns a new configured instance of a logger with sentry integration
    /// </summary>
    internal static ILogger BuildLoggers(string[] args)
    {
        // Default values of args
        char vv = 'm';
        char vs = 'm';

        // Custom parser for logger verbosity configuration 
        foreach (var arg in args)
        {
            if (arg.StartsWith("-v") || arg.StartsWith("--verbose")) vv = arg[^1];
            else if (arg.StartsWith("-s") || arg.StartsWith("--sentry")) vs = arg[^1];
        }

        var levelSwitch = new LoggingLevelSwitch
        {
            MinimumLevel = vv switch
            {
                ('v') => LogEventLevel.Verbose,
                ('d') => LogEventLevel.Debug,
                _ => LogEventLevel.Information,
            }
        };
        return new LoggerConfiguration()
            .MinimumLevel.ControlledBy(levelSwitch)
            .WriteTo.Sentry(o =>
            {
                o.MinimumBreadcrumbLevel = LogEventLevel.Debug;
                o.MinimumEventLevel = LogEventLevel.Error;
                o.StackTraceMode = StackTraceMode.Enhanced;
                o.AttachStacktrace = true;
                o.Dsn = DsnToken;
                o.Debug = ((vs == 'd') || (vs == 'd')) ? true : false;
            })
            .WriteTo.Console(outputTemplate: LogFormat)
            .WriteTo.File("./logs/autocli.log.txt", rollingInterval: RollingInterval.Minute, restrictedToMinimumLevel: LogEventLevel.Verbose)
            .CreateLogger();
    }

    /// <summary>
    /// Async task to parse the array of args as strings
    /// </summary>
    internal static async Task Main(string[] args)
    {
        Log.Logger = BuildLoggers(args);
        try
        {
            Interface.ConsoleApp Interface = new(config);
            Interface.SetHandlers();

            await Interface.GetRootCommand().InvokeAsync(args);
        }
        catch (Exception ex)
        {
            Log.Error(ex, ex.Message, ex.ToString());
            throw new ArgumentException("Architecture deserialization failed.", ex);
        }
    }
}