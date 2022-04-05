namespace autocli.Interface
{
    /// <summary>
    /// ICommand class to serialize the commands of the interface.
    /// </summary>
    public class ICommand
    {
        [JsonProperty("Alias")]
        public string Alias { get; set; }

        [JsonProperty("Parent")]
        public string Parent { get; set; }

        [JsonProperty("Description")]
        public string Description { get; set; }
    }

    public partial interface IRetrieve
    {
        public static List<Command> GetListCommands(Dictionary<string, dynamic> dict, IProperty AppProperties)
        {
            #region Extracting Commands' attributes from json

            const string name = "Commands";
            Log.Verbose("Extracting {entity}", name);
            var ListCommands = dict[name].ToObject<List<ICommand>>();

            #endregion Extracting Commands' attributes from json

            #region Building the RootCommand

            RootCommand root = IConstructor.BuildRoot(AppProperties);

            #endregion Building the RootCommand

            #region Build loop for the other Commands

            var Commands = new List<Command>()
            {
                root
            };
            foreach (ICommand cmd in ListCommands)
            {
                Commands.Add(IConstructor.BuildCommand(
                parent: IConstructor.Get(Commands, cmd.Parent)!, cmd));
            }
            Log.Debug("Commands built.");

            #endregion Build loop for the other Commands

            return Commands;
        }
    }
}