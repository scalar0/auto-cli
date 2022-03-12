global using System.CommandLine;
global using System.CommandLine.NamingConventionBinder;
global using System.CommandLine.Parsing;

namespace autocli
{
    public static class Parser
    {
        public static async Task Main(string[] args)
        {
            // ===========================================ROOTCOMMAND===========================================

            RootCommand ROOTCOMMAND = Builders.MakeRootCommand(
                title: "AUTOCLI : automation for CLI applications interface creation\n",
                description: "[autocli] aims to automate .NET 6.0.* CLI applications development based on an input architecture stored in a .json file.\nThe configuration file stores the architecture for the project's commands, subcommands, options, arguments and properties.\n");

            // ===========================================COMMANDS===========================================

            Command? creation = Commands._creation(ROOTCOMMAND);
            Command? generation = Commands._generation(ROOTCOMMAND);
            Command? testing = Commands._testing(ROOTCOMMAND);

            // ===========================================OPTIONS===========================================

            Option<string>? pushing = Options._pushing(generation);

            // ===========================================ARGUMENTS===========================================

            Argument<string>? file_name = Arguments._file_name(creation);
            Argument<string>? file_path = Arguments._file_path(generation);
            Argument<string>? test_path = Arguments._test_path(testing);

            // ===========================================HANDLERS===========================================

            creation.Handler = CommandHandler.Create<string>(Handlers.Create);
            testing.Handler = CommandHandler.Create<string>(Handlers.Test);
            generation.Handler = CommandHandler.Create<string>(Handlers.Generate);

            // ===========================================INVOKE===========================================

            // Parse the incoming args and invoke the handler
            await ROOTCOMMAND.InvokeAsync(args);
        }
    }
}