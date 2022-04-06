namespace autocli.Interface
{
    /// <summary>
    /// The IConstructor interface creates each entity needed for the application interface on the
    /// CLI. For each method the arguments must be parsed from .json configuration file.
    /// </summary>
    public interface IConstructor
    {
        /// <summary>
        /// Retrieves the command or argument from the input list.
        /// </summary>
        /// <param name="L">Input list.</param>
        /// <param name="alias">Alias of the searched entity.</param>
        public static T? Get<T>(List<T> L, string alias)
        {
            foreach (dynamic? item in L)
                if (item!.Name == alias)
                {
                    Log.Verbose("Accessing {C}.", $"{item}");
                    return item;
                }
            return default;
        }
    }
}