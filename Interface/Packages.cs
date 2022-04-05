namespace autocli.Interface
{
    /// <summary>
    /// Package class to serialize the NuGet packages that must be added to the project.
    /// </summary>
    public class Packages
    {
        [JsonProperty("Name")]
        public string Name { get; set; }

        [JsonProperty("Version")]
        public string? Version { get; set; }
    }

    public static partial class Getter
    {
        public static List<Packages> GetPackages(Dictionary<string, dynamic> dict)
        {
            const string name = "Packages";
            Log.Verbose("Extracting {entity}", name);
            Log.Debug("Packages built.");
            return dict[name].ToObject<List<Packages>>();
        }
    }
}