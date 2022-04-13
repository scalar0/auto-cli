namespace autocli.Interface;

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
        var watch = System.Diagnostics.Stopwatch.StartNew();
        Log.Verbose("Extracting {entity}", "Properties");
        var s = GetConfiguration()["Properties"].ToObject<List<IProperty>>()[0];
        watch.Stop();
        Log.Debug("Properties built: {t} ms", watch.ElapsedMilliseconds);
        return s;
    }

    #endregion Properties

    #region Packages

    internal readonly List<IPackage> packages;

    internal List<IPackage> GetPackages() => packages;

    internal List<IPackage> ConstructPackages()
    {
        var watch = System.Diagnostics.Stopwatch.StartNew();
        Log.Verbose("Extracting {entity}", "Packages");
        var s = GetConfiguration()["Packages"].ToObject<List<IPackage>>();
        watch.Stop();
        Log.Debug("Packages built: {t} ms", watch.ElapsedMilliseconds);
        return s;
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
        var watch = System.Diagnostics.Stopwatch.StartNew();

        #region Extracting Commands' attributes from json

        Log.Verbose("Extracting {entity}", "Commands");
        var ListCommands = GetConfiguration()["Commands"].ToObject<List<ICommand>>();

        #endregion Extracting Commands' attributes from json

        #region Build loop for the Commands

        var Commands = new List<Command>()
        {
            GetProperties().BuildRoot()
        };
        foreach (ICommand cmd in ListCommands)
        {
            Command com = (cmd.Parent == "root") ? Commands[0] : Commands.Find(el => el.Name.Equals(cmd.Parent))!;
            Commands.Add(cmd.BuildCommand(com));
        }

        #endregion Build loop for the Commands

        watch.Stop();
        Log.Debug("Commands built: {t} ms", watch.ElapsedMilliseconds);
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
        var watch = System.Diagnostics.Stopwatch.StartNew();

        #region Extracting the Options' attributes from json

        Log.Verbose("Extracting {entity}", "Options");
        var ListOptions = GetConfiguration()["Options"].ToObject<List<IOption>>();

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

        #endregion Build loop for the Options

        watch.Stop();
        Log.Debug("Options built: {t} ms", watch.ElapsedMilliseconds);
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
        var watch = System.Diagnostics.Stopwatch.StartNew();

        #region Extracting Arguments' attributes from json

        Log.Verbose("Extracting {entity}", "Arguments");
        var ListArguments = GetConfiguration()["Arguments"].ToObject<List<IArgument>>();

        #endregion Extracting Arguments' attributes from json

        #region Build loop for the Arguments

        var Arguments = new List<Argument>();
        foreach (IArgument arg in ListArguments)
        {
            Arguments.Add(arg.BuildArgument(GetCommands().Find(el => el.Name.Equals(arg.Command))!));
        }

        #endregion Build loop for the Arguments

        watch.Stop();
        Log.Debug("Arguments built: {t} ms", watch.ElapsedMilliseconds);
        return Arguments;
    }

    #endregion Arguments

    /// <summary>
    /// Class Constructor.
    /// </summary>
    /// <param name="file">Path to configuration file for deserialization.</param>
    internal IJsonApp(string file)
    {
        var watch = System.Diagnostics.Stopwatch.StartNew();
        configuration = JsonConvert.DeserializeObject<Dictionary<string, dynamic>>(File.ReadAllText(file))!;
        watch.Stop();
        Log.Debug("Architecture deserialized: {t} ms", watch.ElapsedMilliseconds);
        properties = ConstructProperties();
        packages = ConstructPackages();
        commands = ConstructCommands();
        options = ConstructOptions();
        arguments = ConstructArguments();
    }
}