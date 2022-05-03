global using Serilog;
global using System.CommandLine;
global using System.Text;
using Sentry;
using Serilog.Core;
using Serilog.Events;
using System.Diagnostics;

namespace autocli;

internal static class Parser
{
    #region Static strings
    internal static string config = "Properties//Architecture.json";
    internal static string DsnToken = "https://0d3ccea4eae040b384c14383364e2878@o1213812.ingest.sentry.io/6370129";
    internal static string LogFormat = "{Timestamp:yyyy-MM-dd HH:mm:ss.fff zzz} [{Level:u4}] {Message:1j}{NewLine}{Exception}";
    #endregion

    internal static void ExecuteCommandSync(string command)
    {
        /*create the ProcessStartInfo using "cmd" as the program to be run, and "/c " as
        the parameters. Incidentally, /c tells cmd that we want it to execute the command
        that follows, and then exit.*/
        ProcessStartInfo procStartInfo = new("cmd", "/c " + command);

        /* The following commands are needed to redirect the standard output. This means
        that it will be redirected to the Process.StandardOutput StreamReader. */
        procStartInfo.RedirectStandardOutput = true;
        procStartInfo.UseShellExecute = false;
        procStartInfo.CreateNoWindow = true;

        // Now we create a process, assign its ProcessStartInfo and start it
        Process proc = new();
        proc.StartInfo = procStartInfo;
        proc.Start();

        // Get the output into a string
        string result = proc.StandardOutput.ReadToEnd();
        Console.WriteLine(result);
    }
    
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
            .WriteTo.File("./logs/autocli.log", rollingInterval: RollingInterval.Minute, restrictedToMinimumLevel: LogEventLevel.Verbose)
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
            Log.CloseAndFlush();
            throw new ArgumentException("Architecture deserialization failed.", ex);
        }
    }
}