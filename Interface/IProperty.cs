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
    }

    public partial interface IRetrieve
    {
        public static IProperty GetProperties(Dictionary<string, dynamic> dict)
        {
            const string name = "Properties";
            Log.Verbose("Extracting {entity}", name);
            Log.Debug("Properties built.");
            return dict[name].ToObject<List<IProperty>>()[0];
        }
    }
}