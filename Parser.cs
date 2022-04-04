// This file is supposed to be auto-generated
global using Serilog;
global using System.CommandLine;
global using System.Diagnostics;
using Serilog.Core;

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
            Log.Logger = NewLogger();
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
            var Commands = new List<Command>()
            {
                Interface.Constructors.MakeRootCommand(
                name: "autocli",
                title: AppProperties.Title,
                description: AppProperties.Description)
            };
            foreach (Interface.Commands cmd in ListCommands)
            {
                Commands.Add(Interface.Constructors.MakeCommand(
                parent: Interface.Constructors.GetCommand(Commands, cmd.Parent)!,
                alias: cmd.Alias,
                description: cmd.Description,
                verbosity: cmd.Verbosity));
            }
            Log.Debug("Commands built.");

            #endregion Commands

            #region Options

            var ListOptions = Functionnals.ParseArchitecture.GetOptions(dict);
            var Options = new List<Option>();
            foreach (Interface.Options option in ListOptions)
            {
                Options.Add(Interface.Constructors.MakeOption<string>(
                command: Interface.Constructors.GetCommand(Commands, option.Command)!,
                aliases: option.Aliases,
                required: option.Required,
                defaultvalue: option.DefaultValue,
                description: option.Description));
            }
            Log.Debug("Options built.");

            #endregion Options

            #region Arguments

            var ListArguments = Functionnals.ParseArchitecture.GetArguments(dict);
            var Arguments = new List<Argument>();
            foreach (Interface.Arguments arg in ListArguments)
            {
                Arguments.Add(Interface.Constructors.MakeArgument<string>(
                command: Interface.Constructors.GetCommand(Commands, arg.Command)!,
                alias: arg.Alias,
                defaultvalue: arg.DefaultValue,
                description: arg.Description));
            }
            Log.Debug("Arguments built.");

            #endregion Arguments

            #region Handlers

            Functionnals.Handlers.CallHandlers(Commands, Arguments, Options, AppProperties, ListPackages);

            Log.Debug("Handlers implemented.");

            #endregion Handlers

            Log.Information("Automated CLI creation tool built.");
            Log.Verbose("Invoking args parser.");
            Log.CloseAndFlush();

            await Commands[0].InvokeAsync(args);
        }

        /// <summary>
        /// Sets and returns a new configured instance of a logger
        /// </summary>
        /// <param name="verbosity">Output verbosity of the application</param>
        /// <returns></returns>
        public static ILogger NewLogger(string? verbosity = null)
        {
            var verbose = new LoggingLevelSwitch(); // .ControlledBy(verbose)
            Log.Logger = new LoggerConfiguration()
                .MinimumLevel.Verbose()
                .WriteTo.Console(outputTemplate:
                                "[{Timestamp:HH:mm:ss:ff} {Level:u4}] {Message:1j}{NewLine}{Exception}")
                .WriteTo.File("./logs/autocli.log.txt", rollingInterval: RollingInterval.Minute)
                .CreateLogger();
            return Log.Logger;
        }
    }
}