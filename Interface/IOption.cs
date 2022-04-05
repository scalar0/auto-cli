namespace autocli.Interface
{
    /// <summary>
    /// Options class to serialize the options of the application.
    /// </summary>
    public class IOption
    {
        [JsonProperty("Name")]
        public string Name { get; set; }

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

    public partial interface IRetrieve
    {
        public static List<Option> GetListOptions(List<Command> Commands, Dictionary<string, dynamic> dict)
        {
            #region Extracting the Options' attributes from json

            const string name = "Options";
            Log.Verbose("Extracting {entity}", name);
            var ListOptions = dict[name].ToObject<List<IOption>>();

            #endregion Extracting the Options' attributes from json

            #region Build loop for the Options

            var Options = new List<Option>();
            foreach (IOption option in ListOptions)
            {
                Options.Add(IConstructor.BuildOption<string>(
                command: IConstructor.Get(Commands, option.Command)!, option));
            }

            /// <summary>
            /// Add verbosity global option
            /// </summary>
            Option<string> verbose = new Option<string>(
                new[] { "--verbose", "-v" }, "Verbosity level of the output : m[inimal]; d[ebug]; v[erbose].")
                .FromAmong("m", "d", "v");
            verbose.SetDefaultValue("m");
            Commands[0].AddGlobalOption(verbose);

            Log.Debug("Options built.");

            #endregion Build loop for the Options

            return Options;
        }
    }
}