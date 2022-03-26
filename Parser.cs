// This file is supposed to be auto-generated
global using Serilog;
global using System.CommandLine;
using autocli.Functionnals;
using autocli.Interface;
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
            // ===========================================LOGGER===========================================
            Log.Logger = NewLogger();
            // ===========================================PROPERTIES=========================================
            var dict = ParseArchitecture.JsonParser(@"C:\Users\matte\source\repos\autoCLI\Properties\autocli.Architecture.json");
            Properties app = ParseArchitecture.GetProperties(dict);
            List<Packages>? ListPackages = ParseArchitecture.GetPackages(dict);
            // ===========================================COMMANDS===========================================
            var ListCommands = ParseArchitecture.GetCommands(dict);
            var Commands = new List<Command>()
            {
                Constructors.MakeRootCommand(
                name: "RootCommand",
                title: app.Title,
                description: app.Description)
            };
            foreach (Commands cmd in ListCommands)
            {
                Commands.Add(Constructors.MakeCommand(
                parent: Constructors.GetCommand(Commands, cmd.Parent)!,
                alias: cmd.Alias,
                description: cmd.Description,
                verbosity: cmd.Verbosity));
            }
            Log.Information("Commands built.");
            // ===========================================OPTIONS===========================================
            var ListOptions = ParseArchitecture.GetOptions(dict);
            var Options = new List<Option>();
            foreach (Options option in ListOptions)
            {
                Options.Add(Constructors.MakeOption<string>(
                command: Constructors.GetCommand(Commands, option.Command)!,
                aliases: option.Aliases,
                required: option.Required,
                defaultvalue: option.DefaultValue,
                description: option.Description));
            }
            Log.Information("Options built.");
            // ===========================================ARGUMENTS===========================================
            var ListArguments = ParseArchitecture.GetArguments(dict);
            var Arguments = new List<Argument>();
            foreach (Arguments arg in ListArguments)
            {
                Arguments.Add(Constructors.MakeArgument<string>(
                command: Constructors.GetCommand(Commands, arg.Command)!,
                alias: arg.Alias,
                defaultvalue: arg.DefaultValue,
                description: arg.Description));
            }
            Log.Information("Arguments built.");
            // ===========================================HANDLERS===========================================

            Log.Information("Implementing handlers...");

            /*            Constructors.GetCommand(Commands, "create").SetHandler<string, DirectoryInfo>(Handlers.creation, file_name, dir_choice);
                        Constructors.GetCommand(Commands, "generate").SetHandler<string>(Handlers.generation, file_path, pushing);*/
            Log.Information("... done.");
            // ===========================================INVOKE===========================================
            Log.Debug("Invoking args...");
            Log.CloseAndFlush();
            await Commands[0].InvokeAsync(args);
        }

        /// <summary>
        /// Sets and returns a new configured instance of a logger
        /// </summary>
        /// <param name="verbosity">Output verbosity of the application</param>
        /// <returns> </returns>
        public static ILogger NewLogger(string? verbosity=null)
        {
            var verbose = new LoggingLevelSwitch(); // .ControlledBy(verbose)
            Log.Logger = new LoggerConfiguration()
                .MinimumLevel.Debug()
                .WriteTo.Console(outputTemplate:
                                "[{Timestamp:HH:mm:ss:ff} {Level:u4}] {Message:1j}{NewLine}{Exception}")
                .WriteTo.File("./logs/autocli.log.txt", rollingInterval: RollingInterval.Minute)
                .CreateLogger();
            return Log.Logger;
        }
    }
}