using Newtonsoft.Json;

namespace autocli.Interface;

public class ConsoleApp
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
        var P = GetProperties();
        string project_name = P.Name!;
        Console.WriteLine($">> CREATING CONSOLE APPLICATION: {project_name} ({P.Framework}) <<");
        Functionnals.Utils.ExecuteCommandSync($"dotnet [parse] new console --name {project_name} --framework {P.Framework} --output {P.OutputPath}{project_name}");
        // After the project is created, copy the source code to the project main folder Program.cs after overwriting it, then create an Handlers class and define corresponding methods for each command.
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
        int l = GetPackages().Count;
        int i = 1;
        var P = GetProperties();
        foreach (Interface.IPackage pack in GetPackages())
        {
            Log.Information("Installing package {i}/{l}: {package}", pack.Name, i, l);
            Console.WriteLine($">> INSTALLING PACKAGE {i}/{l}: {pack.Name} <<");
            Functionnals.Utils.ExecuteCommandSync("dotnet [parse] add " + P.OutputPath + P.Name + " package " + pack.Name + " " + pack.Version);
            i += 1;
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

        #endregion Extracting Commands' attributes from json

        #region Build loop for the Commands

        List<Command> Commands = new()
        {
            GetProperties().BuildRoot()
        };

        SetSourceCode("\n//Commands\n" + GetProperties().TRootCommand());
        var ListCommands = GetConfiguration()["Commands"].ToObject<List<ICommand>>();
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

        /// <summary>
        /// Add verbosity global option
        /// </summary>
        Option<string> verbose = new Option<string>(
            new[] { "--verbose", "-v" }, "Verbosity level of the output : d[ebug]; m[inimal]; v[erbose]. Always parse this option as last on the CLI call.")
            .FromAmong(new string[] { "m", "d", "v" });
        verbose.SetDefaultValue("m");
        GetCommands()[0].AddGlobalOption(verbose);

        // Verbose option template
        string Tverbose = "\n//Options\n\n" + @"Option<string> verbose = new Option<string>(new[] { ""--verbose"", ""-v"" });" + "\n";
        Tverbose += @"verbose.Description = ""Verbosity level of the output : d[ebug]; m[inimal]; v[erbose]. Always parse this option as last on the CLI call."";" + "\n";
        Tverbose += @"verbose.SetDefaultValue(""m"");" + "\n";
        Tverbose += @"verbose.FromAmong(new string[] { ""m"", ""d"", ""v"" });" + "\n";
        Tverbose += @"root.AddGlobalOption(verbose);" + "\n";

        SetSourceCode(GetSourceCode() + Tverbose);

        var Options = new List<Option>();
        foreach (IOption option in ListOptions)
        {
            Options.Add(option.BuildOption(GetCommands().Find(el => el.Name.Equals(option.Command))!));
            SetSourceCode(GetSourceCode() + option.TOption());
        }

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

        SetSourceCode(GetSourceCode() + "\n//Arguments\n");
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

    #region Handlers
    internal void Constructhandlers()
    {
        // Root command handler
        List<string> handlers = new()
        {
            @"root.SetHandler(() => root.InvokeAsync(""-h""));" + "\n"
        };
        foreach (Command cmd in GetCommands())
        {
            string name = cmd.Name;
            string args = "";
            string param = "";
            foreach (var child in cmd.Children)
            {
                // Exclude child-commands from arguments
                if (child is not Command)
                {
                    args += "string,";
                    param += $"{child.Name.Replace("-", "_")},";
                }
            }
            // Remove commas
            args = args.Remove(args.Length - 1, 1);
            param = param.Remove(param.Length - 1, 1);

            handlers.Add($@"{name}.SetHandler<{args}>(
                    Handlers.{name},
                    {param});" + "\n");
        }
        // Removing second root command handler
        handlers.Remove(handlers[1]);
        // Adding handlers templates to the source code
        SetSourceCode(GetSourceCode() + "\n//Handlers\n");
        foreach (string template in handlers)
            SetSourceCode(GetSourceCode() + "\n" + template);
        Log.Debug("Handlers implemented.");
    }

    // <auto-generated>
    // Global handler for autocli.
    // Maybe try generating a property for the IJsonApp class that will command it to generate the CallHandlers method, or either write it as a loop on the commands.
    internal void SetHandlers()
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

    #endregion

    /// <summary>
    /// Class Constructor.
    /// </summary>
    /// <param name="file">Path to configuration file for deserialization.</param>
    internal ConsoleApp(string file)
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
        Constructhandlers();
        SetHandlers();
    }
}