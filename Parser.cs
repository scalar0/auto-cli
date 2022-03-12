﻿global using System.CommandLine;
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
            Command? creation = new("create");
            creation.Description = "Creates a template of a new .json configuration file with specified name.";
            ROOTCOMMAND.AddCommand(creation);

            //ARG file_name (COMMAND creation)
            Argument? file_name = new("name");
            file_name.Description = "Name of .json configuration file.";
            file_name.SetDefaultValue(Path.GetDirectoryName(Environment.CurrentDirectory));
            creation.AddArgument(file_name);

            creation.SetHandler<string>((string file_name) => Utils.Create(file_name), file_name);

            // ===========================================generation COMMAND===========================================

            //COMMAND
            // Generate the CLI project based on the input .json configuration file.
            Command? generation = new("generate");
            generation.Description = "Generate the CLI project based on the input .json configuration file.";
            ROOTCOMMAND.AddCommand(generation);

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

            generation.SetHandler<string>((string file_path) => Utils.Generation(file_path), file_path);

            // ===========================================HANDLERS===========================================

            creation.Handler = CommandHandler.Create<string>(Utils.Create);

            generation.Handler = CommandHandler.Create<string>(Utils.Generation);

            // ===========================================INVOKE===========================================

            // Parse the incoming args and invoke the handler
            await ROOTCOMMAND.InvokeAsync(args);
        }
    }
}