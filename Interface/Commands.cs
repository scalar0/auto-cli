namespace autocli.Interface
{
    /// <summary>
    /// Commands class to serialize the commands of the interface.
    /// </summary>
    public class Commands
    {
        [JsonProperty("Alias")]
        public string Alias { get; set; }

        [JsonProperty("Parent")]
        public string Parent { get; set; }

        [JsonProperty("Description")]
        public string Description { get; set; }
    }

    public static partial class Getter
    {
        public static List<Command> GetListCommands(Dictionary<string, dynamic> dict, Properties AppProperties)
        {
            #region Extracting Commands' attributes from json

            const string name = "Commands";
            Log.Verbose("Extracting {entity}", name);
            var ListCommands = dict[name].ToObject<List<Commands>>();

            #endregion Extracting Commands' attributes from json

            #region Building the RootCommand

            RootCommand root = Constructors.BuildRoot(AppProperties);

            #endregion Building the RootCommand

            #region Build loop for the other Commands

            var Commands = new List<Command>()
            {
                root
            };
            foreach (Commands cmd in ListCommands)
            {
                Commands.Add(Constructors.BuildCommand(
                parent: Constructors.Get(Commands, cmd.Parent)!, cmd));
            }
            Log.Debug("Commands built.");

            #endregion Build loop for the other Commands

            return Commands;
        }
    }
}