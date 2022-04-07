namespace autocli.Interface
{
    /// <summary>
    /// Option Interface class to deserialize the options of the application.
    /// </summary>
    internal class IOption
    {
        [JsonProperty("Name")]
        internal string Name { get; set; }

        [JsonProperty("Aliases")]
        internal string[] Aliases { get; set; }

        [JsonProperty("Type")]
        internal string Type { get; set; }

        [JsonProperty("Command")]
        internal string Command { get; set; }

        [JsonProperty("Required")]
        internal bool Required { get; set; }

        [JsonProperty("DefautlValue")]
        internal string? DefaultValue { get; set; }

        [JsonProperty("Description")]
        internal string Description { get; set; }

        /// <summary>
        /// Constructs a new instance of the IOption class.
        /// </summary>
        /// <typeparam name="T">Type of the option.</typeparam>
        /// <param name="command">Parent command.</param>
        /// <returns>Corresponding Option.</returns>
        internal Option BuildOption(Command command)
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