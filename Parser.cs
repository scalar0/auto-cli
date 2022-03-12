global using System.CommandLine;
global using System.CommandLine.NamingConventionBinder;
global using System.CommandLine.Parsing;

namespace autocli
{
    internal static class Parser
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

            // ===========================================creation COMMAND===========================================

            //COMMAND
            // Creates a template of a new .json configuration file with specified name.
            Command? creation = Builders.MakeCommand(
                command: ROOTCOMMAND,
                symbol: "create",
                description: "Creates a template of a new .json configuration file with specified name.");

            //ARG file_name (COMMAND creation)
#pragma warning disable CS8604 // Possible null reference argument.
            Argument<string>? file_name = Builders.MakeArgument(
                command: creation,
                symbol: "name",
                defaultvalue: Path.GetDirectoryName(Environment.CurrentDirectory),
                description: "Name of .json configuration file.") as Argument<string>;

            creation?.SetHandler<string>((string file_name) => Utils.Create(file_name), file_name);

            // ===========================================generation COMMAND===========================================

            //COMMAND
            // Generate the CLI project based on the input .json configuration file.
            Command? generation = Builders.MakeCommand(
                command: ROOTCOMMAND,
                symbol: "generate",
                description: "Generate the CLI project based on the input .json configuration file.");

            //ARG file_path (COMMAND generation)
            Argument<string>? file_path = Builders.MakeArgument(
                command: generation,
                symbol: "file",
                defaultvalue: Utils.Locate("", "*.json"),
                description: "Path to .json configuration file.") as Argument<string>;

            //OPTION push to github (COMMAND generation)
            Option<string>? pushing = Builders.MakeOption(
                command: generation,
                required: false,
                symbol: "--push",
                alias: "-p",
                defaultvalue: "n",
                description: "Push to GitHub with project name ? (y/n)") as Option<string>;

            generation?.SetHandler<string>((string file_path) => Utils.Generation(file_path), file_path);
#pragma warning restore CS8604 // Possible null reference argument.

            // ===========================================HANDLERS===========================================

#pragma warning disable CS8602 // Dereference of a possibly null reference.
            creation.Handler = CommandHandler.Create<string>(Utils.Create);
            generation.Handler = CommandHandler.Create<string>(Utils.Generation);
#pragma warning restore CS8602 // Dereference of a possibly null reference.

            // ===========================================INVOKE===========================================

            // Parse the incoming args and invoke the handler
            await ROOTCOMMAND.InvokeAsync(args);
        }
    }
}