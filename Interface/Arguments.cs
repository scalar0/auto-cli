namespace autocli.Interface
{
    /// <summary>
    /// Arguments class to serialize the args of the interface.
    /// </summary>
    public class Arguments
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
    }

    public static partial class Getter
    {
        public static List<Argument> GetListArguments(List<Command> Commands, Dictionary<string, dynamic> dict)
        {
            #region Extracting Arguments' attributes from json

            const string name = "Arguments";
            Log.Verbose("Extracting {entity}", name);
            var ListArguments = dict[name].ToObject<List<Arguments>>();

            #endregion Extracting Arguments' attributes from json

            #region Build loop for the Arguments

            var Arguments = new List<Argument>();
            foreach (Arguments arg in ListArguments)
            {
                Arguments.Add(Constructors.BuildArgument<string>(
                command: Constructors.Get(Commands, arg.Command)!, arg));
            }
            Log.Debug("Arguments built.");

            #endregion Build loop for the Arguments

            return Arguments;
        }
    }
}