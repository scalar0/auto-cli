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

            List<SubCommands>? scom = Utils.GetSubCommands(@"C:\Users\matte\source\repos\autoCLI\Properties\subcom.json");
            if (scom is not null) foreach (SubCommands com in scom)
                {
                    Log.Debug($"{com}");
                }
            // ===========================================OPTIONS===========================================

            Option<DirectoryInfo> dir_choice = Options._dir_choice(creation);
            Option<string> pushing = Options._pushing(generation);
            Log.Debug("Options built.");
            // ===========================================ARGUMENTS===========================================

            Argument<string> file_name = Arguments._file_name(creation);
            Argument<string> file_path = Arguments._file_path(generation);
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