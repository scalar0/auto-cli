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
        public Option BuildOption(Command command)
        {
            Option<string> option = new(Aliases);
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
}