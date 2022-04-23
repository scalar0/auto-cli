using Newtonsoft.Json;

namespace autocli.Interface;

public class IJsonApp
{
    #region Source Code

    internal string sourceCode;

    internal string GetSourceCode() => sourceCode;

    internal void SetSourceCode(string value) => sourceCode = value;

    #endregion Source Code

    #region Configuration

    internal readonly Dictionary<string, dynamic> configuration;

    internal Dictionary<string, dynamic> GetConfiguration() => configuration;

    #endregion Configuration

    #region Properties

    internal readonly IProperty properties;

    internal IProperty GetProperties() => properties;

    internal IProperty ConstructProperties()
    {
        Log.Verbose("Extracting {entity}", "Properties");
        var s = GetConfiguration()["Properties"].ToObject<List<IProperty>>()[0];

        Log.Debug("Properties built.");
        return s;
    }

    internal void InstallProject()
    {
        // Retrieve project name
        string project_name = GetProperties().Name!;
        Log.Information("Creating new console application. Target framework : net6.0.");
        Console.WriteLine($"Project name : {project_name}");
        Functionnals.Utils.ExecuteCommandSync("dotnet [parse] new console --name " + project_name + ".CLI --framework net6.0 --output " + GetProperties().OutputPath + @"\" + project_name + ".CLI");
        // After the project is created, we need to copy the source code to the project main folder Program.cs after overwriting it.
    }

    #endregion Properties

    #region Packages

    internal readonly List<IPackage> packages;

    internal List<IPackage> GetPackages() => packages;

    internal List<IPackage> ConstructPackages()
    {
        Log.Verbose("Extracting {entity}", "Packages");
        var s = GetConfiguration()["Packages"].ToObject<List<IPackage>>();

        Log.Debug("Packages built.");
        return s;
    }

    internal void InstallPackages()
    {
        foreach (Interface.IPackage pack in GetPackages())
        {
            Log.Information("Installing package: " + pack.Name);
            Console.WriteLine("Installing package: " + pack.Name);
            Functionnals.Utils.ExecuteCommandSync("dotnet [parse] add package " + pack.Name + pack.Version);
        }
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

        Log.Verbose("Extracting {entity}", "Commands");
        var ListCommands = GetConfiguration()["Commands"].ToObject<List<ICommand>>();

        #endregion Extracting Commands' attributes from json

        #region Build loop for the Commands

        var Commands = new List<Command>()
        {
            GetProperties().BuildRoot()
        };
        SetSourceCode(GetProperties().TRootCommand());
        foreach (ICommand cmd in ListCommands)
        {
            Command com = (cmd.Parent == "root") ? Commands[0] : Commands.Find(el => el.Name.Equals(cmd.Parent))!;
            Commands.Add(cmd.BuildCommand(com));
            SetSourceCode(GetSourceCode() + cmd.TCommand());
        }

        #endregion Build loop for the Commands

        Log.Debug("Commands built.");
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

        Log.Verbose("Extracting {entity}", "Options");
        var ListOptions = GetConfiguration()["Options"].ToObject<List<IOption>>();

        #endregion Extracting the Options' attributes from json

        #region Build loop for the Options

        var Options = new List<Option>();
        foreach (IOption option in ListOptions)
        {
            Options.Add(option.BuildOption(GetCommands().Find(el => el.Name.Equals(option.Command))!));
            SetSourceCode(GetSourceCode() + option.TOption());
        }

        /// <summary>
        /// Add verbosity global option
        /// </summary>
        Option<string> verbose = new Option<string>(
            new[] { "--verbose", "-v" }, "Verbosity level of the output : m[inimal]; d[ebug]; v[erbose]. Always parse this option as last on the CLI call.")
            .FromAmong(new string[] { "m", "d", "v" });
        verbose.SetDefaultValue("m");
        GetCommands()[0].AddGlobalOption(verbose);

        #endregion Build loop for the Options

        Log.Debug("Options built.");
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

        Log.Verbose("Extracting {entity}", "Arguments");
        var ListArguments = GetConfiguration()["Arguments"].ToObject<List<IArgument>>();

        #endregion Extracting Arguments' attributes from json

        #region Build loop for the Arguments

        var Arguments = new List<Argument>();
        foreach (IArgument arg in ListArguments)
        {
            Arguments.Add(arg.BuildArgument(GetCommands().Find(el => el.Name.Equals(arg.Command))!));
            SetSourceCode(GetSourceCode() + arg.TArgument());
        }

        #endregion Build loop for the Arguments

        Log.Debug("Arguments built.");
        return Arguments;
    }

    #endregion Arguments

    // <auto-generated>
    // Maybe try generating a property for the IJsonApp class that will command it to generate the CallHandlers method, or either write it as a loop on the commands.
    internal void CallHandlers()
    {
        GetCommand("create")!.SetHandler<string, string>(
            Functionnals.Handlers.create,
            GetArgument("name")!,
            GetOption("directory")!);

        GetCommand("generate")!.SetHandler<string>(
            Functionnals.Handlers.generate,
            GetArgument("file")!);

        Log.Debug("Handlers implemented.");
    }

    /// <summary>
    /// Class Constructor.
    /// </summary>
    /// <param name="file">Path to configuration file for deserialization.</param>
    internal IJsonApp(string file)
    {
        try
        {
            configuration = JsonConvert.DeserializeObject<Dictionary<string, dynamic>>(File.ReadAllText(file))!;
            Log.Debug("Architecture deserialized.");
        }
        catch (Exception ex) { Log.Error(ex, ex.Message, ex.ToString()); }
        properties = ConstructProperties();
        packages = ConstructPackages();
        commands = ConstructCommands();
        options = ConstructOptions();
        arguments = ConstructArguments();
    }
}