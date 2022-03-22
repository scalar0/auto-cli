namespace autocli.Interface
{
    public class Options_ : Option
    {
        /// <summary>
        /// The parent command of the option.
        /// </summary>
        public object Parent { get; set; }

        /// <summary>
        /// The name of the method that will be generated to call the option.
        /// </summary>
        public string Method { get; set; }

#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.

        public Options_(string[] symbols)
#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
            : base(symbols)
        {
        }

        /// <summary>
        /// Initializes a new instance of the Options class.
        /// </summary>
        /// <param name="symbols">Aliases of the option.</param>
        /// <param name="description">Description of the option, shown in help.</param>
        /// <param name="parent">Parent command of the option</param>
        /// <param name="method">Name of the method calling the option.</param>
        public Options_(string[] symbols,
                          string? description,
                          Command parent,
                          string method)
            : base(symbols, description)
        {
            Description = description;
            Parent = parent;
            Method = method;
        }
    }
}