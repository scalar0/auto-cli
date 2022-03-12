namespace autocli
{
    internal static class Builders
    {
        internal static Command? MakeCommand(
            Command? command,
            string symbol,
            string description)
        {
            try
            {
                if (command != null)
                {
                    Command? cmd = new(symbol);
                    cmd.Description = description;
                    command.AddCommand(cmd);
                    return cmd;
                }
            }
            catch (Exception)
            {
#pragma warning disable CA2208 // Instantiate argument exceptions correctly
                throw new ArgumentNullException("Command is null here");
#pragma warning restore CA2208 // Instantiate argument exceptions correctly
            }

            return null;
        }

        internal static Argument? MakeArgument<T>(
            Command? command,
            string symbol,
            string defaultvalue,
            string description)
        {
            try
            {
                if (command != null)
                {
                    Argument<T>? argument = new(symbol);
                    argument.Description = description;
                    argument.SetDefaultValue(defaultvalue);
                    command.AddArgument(argument);
                    return argument;
                }
            }
            catch (Exception)
            {
#pragma warning disable CA2208 // Instantiate argument exceptions correctly
                throw new ArgumentNullException("Input Command is null");
#pragma warning restore CA2208 // Instantiate argument exceptions correctly
            }

            return null;
        }

        internal static Option? MakeOption<T>(
            Command? command,
            bool required,
            string symbol,
            string alias,
            string defaultvalue,
            string description)
        {
            try
            {
                if (command != null)
                {
                    Option<T>? option = new(symbol);
                    option.SetDefaultValue(defaultvalue);
                    option.Description = description;
                    if (alias != null) option.AddAlias(alias);
                    option.IsRequired = required;
                    command.AddOption(option);
                    return option;
                }

                return null;
            }
            catch (Exception)
            {
#pragma warning disable CA2208 // Instantiate argument exceptions correctly
                throw new ArgumentNullException("Input Command is null");
#pragma warning restore CA2208 // Instantiate argument exceptions correctly
            }
        }
    }
}