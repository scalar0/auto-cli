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

            RootCommand root = AppProperties.BuildRoot();

            #endregion Building the RootCommand

            #region Build loop for the other Commands

            var Commands = new List<Command>()
            {
                root
            };
            foreach (ICommand cmd in ListCommands)
            {
                Commands.Add(cmd.BuildCommand(
                parent: IConstructor.Get(Commands, cmd.Parent)!));
            }
            Log.Debug("Commands built.");

            #endregion Build loop for the other Commands

            return Commands;
        }
    }
}