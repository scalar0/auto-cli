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
            // ===========================================COMMANDS===========================================

            RootCommand command = Builders.MakeRootCommand(
                title: "AUTOCLI : automation for CLI applications interface creation\n",
                description: "[autocli] aims to automate .NET 6.0.* CLI applications development based on an input architecture stored in a .json file.\nThe configuration file stores the architecture for the project's commands, subcommands, options, arguments and properties.\n");

            Command creation = Commands._creation(command);
            Command generation = Commands._generation(command);
            Command testing = Commands._testing(command);

            // ===========================================OPTIONS===========================================

            Options._verbosity(generation);
            Options._pushing(generation);
            Options._dir_choice(creation);

            // ===========================================ARGUMENTS===========================================

            Arguments._file_name(creation);
            Arguments._file_path(generation);
            Arguments._test_path(testing);

            // ===========================================HANDLERS===========================================

            creation.Handler = CommandHandler.Create<string, DirectoryInfo>(Handlers.Create_Template);
            testing.Handler = CommandHandler.Create<string>(Handlers.Test);
            generation.Handler = CommandHandler.Create<FileInfo>(Handlers.Generate);

            // ===========================================INVOKE===========================================

            // Parse the incoming args and invoke the handler
            await command.InvokeAsync(args);
        }
    }
}