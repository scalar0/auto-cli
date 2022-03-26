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
    public class Packages
    {
        [JsonProperty("Name")]
        public string Name { get; set; }

        [JsonProperty("Version")]
        public string Version { get; set; }
    }

    /// <summary>
    /// Commands class to serialize the commands of the interface.
    /// </summary>
    public class Commands
    {
        [JsonProperty("Alias")]
        public string Alias { get; set; }

        [JsonProperty("Parent")]
        public string Parent { get; set; }

        [JsonProperty("Verbosity")]
        public bool Verbosity { get; set; }

        [JsonProperty("Description")]
        public string Description { get; set; }
    }

    /// <summary>
    /// Arguments class to serialize the args of the interface.
    /// </summary>
    public class Arguments
    {
        [JsonProperty("Alias")]
        public string Alias { get; set; }

        [JsonProperty("Type")]
        public string Type { get; set; }

        [JsonProperty("Command")]
        public string Command { get; set; }

        [JsonProperty("DefautlValue")]
        public string? DefaultValue { get; set; }

        [JsonProperty("Description")]
        public string Description { get; set; }
    }

    /// <summary>
    /// Options class to serialize the options of the application.
    /// </summary>
    public class Options
    {
        [JsonProperty("Aliases")]
        public string[] Aliases { get; set; }

        [JsonProperty("Type")]
        public string Type { get; set; }

        [JsonProperty("Command")]
        public string Command { get; set; }

        [JsonProperty("Required")]
        public bool Required { get; set; }

        [JsonProperty("DefautlValue")]
        public string? DefaultValue { get; set; }

        [JsonProperty("Description")]
        public string Description { get; set; }
    }
}