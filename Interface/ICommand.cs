namespace autocli.Interface
{
    /// <summary>
    /// Interface Command class to deserialize the commands of the interface.
    /// </summary>
    public class ICommand
    {
        [JsonProperty("Alias")]
        public string Alias { get; set; }

        [JsonProperty("Parent")]
        public string Parent { get; set; }

        [JsonProperty("Description")]
        public string Description { get; set; }

        /// <summary>
        /// Constructs a new instance of the Command class.
        /// </summary>
        /// <param name="parent">Parent Command.</param>
        /// <returns>Corresponding Command.</returns>
        public Command BuildCommand(Command parent)
        {
            Command cmd = new(Alias);
            cmd.Description = Description;
            try
            {
                parent.AddCommand(cmd);
                Log.Verbose("{C} built and added to {U}.", $"{cmd}", $"{parent}");
            }
            catch (Exception ex)
            {
                Log.Error(ex, ex.Message, ex.ToString);
            }
            return cmd;
        }
    }
}