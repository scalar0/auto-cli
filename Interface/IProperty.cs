namespace autocli.Interface
{
    /// <summary>
    /// Properties class to serialize the properties of the application.
    /// </summary>
    public class IProperty
    {
        [JsonProperty("Name")]
        internal string? Name { get; set; }

        [JsonProperty("Title")]
        internal string Title { get; set; }

        [JsonProperty("Description")]
        internal string Description { get; set; }

        [JsonProperty("OutputPath")]
        internal string OutputPath { get; set; }

        [JsonProperty("Repo")]
        internal string Repo { get; set; }

        /// <summary>
        /// Constructs a new instance of the RootCommand class.
        /// </summary>
        /// <returns>Corresponding RootCommand.</returns>
        internal RootCommand BuildRoot()
        {
            RootCommand root = new(Functionnals.Utils.Boxed(Title) + Description + "\n");
            root.Name = Name;
            root.SetHandler(() => root.InvokeAsync("-h"));
            Log.Debug("RootCommand built.");
            return root;
        }
    }
}