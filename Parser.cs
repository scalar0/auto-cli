// This file is supposed to be auto-generated
global using Serilog;
global using System.CommandLine;
using autocli.Functionnals;
using autocli.Interface;
using Serilog.Core;
using System.CommandLine.Parsing;

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
            var levelSwitch = new LoggingLevelSwitch(); // .ControlledBy(levelSwitch)
            Log.Logger = new LoggerConfiguration()
                .MinimumLevel.Debug()
                .WriteTo.Console(outputTemplate:
                                "[{Timestamp:HH:mm:ss:ff} {Level:u4}] {Message:1j}{NewLine}{Exception}")
                .WriteTo.File("./logs/autocli.log.txt", rollingInterval: RollingInterval.Minute)
                .CreateLogger();

            var dict = ParseArchitecture.JsonParser(@"C:\Users\matte\source\repos\autoCLI\Properties\autocli.Architecture.json");
            Properties properties = ParseArchitecture.GetProperties(dict);
            List<Package>? ListPackage = ParseArchitecture.GetPackages(dict);
            var ListSubCommand = ParseArchitecture.GetCommands(dict);
            /*            var ListArgument = ParseArchitecture.GetArguments(dict);
                        var ListOption = ParseArchitecture.GetOptions(dict);*/

            // ===========================================COMMANDS===========================================

            RootCommand RootCommand = Constructors.MakeRootCommand(
                title: properties.Title,
                description: properties.Description);

            SubCommand creation = Constructors.MakeCommand(
                parent: RootCommand,
                symbol: "create",
                description: "Creates a template of a new .json configuration file with specified name.",
                verbosity: false);
            SubCommand generation = Constructors.MakeCommand(
                parent: RootCommand,
                symbol: "generate",
                description: "Generate the CLI project based on the input .json configuration file.",
                verbosity: true);

            // https://youtu.be/shES1R7e1lQ

            Log.Debug("Commands and subcommands built.");

            // ===========================================OPTIONS===========================================

            Option<DirectoryInfo> dir_choice = Constructors.MakeOption<DirectoryInfo>(
                command: creation,
                symbols: new string[] { "--directory", "-d" },
                required: true,
                defaultvalue: null,
                description: "Specify the directory output.");
            Option<string> pushing = Constructors.MakeOption<string>(
                command: generation,
                symbols: new string[] { "--push", "-p" },
                required: false,
                defaultvalue: "n",
                description: "Push to GitHub with repo-name ? (y/n)");
            Log.Debug("Options built.");
            // ===========================================ARGUMENTS===========================================

            Argument<string> file_name = Constructors.MakeArgument<string>(
                command: creation,
                symbol: "name",
                defaultvalue: null,
                description: "Name of .json configuration file.");
            Argument<string> file_path = Constructors.MakeArgument<string>(
                command: generation,
                symbol: "file",
                defaultvalue: null,
                description: "Path to .json configuration file.");
            Log.Debug("Arguments built.");
            // ===========================================HANDLERS===========================================

            Log.Debug("Implementing handlers...");

            creation.SetHandler<string, DirectoryInfo>(Handlers.creation, file_name, dir_choice);
            generation.SetHandler<string>(Handlers.generation, file_path, pushing);
            // ===========================================INVOKE===========================================

            // Parse the incoming args and invoke the handler
            Log.Debug("Invoking args...");
            Log.CloseAndFlush();

            await RootCommand.InvokeAsync(args);
        }
    }
}