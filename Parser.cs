// This file is supposed to be auto-generated
global using Serilog;
global using System.CommandLine;
global using System.CommandLine.Parsing;
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
            var levelSwitch = new LoggingLevelSwitch(); // .ControlledBy(levelSwitch)
            Log.Logger = new LoggerConfiguration()
                .MinimumLevel.Debug()
                .WriteTo.Console(outputTemplate:
                                "[{Timestamp:HH:mm:ss:ff} {Level:u4}] {Message:1j}{NewLine}{Exception}")
                .WriteTo.File("./logs/autocli.log.txt", rollingInterval: RollingInterval.Minute)
                .CreateLogger();

            // ===========================================COMMANDS===========================================

            RootCommand command = Constructors.MakeRootCommand(
                title: "AUTOCLI : automation for CLI applications interface creation",
                description: "[autocli] aims to automate .NET 6.0.* CLI applications development based on an input architecture stored in a .json file.\nThe configuration file stores the architecture for the project's commands, subcommands, options, arguments and properties.");

            SubCommand creation = Constructors.MakeCommand(
                parent: command,
                symbol: "create",
                description: "Creates a template of a new .json configuration file with specified name.",
                verbosity: false);
            SubCommand generation = Constructors.MakeCommand(
                parent: command,
                symbol: "generate",
                description: "Generate the CLI project based on the input .json configuration file.",
                verbosity: true);
            Log.Debug("Commands and subcommands built.");

            /*            List<SubCommand>? scom = ParseArchitecture.GetSubCommands(@"C:\Users\matte\source\repos\autoCLI\Properties\subcom.json");
                        if (scom is not null) foreach (SubCommand com in scom)
                            {
                                Log.Debug($"{com}");
                            }*/
            // ===========================================OPTIONS===========================================

            Option<DirectoryInfo> dir_choice = Constructors.MakeOption<DirectoryInfo>(
                command: creation,
                required: true,
                symbols: new string[] { "--directory", "-d" },
                defaultvalue: null,
                description: "Specify the directory output.");
            Option<string> pushing = Constructors.MakeOption<string>(
                command: generation,
                required: false,
                symbols: new string[] { "--push", "-p" },
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
                description: "Path to .json configuration file."); ;
            Log.Debug("Arguments built.");
            // ===========================================HANDLERS===========================================

            Log.Debug("Implementing handlers...");
            creation.SetHandler<string, DirectoryInfo>(Handlers.Create_Template,
                file_name,
                dir_choice);
            generation.SetHandler<string>(Handlers.Generate,
                file_path,
                pushing);
            // ===========================================INVOKE===========================================

            // Parse the incoming args and invoke the handler
            Log.Debug("Invoking args...");
            Log.CloseAndFlush();

            await command.InvokeAsync(args);
        }
    }
}