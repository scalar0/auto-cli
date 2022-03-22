namespace autocli.Interface
{
    /// <summary>
    /// Properties class to serialize the properties of the application.
    /// </summary>
    public class Properties
    {
        [JsonProperty("Title")]
        public string Title { get; set; }

        [JsonProperty("Description")]
        public string Description { get; set; }

        [JsonProperty("Outfolder")]
        public string Outfolder { get; set; }

        [JsonProperty("Repo")]
        public string Repo { get; set; }
    }

    /// <summary>
    /// Package class to serialize the NuGet packages that must be added to the project.
    /// </summary>
    public class Package
    {
        [JsonProperty("Name")]
        public string Name { get; }

        [JsonProperty("Version")]
        public string Version { get; }
    }
}