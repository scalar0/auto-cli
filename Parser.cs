global using System.CommandLine;
global using System.CommandLine.NamingConventionBinder;
global using System.CommandLine.Parsing;

namespace autocli
{
    public static class Parser
    {
        public static async Task Main(string[] args)
        {
            // ===========================================autocli ROOTCOMMAND===========================================
            RootCommand? ROOTCOMMAND = new();
            ROOTCOMMAND.Description =
                $"__________________________________________________\n\nAUTOCLI : automation for CLI applications creation\n__________________________________________________\n\nAuthor : scalar-tns.\nHost name : {Environment.MachineName}\nOS : {Environment.OSVersion}\nHost version : .NET {Environment.Version}\n\n";

            System.Text.StringBuilder? sb = new();
            sb.Append("[autocli] aims to automate .NET 6.0.* CLI applications development based on an input architecture stored in a .json file.\n").Append("The configuration file stores the architecture for the project's commands, subcommands, options, arguments and properties.\n").Append("\n========================================================================================================================================");
            ROOTCOMMAND.Description += sb.ToString();

            // ===========================================COMMANDS===========================================

            Command? creation = Commands._creation(ROOTCOMMAND);
            Command? generation = Commands._generation(ROOTCOMMAND);

            // ===========================================OPTIONS===========================================

            Option<string>? pushing = Options._pushing(ROOTCOMMAND);

            // ===========================================ARGUMENTS===========================================

            Argument<string>? file_name = Arguments._file_name(ROOTCOMMAND);
            Argument<string>? file_path = Arguments._file_path(ROOTCOMMAND);

            // ===========================================HANDLERS===========================================

            creation.Handler = CommandHandler.Create<string>(Utils.Create);
            generation.Handler = CommandHandler.Create<string>(Utils.Generation);

            // ===========================================INVOKE===========================================

            // Parse the incoming args and invoke the handler
            await ROOTCOMMAND.InvokeAsync(args);
        }
    }
}