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
            ROOTCOMMAND.Description = Utils.Boxed(title) + description;
            ROOTCOMMAND.Handler = CommandHandler.Create(() => ROOTCOMMAND.Invoke("-h"));

            return ROOTCOMMAND;
        }

        public static Command MakeCommand(
            Command command,
            string symbol,
            string description)
        {
            Command cmd = new(symbol);
            cmd.Description = description;
            command.AddCommand(cmd);
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
            command.AddArgument(argument);
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
            command.AddOption(option);
            return option;
        }
    }

    // TODO:    Template for each entity
    public static class Templates
    {
        public static void _Command()
        {
        }

        public static void _Argument()
        {
        }

        public static void _Option()
        {
        }
    }
}