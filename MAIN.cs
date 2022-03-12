global using System.CommandLine;
global using System.CommandLine.NamingConventionBinder;
global using System.CommandLine.Parsing;

namespace autocli
{
    internal static class MAIN
    {
        private static async Task Main(string[] args)
        {
            //ROOTCOMMAND autocli
            RootCommand? rcom = new();
            rcom.Description =
                $"__________________________________________________\n\nAUTOCLI : automation for CLI applications creation\n__________________________________________________\n\nAuthor : scalar-tns.\nHost name : {Environment.MachineName}\nOS : {Environment.OSVersion}\nHost version : .NET {Environment.Version}\n\n";

            var sb = new System.Text.StringBuilder();
            sb.Append("[autocli] aims to automate .NET 6.0.* CLI applications development based on an input architecture stored in a .json file.\n").Append("The configuration file stores the architecture for the project's commands, subcommands, options and arguments.\n").Append("\n========================================================================================================================================");
            rcom.Description += sb.ToString();

            // ===========================================creation COMMAND===========================================

            //COMMAND
            // Creates a template of a new .json configuration file with specified name.
            Command? creation = new("create");
            creation.Description = "Creates a template of a new .json configuration file with specified name.";
            rcom.AddCommand(creation);

            //ARG file_name (COMMAND creation)
            Argument? file_name = new("name");
            file_name.Description = "Name of .json configuration file.";
            creation.AddArgument(file_name);

            // ===========================================generation COMMAND===========================================

            //COMMAND
            // Generate the CLI project based on the input .json configuration file.
            Command? generation = new("generate");
            generation.Description = "Generate the CLI project based on the input .json configuration file.";
            rcom.AddCommand(generation);

            //ARG file_path (COMMAND generation)
            // Path to .json configuration file.
            Argument? file_path = new("file");
            file_path.Description = "Path to .json configuration file.";
            generation.AddArgument(file_path);

            //OPTION push to github (COMMAND generation)
            // Push to GitHub with project name ? (y/n)
            Option<string>? pushing = new("--push");
            pushing.SetDefaultValue("n");
            pushing.Description = "Push to GitHub with project name ? (y/n)";
            pushing.AddAlias("-p");
            pushing.IsRequired = false;
            generation.AddOption(pushing);

            // ===========================================HANDLERS===========================================

            creation.Handler = CommandHandler.Create<string>(Func.Create);
            generation.Handler = CommandHandler.Create<string>(Func.Generation);

            // ===========================================INVOKE===========================================

            rcom.SetHandler((string file_path) =>
            {
                Console.WriteLine("Test started...");
                Console.WriteLine(Path.GetFileName(file_path));
                Console.WriteLine("Build succesful");
            }, file_path);

            // Parse the incoming args and invoke the handler
            await rcom.InvokeAsync(args);
        }
    }
}