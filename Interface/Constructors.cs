// This file must be pasted in ther interface folder

namespace autocli
{
    public static class Builders
    {
        public static RootCommand MakeRootCommand(
            string title,
            string description)
        {
            RootCommand ROOTCOMMAND = new();
            ROOTCOMMAND.Description = Utils.Boxed(title) + description + "\n";
            ROOTCOMMAND.Handler = CommandHandler.Create(() => ROOTCOMMAND.Invoke("-h"));
            Log.Debug("RootCommand built.");
            return ROOTCOMMAND;
        }

        public static Command MakeCommand(
            Command command,
            string symbol,
            string description)
        {
            Command cmd = new(symbol);
            cmd.Description = description;
            // Adding command
            command.AddCommand(cmd);
            Log.Debug("Command {symbol} built and added to {Command}.", symbol, command);
            return cmd;
        }

        public static Argument<T> MakeArgument<T>(
            Command command,
            string symbol,
            string? defaultvalue,
            string description)
        {
            Argument<T> argument = new(symbol);
            if (defaultvalue != null) argument.SetDefaultValue(defaultvalue);
            argument.Description = description;
            // Adding argument
            command.AddArgument(argument);
            Log.Debug("Argument {Arg} built and added to {Command}.", symbol, command);
            return argument;
        }

        public static Option<T> MakeOption<T>(
            Command command,
            bool required,
            string symbol,
            string? alias,
            string? defaultvalue,
            string description)
        {
            Option<T> option = new(symbol);
            option.IsRequired = required;
            if (alias != null) option.AddAlias(alias);
            if (defaultvalue != null) option.SetDefaultValue(defaultvalue);
            option.Description = description;
            // Adding option
            command.AddOption(option);
            Log.Debug("Option {symbol} built and added to {Command}.", symbol, command);
            return option;
        }
    }

    // TODO:    Template for each entity
    public static class Templates
    {
        public static void _RootCommand(
            string title,
            string description)
        {
        }

        public static void _Command(
            Type T,
            Command command,
            string symbol,
            string? defaultvalue,
            string description)
        {
        }

        public static void _Argument(
            Type T,
            Command command,
            string symbol,
            string? defaultvalue,
            string description)
        {
        }

        public static void _Option(
            Type T,
            Command command,
            bool required,
            string symbol,
            string? alias,
            string? defaultvalue,
            string description)
        {
        }
    }
}