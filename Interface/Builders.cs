namespace autocli
{
    public static class Builders
    {
        public static RootCommand MakeRootCommand(
            string title,
            string description)
        {
            RootCommand ROOTCOMMAND = new();
            ROOTCOMMAND.Description = Utils.Boxed(title);
            ROOTCOMMAND.Description += description;
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

        public static Argument MakeArgument(
            Command command,
            string symbol,
            string? defaultvalue,
            string description)
        {
            Argument argument = new(symbol);
            argument.Description = description;
            if (defaultvalue != null)
                argument.SetDefaultValue(defaultvalue);
            command.AddArgument(argument);
            return argument;
        }

        public static Option MakeOption(
            Command command,
            bool required,
            string symbol,
            string? alias,
            string defaultvalue,
            string description)
        {
            Option option = new(symbol);
            option.SetDefaultValue(defaultvalue);
            option.Description = description;
            if (alias != null) option.AddAlias(alias);
            option.IsRequired = required;
            command.AddOption(option);
            return option;
        }
    }
}