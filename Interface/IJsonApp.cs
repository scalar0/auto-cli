namespace autocli.Interface
{
    public class IJsonApp
    {
        #region Configuration

        internal readonly Dictionary<string, dynamic> configuration;

        internal Dictionary<string, dynamic> GetConfiguration() => configuration;

        #endregion Configuration

        #region Properties

        internal readonly IProperty properties;

        internal IProperty GetProperties() => properties;

        internal IProperty ConstructProperties()
        {
            const string name = "Properties";
            Log.Verbose("Extracting {entity}", name);
            Log.Debug("Properties built.");
            return GetConfiguration()[name].ToObject<List<IProperty>>()[0];
        }

        #endregion Properties

        #region Packages

        internal readonly List<IPackage> packages;

        internal List<IPackage> GetPackages() => packages;

        internal List<IPackage> ConstructPackages()
        {
            const string name = "Packages";
            Log.Verbose("Extracting {entity}", name);
            Log.Debug("Packages built.");
            return GetConfiguration()[name].ToObject<List<IPackage>>();
        }

        #endregion Packages

        #region Commands

        internal readonly List<Command> commands;

        internal List<Command> GetCommands() => commands;

        internal RootCommand GetRootCommand() => (RootCommand)GetCommands()[0];

        internal Command GetCommand(string name)
        {
            foreach (Command item in GetCommands())
                if (item!.Name == name)
                {
                    Log.Verbose("Accessing {C}.", $"{item}");
                    return item;
                }
            Log.Error("No command of name: {name} found.", name);
            return default!;
        }

        internal List<Command> ConstructCommands()
        {
            #region Extracting Commands' attributes from json

            const string name = "Commands";
            Log.Verbose("Extracting {entity}", name);
            var ListCommands = GetConfiguration()[name].ToObject<List<ICommand>>();

            #endregion Extracting Commands' attributes from json

            #region Build loop for the Commands

            var Commands = new List<Command>()
            {
                GetProperties().BuildRoot()
            };
            foreach (ICommand cmd in ListCommands)
            {
                Commands.Add(cmd.BuildCommand(Commands.Find(el => el.Name.Equals(cmd.Parent))!));
            }
            Log.Debug("Commands built.");

            #endregion Build loop for the Commands

            return Commands;
        }

        #endregion Commands

        #region Options

        internal readonly List<Option> options;

        internal List<Option> GetOptions() => options;

        internal Option GetOption(string name)
        {
            foreach (Option item in GetOptions())
                if (item!.Name == name)
                {
                    Log.Verbose("Accessing {C}.", $"{item}");
                    return item;
                }
            Log.Error("No option of name: {name} found.", name);
            return default!;
        }

        internal List<Option> ConstructOptions()
        {
            #region Extracting the Options' attributes from json

            const string name = "Options";
            Log.Verbose("Extracting {entity}", name);
            var ListOptions = GetConfiguration()[name].ToObject<List<IOption>>();

            #endregion Extracting the Options' attributes from json

            #region Build loop for the Options

            var Options = new List<Option>();
            foreach (IOption option in ListOptions)
            {
                Options.Add(option.BuildOption(GetCommands().Find(el => el.Name.Equals(option.Command))!));
            }

            /// <summary>
            /// Add verbosity global option
            /// </summary>
            Option<string> verbose = new Option<string>(
                new[] { "--verbose", "-v" }, "Verbosity level of the output : m[inimal]; d[ebug]; v[erbose].")
                .FromAmong("m", "d", "v");
            verbose.SetDefaultValue("m");
            GetCommands()[0].AddGlobalOption(verbose);

            Log.Debug("Options built.");

            #endregion Build loop for the Options

            return Options;
        }

        #endregion Options

        #region Arguments

        internal readonly List<Argument> arguments;

        internal List<Argument> GetArguments() => arguments;

        internal Argument GetArgument(string name)
        {
            foreach (Argument item in GetArguments())
                if (item!.Name == name)
                {
                    Log.Verbose("Accessing {C}.", $"{item}");
                    return item;
                }
            Log.Error("No argument of name: {name} found.", name);
            return default!;
        }

        internal List<Argument> ConstructArguments()
        {
            #region Extracting Arguments' attributes from json

            const string name = "Arguments";
            Log.Verbose("Extracting {entity}", name);
            var ListArguments = GetConfiguration()[name].ToObject<List<IArgument>>();

            #endregion Extracting Arguments' attributes from json

            #region Build loop for the Arguments

            var Arguments = new List<Argument>();
            foreach (IArgument arg in ListArguments)
            {
                Arguments.Add(arg.BuildArgument(GetCommands().Find(el => el.Name.Equals(arg.Command))!));
            }
            Log.Debug("Arguments built.");

            #endregion Build loop for the Arguments

            return Arguments;
        }

        #endregion Arguments

        /// <summary>
        /// Class Constructor.
        /// </summary>
        /// <param name="file">Path to configuration file for deserialization.</param>
        internal IJsonApp(string file)
        {
            configuration = JsonConvert.DeserializeObject<Dictionary<string, dynamic>>(File.ReadAllText(file))!;
            properties = ConstructProperties();
            packages = ConstructPackages();
            commands = ConstructCommands();
            options = ConstructOptions();
            arguments = ConstructArguments();
        }
    }
}