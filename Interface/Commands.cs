﻿// This file is supposed to be auto-generated

namespace autocli.Interface
{
    public static class Commands
    {
        // TODO:    Parse .json data to option for --output directory for CREATE command
        public static Command _creation(RootCommand command)
        {
            return Builders.MakeCommand(
                command: command,
                symbol: "create",
                description: "Creates a template of a new .json configuration file with specified name.",
                setverbosity: false);
        }

        // BUG:     _generation : args not parsed
        public static Command _generation(RootCommand command)
        {
            return Builders.MakeCommand(
                command: command,
                symbol: "generate",
                description: "Generate the CLI project based on the input .json configuration file.",
                setverbosity: true);
        }
    }
}

//TODO:     Search for Serilog