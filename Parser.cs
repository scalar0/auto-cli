global using Newtonsoft.Json;
global using Serilog;
global using System.CommandLine;
using Serilog.Core;
using Serilog.Events;

namespace autocli
{
    public static class Parser
    {
        /// <summary>
        /// Sets and returns a new configured instance of a logger
        /// </summary>
        /// <param name="verbose">Output verbosity of the application</param>
        public static ILogger BuildLogger(string verbose = null!)
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

        private const string config = @"C:\Users\matte\source\repos\autoCLI\Properties\Architecture.json";

        /// <summary>
        /// Async task to parse the array of args as strings
        /// </summary>
        /// <param name="args">Type : string[]</param>
        public static async Task Main(string[] args)
        {
            // Logger
            Log.Logger = (args.Length is not 0) ? BuildLogger(args[^1]) : BuildLogger();

            // App Interface
            Interface.IJsonApp Interface = new(config);
            RootCommand root = Interface.GetRootCommand();

            // Handlers
            root.SetHandler(() => root.InvokeAsync("-h"));
            Functionnals.Handlers.CallHandlers(Interface);

            Log.Verbose("Invoking args parser.");
            Log.CloseAndFlush();
            await root.InvokeAsync(args);
        }
    }
}