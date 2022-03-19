// This file is supposed to be pasted

namespace autocli.Interface
{
    [Serializable]
    public class SubCommands
    {
        public string Name { get; set; }
        public string Parent { get; set; }
        public string Method { get; set; }
        public string Description { get; set; }
        public bool Verbosity { get; set; }
    }

    [Serializable]
    public sealed class SubCommand : Command
    {
        private readonly Command parent;
        public Command Parent { get => parent; set => value.AddCommand(this); }
        public string Method { get; }
        public bool Verbosity { get; }

        public Option<string>? SetVerbosity(bool value)
        {
            if (value)
            {
                return Constructors.MakeOption<string>(
                command: this,
                required: false,
                symbol: "--verbosity",
                alias: "-v",
                defaultvalue: "m",
                description: "Choix de verbosité de sortie : q[uiet]; m[inimal]; diag[nostic].");
            }
            else return null;
        }

        public SubCommand(string symbol)
            : base(symbol)
        {
        }

        /// <summary> Initializes a new instance of the Command class. </summary> <param
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
            Name = symbol;
            Parent = parent;
            Method = method;
            Description = description;
            SetVerbosity(verbosity);
        }
    }
}