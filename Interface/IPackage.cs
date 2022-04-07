namespace autocli.Interface
{
    /// <summary>
    /// Interface Package class to deserialize the NuGet packages that must be added to the project.
    /// </summary>
    public class IPackage
    {
        [JsonProperty("Name")]
        internal string Name { get; set; }

        [JsonProperty("Version")]
        internal string? Version { get; set; }
    }
}