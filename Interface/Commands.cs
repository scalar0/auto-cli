namespace autocli.Interface
{
    public class SubCommand : Command
    {
        /// <summary>
        /// The parent command of the subcommand.
        /// </summary>
        public Command Parent;

        public Command GetParent() => Parent;

        /// <summary>
        /// Sets the parent of a command by adding the command to the parent via the AddCommand method.
        /// </summary>
        /// <see cref="Command.AddCommand(Command)"/>
        /// <param name="value">The Parent command of the subcommand.</param>
        public void SetParent(Command value)
        {
            value.AddCommand(this);
        }

        /// <summary>
        /// The name of the method that will be generated to call the subcommand.
        /// </summary>
        public string Method { get; set; }

        /// <summary>
        /// Boolean to set the verbosity.
        /// </summary>
        public bool Verbosity;

        public bool GetVerbosity() => Verbosity;

        /// <summary>
        /// Construct the verbosity option (or not if false).
        /// </summary>
        /// <param name="value">Boolean to set the verbosity.</param>
        public void SetVerbosity(bool value)
        {
            if (value)
            {
                Constructors.MakeOption<string>(
                command: this,
                aliases: new string[] { "--verbosity", "-v" },
                required: false,
                defaultvalue: "m",
                description: "Choix de verbosité de sortie : m[inimal]; d[ebug]."); ;
            }
        }

#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.

        public SubCommand(string symbol)
            : base(symbol)
        {
        }

        /// <summary>
        /// Initializes a new instance of the SubCommand class.
        /// </summary>
        /// <param name="symbol">The symbol/command-let of the command.</param>
        /// <param name="parent">The parent command in the hierarchical order.</param>
        /// <param name="method">The name of the method calling the command.</param>
        /// <param name="verbosity">The verbosity type output to debug in the console.</param>
        /// <param name="description">The description of the command, shown in help.</param>
        public SubCommand(string symbol,
                          Command parent,
                          string method,
                          bool verbosity,
                          string? description)
            : base(symbol, description)
        {
            SetParent(parent);
            Method = method;
            SetVerbosity(verbosity);
        }

#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
    }
}