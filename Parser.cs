// This file is supposed to be auto-generated
global using Serilog;
global using System.CommandLine;
global using System.Diagnostics;
using Serilog.Core;
using Serilog.Events;

namespace autocli
{
    public static class Parser
    {
        /// <summary>
        /// Async task to parse the array of args as strings
        /// </summary>
        /// <param name="args">Type : string[]</param>
        public static async Task Main(string[] args)
        {
            Log.Logger = (args.Length is not 0) ? NewLogger(args[1]) : NewLogger();

            Log.Debug("Logger built.");

            var dict = Functionnals.ParseArchitecture.JsonParser(@"C:\Users\matte\source\repos\autoCLI\Properties\Architecture.autocli.json");

            #region Properties

            Interface.Properties AppProperties = Functionnals.ParseArchitecture.GetProperties(dict);
            AppProperties.SetName(@"C:\Users\matte\source\repos\autoCLI\Properties\Architecture.autocli.json");
            Log.Debug("Properties built.");

            #endregion Properties

            #region Packages

            List<Interface.Packages> ListPackages = Functionnals.ParseArchitecture.GetPackages(dict)!;
            Log.Debug("Packages built.");

            #endregion Packages

            #region Commands

            var ListCommands = Functionnals.ParseArchitecture.GetCommands(dict);
            RootCommand root = Interface.Constructors.BuildRoot(
                name: "autocli",
                title: AppProperties.Title,
                description: AppProperties.Description);

            var Commands = new List<Command>()
            {
                root
            };
            foreach (Interface.Commands cmd in ListCommands)
            {
                Commands.Add(Interface.Constructors.BuildCommand(
                parent: Interface.Constructors.Get(Commands, cmd.Parent)!,
                alias: cmd.Alias,
                description: cmd.Description));
            }
            Log.Debug("Commands built.");

            #endregion Commands

            #region Options

            var ListOptions = Functionnals.ParseArchitecture.GetOptions(dict);
            var Options = new List<Option>();
            foreach (Interface.Options option in ListOptions)
            {
                Options.Add(Interface.Constructors.BuildOption<string>(
                command: Interface.Constructors.Get(Commands, option.Command)!,
                name: option.Name,
                aliases: option.Aliases,
                required: option.Required,
                defaultvalue: option.DefaultValue,
                description: option.Description));
            }
            /// <summary>
            /// Add verbosity global option
            /// </summary>
            Option<string> verbose = new Option<string>(
                new[] { "--verbose", "-v" }, "Verbosity level of the output : m[inimal]; d[ebug]; v[erbose].")
                .FromAmong("m", "d", "v");
            verbose.SetDefaultValue("m");
            Commands[0].AddGlobalOption(verbose);

            Log.Debug("Options built.");

            #endregion Options

            #region Arguments

            var ListArguments = Functionnals.ParseArchitecture.GetArguments(dict);
            var Arguments = new List<Argument>();
            foreach (Interface.Arguments arg in ListArguments)
            {
                Arguments.Add(Interface.Constructors.BuildArgument<string>(
                command: Interface.Constructors.Get(Commands, arg.Command)!,
                alias: arg.Alias,
                defaultvalue: arg.DefaultValue,
                description: arg.Description));
            }
            Log.Debug("Arguments built.");

            #endregion Arguments

            #region Handlers

            root.SetHandler(() => root.InvokeAsync("-h"));
            Functionnals.Handlers.CallHandlers(Commands, Arguments, Options, verbose, AppProperties, ListPackages);

            Log.Debug("Handlers implemented.");

            #endregion Handlers

            Log.Information("Automated CLI creation tool built.");
            Log.Verbose("Invoking args parser.");
            Log.CloseAndFlush();

            await root.InvokeAsync(args);
        }

        /// <summary>
        /// Sets and returns a new configured instance of a logger
        /// </summary>
        /// <param name="verbose">Output verbosity of the application</param>
        public static ILogger NewLogger(string verbose = null!)
        {
            var levelSwitch = new LoggingLevelSwitch
            {
                MinimumLevel = verbose switch
                {
                    ("v") => Serilog.Events.LogEventLevel.Verbose,
                    ("d") => Serilog.Events.LogEventLevel.Debug,
                    _ => Serilog.Events.LogEventLevel.Information,
                }
            };
            return new LoggerConfiguration()
                .MinimumLevel.ControlledBy(levelSwitch)
                .WriteTo.Console(outputTemplate:
                        "[{Timestamp:HH:mm:ss:ff} {Level:u4}] {Message:1j}{NewLine}{Exception}")
                .WriteTo.File("./logs/autocli.log.txt", rollingInterval: RollingInterval.Minute, restrictedToMinimumLevel: LogEventLevel.Verbose)
                .CreateLogger();
        }
    }
}