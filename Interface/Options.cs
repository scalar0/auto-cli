namespace autocli.Interface
{
    [Serializable]
    public class Options : Option
    {
        /// <summary>
        /// The parent command of the option.
        /// </summary>
        public object Parent { get; set; }

        /// <summary>
        /// The name of the method that will be generated to call the option.
        /// </summary>
        public string Method { get; set; }

        public Options(string[] symbols)
            : base(symbols)
        {
        }

        // TODO:    Correct the symbols in the json template and Constructors
        /// <summary>
        /// Initializes a new instance of the Options class.
        /// </summary>
        /// <param name="symbols">Aliases of the option.</param>
        /// <param name="description">Description of the option, shown in help.</param>
        /// <param name="parent">Parent command of the option</param>
        /// <param name="method">Name of the method calling the option.</param>
        public Options(string[] symbols,
                          string? description,
                          Command parent,
                          string method)
            : base(symbols, description)
        {
            Parent = parent;
            Method = method;
        }
    }
}