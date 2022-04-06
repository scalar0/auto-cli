namespace autocli.Interface
{
    /// <summary>
    /// Properties class to serialize the properties of the application.
    /// </summary>
    public class IProperty
    {
        [JsonProperty("Name")]
        public string? Name { get; set; }

        [JsonProperty("Title")]
        public string Title { get; set; }

        [JsonProperty("Description")]
        public string Description { get; set; }

        [JsonProperty("OutputPath")]
        public string OutputPath { get; set; }

        [JsonProperty("Repo")]
        public string Repo { get; set; }

        /// <summary>
        /// Constructs a new instance of the RootCommand class.
        /// </summary>
        /// <returns>Corresponding RootCommand.</returns>
        public RootCommand BuildRoot()
        {
            RootCommand rcom = new(Functionnals.Utils.Boxed(Title) + Description + "\n");
            rcom.Name = Name;
            Log.Debug("RootCommand built.");
            return rcom;
        }
    }
}