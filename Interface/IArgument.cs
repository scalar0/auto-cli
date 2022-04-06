namespace autocli.Interface
{
    /// <summary>
    /// Interface Argument class to deserialize the args of the interface.
    /// </summary>
    public class IArgument
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

        public Argument BuildArgument(Command command)
        {
            Argument<string> argument = new(Alias);
            argument.Description = Description;
            if (DefaultValue is not null) argument.SetDefaultValue(DefaultValue);
            try
            {
                command.AddArgument(argument);
                Log.Verbose("{A} built and added to {U}.", $"{argument}", $"{command}");
            }
            catch (Exception ex)
            {
                Log.Error(ex, ex.Message, ex.ToString);
            }
            return argument;
        }
    }

    public partial interface IRetrieve
    {
        public static List<Argument> GetArguments(List<Command> Commands, Dictionary<string, dynamic> dict)
        {
            #region Extracting Arguments' attributes from json

            const string name = "Arguments";
            Log.Verbose("Extracting {entity}", name);
            var ListArguments = dict[name].ToObject<List<IArgument>>();

            #endregion Extracting Arguments' attributes from json

            #region Build loop for the Arguments

            var Arguments = new List<Argument>();
            foreach (IArgument arg in ListArguments)
            {
                Arguments.Add(arg.BuildArgument(command: IConstructor.Get(Commands, arg.Command)!));
            }
            Log.Debug("Arguments built.");

            #endregion Build loop for the Arguments

            return Arguments;
        }
    }
}