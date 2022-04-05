namespace autocli.Interface
{
    /// <summary>
    /// Option Interface class to deserialize the options of the application.
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

        /// <summary>
        /// Constructs a new instance of the IOption class.
        /// </summary>
        /// <typeparam name="T">Type of the option.</typeparam>
        /// <param name="command">Parent command.</param>
        /// <returns>Corresponding Option.</returns>
        public Option<T> BuildOption<T>(Command command)
        {
            Option<T> option = new(Aliases);
            option.Name = Name;
            option.IsRequired = Required;
            option.Description = Description;
            if (DefaultValue is not null) option.SetDefaultValue(DefaultValue);
            try
            {
                command.AddOption(option);
                Log.Verbose("{O} built and added to {U}.", $"{option}", $"{command}");
            }
            catch (Exception ex)
            {
                Log.Error(ex, ex.Message, ex.ToString);
            }
            return option;
        }
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
                Options.Add(option.BuildOption<string>(
                command: IConstructor.Get(Commands, option.Command)!));
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