namespace autocli.Interface
{
    [Serializable]
    public class Arguments : Argument
    {
        /// <summary>
        /// The parent command of the argument.
        /// </summary>
        public object Parent { get; set; }

        /// <summary>
        /// The name of the method that will be generated to call the argument.
        /// </summary>
        public string Method { get; set; }

        public Arguments(string symbol)
            : base(symbol)
        {
        }

        /// <summary>
        /// Initializes a new instance of the Arguments class.
        /// </summary>
        /// <param name="symbol">Symbol/command-let of the argument.</param>
        /// <param name="description">Cescription of the argument, shown in help.</param>
        /// <param name="parent">Parent command of the argument</param>
        /// <param name="method">Name of the method calling the argument.</param>
        public Arguments(string symbol,
                          string? description,
                          Command parent,
                          string method)
            : base(symbol, description)
        {
            Parent = parent;
            Method = method;
        }
    }
}