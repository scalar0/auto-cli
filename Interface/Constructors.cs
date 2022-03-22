using autocli.Functionnals;

namespace autocli.Interface
{
    /// <summary>
    /// The Constructors class creates each entity needed for the application interface on the CLI.
    /// For each method the arguments must be parsed from .json configuration file.
    /// </summary>
    public static class Constructors
    {
        /// <summary>
        /// Constructs a new instance of the RootCommand class.
        /// </summary>
        /// <param name="title">Describe the application title displayed in the console output.</param>
        /// <param name="description">Description of the application purpose.</param>
        /// <returns>Corresponding RootCommand.</returns>
        public static RootCommand MakeRootCommand(
            string name,
            string title,
            string description)
        {
            RootCommand rcom = new(Utils.Boxed(title) + description + "\n");
            rcom.Name = name;
            rcom.SetHandler(() => rcom.Invoke("-h"));
            Log.Debug("RootCommand built.");
            return rcom;
        }

        /// <summary>
        /// Constructs a new instance of the SubCommand class.
        /// </summary>
        /// <param name="parent">Parent Command.</param>
        /// <param name="alias">Command-let of the command.</param>
        /// <param name="description"></param>
        /// <param name="verbosity">Output verbosity for debugging.</param>
        /// <returns>Corresponding SubCommand.</returns>
        public static SubCommand MakeCommand(
            Command parent,
            string alias,
            string description,
            bool verbosity)
        {
            SubCommand cmd = new(alias);
            try
            {
                cmd.Description = description;
                cmd.SetParent(parent);
                cmd.SetVerbosity(verbosity);
                Log.Debug("{C} built and added to {U}.", $"{cmd}", $"{parent}");
            }
            catch (Exception ex)
            {
                Log.Error(ex, ex.Message, ex.ToString);
            }
            return cmd;
        }

        public static Command? GetCommand(List<Command> L, string name)
        {
            foreach (Command com in L) if (com.Name == name) return com;
            return null;
        }

        /// <summary>
        /// Constructs a new instance of the Argument class.
        /// </summary>
        /// <typeparam name="T">Type fo the argument.</typeparam>
        /// <param name="command">Parent command for the argument.</param>
        /// <param name="alias">Argument's name.</param>
        /// <param name="defaultvalue">Default value of the argument (none if null).</param>
        /// <param name="description"></param>
        /// <returns>Corresponding Argument.</returns>
        public static Argument<T> MakeArgument<T>(
            Command command,
            string alias,
            string? defaultvalue,
            string description)
        {
            Argument<T> argument = new(alias);
            try
            {
                argument.Description = description;
                if (defaultvalue is not null) argument.SetDefaultValue(defaultvalue);
                command.AddArgument(argument);
                Log.Debug("{A} built and added to {U}.", $"{argument}", $"{command}");
            }
            catch (Exception ex)
            {
                Log.Error(ex, ex.Message, ex.ToString);
            }
            return argument;
        }

        /// <summary>
        /// Constructs a new instance of the Option class.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="command">Parent command.</param>
        /// <param name="required">
        /// Boolean to specify if the option must be required by the cli parser.
        /// </param>
        /// <param name="aliases">Aliases of the option.</param>
        /// <param name="defaultvalue"></param>
        /// <param name="description"></param>
        /// <returns>Corresponding Option.</returns>
        public static Option<T> MakeOption<T>(
            Command command,
            string[] aliases,
            bool required,
            string? defaultvalue,
            string description)
        {
            Option<T> option = new(aliases);
            if (defaultvalue is not null) option.SetDefaultValue(defaultvalue);
            try
            {
                option.IsRequired = required;
                option.Description = description;
                command.AddOption(option);
                Log.Debug("{O} built and added to {U}.", $"{option}", $"{command}");
            }
            catch (Exception ex)
            {
                Log.Error(ex, ex.Message, ex.ToString);
            }
            return option;
        }
    }
}