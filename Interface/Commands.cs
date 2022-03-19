// This file is supposed to be pasted

namespace autocli.Interface
{
    [Serializable]
    public class SubCommand : Command
    {
        /// <summary>
        /// The parent command of the subcommand.
        /// </summary>
        private readonly Command parent;

        public Command GetParent() => parent;

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
        /// Boolean to set the verbosity of the output on the cli.
        /// </summary>
        private readonly bool verbosity;

        public bool GetVerbosity() => verbosity;

        /// <summary>
        /// Construct the verbosity option (or not if false).
        /// </summary>
        /// <param name="value">Boolean to set the verbosity level</param>
        public Option<string>? SetVerbosity(bool value)
        {
            if (value)
            {
                return Constructors.MakeOption<string>(
                command: this,
                required: false,
                symbols: new string[] { "--verbosity", "-v" },
                defaultvalue: "m",
                description: "Choix de verbosité de sortie : q[uiet]; m[inimal]; diag[nostic].");
            }
            else return null;
        }

        public SubCommand(string symbol)
            : base(symbol)
        {
        }

        /// <summary> Initializes a new instance of the SubCommand class. </summary> <param
        /// name="symbol">The symbol/command-let of the command.</param> <param
        /// name="description">The description of the command, shown in help.</param> <param
        /// name="parent">The parent command in the hierarchical order.</param> <param
        /// name="method">The name of the method calling the command.param> <param
        /// name="verbosity">The verbosity type output to debug in the console.</param>
        public SubCommand(string symbol,
                          string? description,
                          Command parent,
                          bool verbosity,
                          string method)
            : base(symbol, description)
        {
            SetParent(parent);
            Method = method;
            SetVerbosity(verbosity);
        }
    }
}