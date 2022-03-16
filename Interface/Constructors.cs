// This file must be pasted in ther interface folder

using autocli.Functionnals;

namespace autocli.Interface
{
    public static class Builders
    {
        public static RootCommand MakeRootCommand(
            string title,
            string description)
        {
            RootCommand ROOTCOMMAND = new()
            {
                Description = Utils.Boxed(title) + description + "\n"
            };
            ROOTCOMMAND.Handler = CommandHandler.Create(() => ROOTCOMMAND.Invoke("-h"));
            Log.Debug("RootCommand built.");
            return ROOTCOMMAND;
        }

        public static Command MakeCommand(
            Command command,
            string symbol,
            string description,
            bool setverbosity)
        {
            Command cmd = new(symbol)
            {
                Description = description
            };
            try
            {
                command.AddCommand(cmd);
                string upco = $"{command}";
                Log.Debug("Command {symbol} built and added to {upco}.", symbol, upco);
            }
            catch (Exception ex)
            {
                Log.Error(ex, ex.Message, ex.ToString);
            }
            if (setverbosity) SetVerbosity(cmd);
            return cmd;
        }

        public static Argument<T> MakeArgument<T>(
            Command command,
            string symbol,
            string? defaultvalue,
            string description)
        {
            Argument<T> argument = new(symbol)
            {
                Description = description
            };
            if (defaultvalue != null) argument.SetDefaultValue(defaultvalue);
            try
            {
                command.AddArgument(argument);
                string upco = $"{command}";
                Log.Debug("Argument {symbol} built and added to {upco}.", symbol, upco);
            }
            catch (Exception ex)
            {
                Log.Error(ex, ex.Message, ex.ToString);
            }
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
            Option<T> option = new(symbol)
            {
                IsRequired = required,
                Description = description
            };
            if (alias != null) option.AddAlias(alias);
            if (defaultvalue != null) option.SetDefaultValue(defaultvalue);
            try
            {
                command.AddOption(option);
                string upco = $"{command}";
                Log.Debug("Option {symbol} built and added to {upco}.", symbol, upco);
            }
            catch (Exception ex)
            {
                Log.Error(ex, ex.Message, ex.ToString);
            }
            return option;
        }

        // Implement verbosity option
        public static Option<string> SetVerbosity(Command command)
        {
            return MakeOption<string>(
                command: command,
                required: false,
                symbol: "--verbosity",
                alias: "-v",
                defaultvalue: "m",
                description: "Choix de verbosité de sortie : q[uiet]; m[inimal]; diag[nostic].");
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
            Command command,
            string symbol,
            string? defaultvalue,
            string description,
            bool setverbosity)
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