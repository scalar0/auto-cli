global using Serilog;
global using System.CommandLine;
global using System.CommandLine.NamingConventionBinder;
global using System.CommandLine.Parsing;

// This file is supposed to be auto-generated

namespace autocli
{
    public static class Parser
    {
        public static async Task Main(string[] args)
        {
            Log.Logger = new LoggerConfiguration()
                .MinimumLevel.Debug()
                .WriteTo.Console()
                .WriteTo.File("./logs/autocli.log.txt", rollingInterval: RollingInterval.Minute)
                .CreateLogger();

            // ===========================================COMMANDS===========================================

            RootCommand command = Builders.MakeRootCommand(
                title: "AUTOCLI : automation for CLI applications interface creation",
                description: "[autocli] aims to automate .NET 6.0.* CLI applications development based on an input architecture stored in a .json file.\nThe configuration file stores the architecture for the project's commands, subcommands, options, arguments and properties.");

            Command creation = Commands._creation(command);
            Command generation = Commands._generation(command);

            // ===========================================OPTIONS===========================================

            Option<string> pushing = Options._pushing(generation);
            Option<DirectoryInfo> dir_choice = Options._dir_choice(creation);

            // ===========================================ARGUMENTS===========================================

            Argument file_name = Arguments._file_name(creation);
            Argument file_path = Arguments._file_path(generation);

            // ===========================================HANDLERS===========================================

            creation.Handler = CommandHandler.Create<string, DirectoryInfo>(Handlers.Create_Template);
            generation.Handler = CommandHandler.Create<string>(Handlers.Generate);

            // ===========================================INVOKE===========================================

            // Parse the incoming args and invoke the handler
            Log.CloseAndFlush();
            await command.InvokeAsync(args);
        }
    }
}