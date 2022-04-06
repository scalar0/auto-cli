namespace autocli.Interface
{
    /// <summary>
    /// Interface Package class to deserialize the NuGet packages that must be added to the project.
    /// </summary>
    public class IPackage
    {
        [JsonProperty("Name")]
        public string Name { get; set; }

        [JsonProperty("Version")]
        public string? Version { get; set; }
    }
}