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

    internal IProperty properties;

    internal IProperty GetProperties() => properties;

    internal void SetProperties()
    {
        Log.Verbose("Extracting {entity}", "Properties");
        properties = configuration["Properties"].ToObject<List<IProperty>>()[0];
        Log.Debug("Properties built.");
    }

    public void InstallProject()
    {
        // Retrieve project name
        string project_name = properties.Name!;
        Console.WriteLine($">> CREATING CONSOLE APPLICATION: {project_name} ({properties.Framework}) <<");
        Functionnals.Utils.ExecuteCommandSync($"dotnet new console --name {project_name} --framework {properties.Framework} --output {properties.OutputPath}{project_name}");
        // Copy the source code to the project main method
        string main = $"{properties.OutputPath}{project_name}";
        File.WriteAllText(main + "\\Program.cs", SourceMain());
        File.Move(main + "\\Program.cs", main + "\\Parser.cs");

        // Create an Handlers class and define corresponding methods for each command
        string Handlers = $"{properties.OutputPath}{project_name}\\Handlers.cs";
        File.WriteAllText(Handlers, $"namespace {project_name}; \n\tpublic static class Handlers {{\n\t");

        // Loop over the commands except for first element of GetCommands()

        foreach (Command cmd in commands.Skip(1))
        {
            string param = "";
            foreach (var child in cmd.Children)
            {
                // TODO: Implement different argument types Exclude child-commands from arguments
                if (child is not Command)
                {
                    param += $"string {child.Name.Replace("-", "_")},";
                }
            }
            // Remove commas
            param = param.Remove(param.Length - 1, 1);

            File.AppendAllText(Handlers, $@"
    public static void {cmd.Name}({param})
    {{
    }}");
        }
        File.AppendAllText(Handlers, $"\n}}");
    }

    #endregion Properties

    #region Packages

    internal List<IPackage> packages;

    internal List<IPackage> GetPackages() => packages;

    internal void SetPackages()
    {
        packages = configuration["Packages"].ToObject<List<IPackage>>();
        Log.Debug("Packages built.");
    }

    internal void InstallPackages()
    {
        int l = packages.Count;
        int i = 1;

        foreach (Interface.IPackage pack in packages)
        {
            Log.Information("Installing package {i}/{l}: {package}", pack.Name, i, l);
            Console.WriteLine($">> INSTALLING PACKAGE {i}/{l}: {pack.Name} <<");
            Functionnals.Utils.ExecuteCommandSync("dotnet add " + properties.OutputPath + properties.Name + " package " + pack.Name + " " + pack.Version);
            i += 1;
        }
        Console.WriteLine($">> PACKAGES INSTALLED <<");
        Functionnals.Utils.ExecuteCommandSync($"dotnet list {properties.OutputPath}{properties.Name} package");
    }

    #endregion Packages

    #region Commands

    internal List<Command> commands;

    internal List<Command> GetCommands() => commands;

    internal RootCommand GetRootCommand() => (RootCommand)commands[0];

    internal Command GetCommand(string name)
    {
        foreach (Command item in commands)
            if (item!.Name == name)
            {
                Log.Verbose("Accessing {C}.", $"{item}");
                return item;
            }
        Log.Error("No command of name: {name} found.", name);
        return default!;
    }

    internal void SetCommands()
    {
        commands = new List<Command>() { properties.BuildRoot() };

        SetSourceCode("\n//Commands\n" + properties.TRootCommand());
        var ListCommands = configuration["Commands"].ToObject<List<ICommand>>();
        foreach (ICommand cmd in ListCommands)
        {
            Command parent = (cmd.Parent == "root") ? commands[0] : commands.Find(el => el.Name.Equals(cmd.Parent))!;
            commands.Add(cmd.BuildCommand(parent));
            SetSourceCode(sourceCode + cmd.TCommand());
        }

        Log.Debug("Commands built.");
    }

    #endregion Commands

    #region Options

    internal List<Option> options;

    internal List<Option> GetOptions() => options;

    internal Option GetOption(string name)
    {
        foreach (Option item in options)
            if (item!.Name == name)
            {
                Log.Verbose("Accessing {C}.", $"{item}");
                return item;
            }
        Log.Error("No option of name: {name} found.", name);
        return default!;
    }

    internal void SetOptions()
    {
        /// <summary>
        /// Add verbosity global option
        /// </summary>
        Option<string> verbose = new Option<string>(
            new[] { "--verbose", "-v" }, "Verbosity level of the output : d[ebug]; m[inimal]; v[erbose]. Always parse this option as last on the CLI call.")
            .FromAmong(new string[] { "m", "d", "v" });
        verbose.SetDefaultValue("m");
        commands[0].AddGlobalOption(verbose);

        #region Verbose option template

        string Tverbose = "\n//Options\n\n" + @"Option<string> verbose = new Option<string>(new[] { ""--verbose"", ""-v"" });" + "\n";
        Tverbose += @"verbose.Description = ""Verbosity level of the output : d[ebug]; m[inimal]; v[erbose]. Always parse this option as last on the CLI call."";" + "\n";
        Tverbose += @"verbose.SetDefaultValue(""m"");" + "\n";
        Tverbose += @"verbose.FromAmong(new string[] { ""m"", ""d"", ""v"" });" + "\n";
        Tverbose += @"root.AddGlobalOption(verbose);" + "\n";

        SetSourceCode(sourceCode + Tverbose);

        #endregion Verbose option template

        options = new List<Option>();
        var ListOptions = configuration["Options"].ToObject<List<IOption>>();
        foreach (IOption option in ListOptions)
        {
            options.Add(option.BuildOption(commands.Find(el => el.Name.Equals(option.Command))!));
            SetSourceCode(sourceCode + option.TOption());
        }

        Log.Debug("Options built.");
    }

    #endregion Options

    #region Arguments

    internal List<Argument> arguments;

    internal List<Argument> GetArguments() => arguments;

    internal Argument GetArgument(string name)
    {
        foreach (Argument item in arguments)
            if (item!.Name == name)
            {
                Log.Verbose("Accessing {C}.", $"{item}");
                return item;
            }
        Log.Error("No argument of name: {name} found.", name);
        return default!;
    }

    internal void SetArguments()
    {
        SetSourceCode(sourceCode + "\n//Arguments\n");
        arguments = new List<Argument>();

        var ListArguments = configuration["Arguments"].ToObject<List<IArgument>>();
        foreach (IArgument arg in ListArguments)
        {
            arguments.Add(arg.BuildArgument(commands.Find(el => el.Name.Equals(arg.Command))!));
            SetSourceCode(sourceCode + arg.TArgument());
        }

        Log.Debug("Arguments built.");
    }

    #endregion Arguments

    #region Handlers

    internal void SourceHandlers()
    {
        // Root command handler
        SetSourceCode(sourceCode + "\n//Handlers\n\n" + @"root.SetHandler(() => root.InvokeAsync(""-h""));" + "\n");

        foreach (Command cmd in commands.Skip(1))
        {
            string name = cmd.Name;
            string args = string.Empty;
            string param = string.Empty;
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

            string template = $@"{name}.SetHandler<{args}>(
                    Handlers.{name},
                    {param});" + "\n";
            SetSourceCode(sourceCode + "\n" + template);
        }
        Log.Debug("Handlers implemented.");
    }

    internal string SourceMain()
    {
        SourceHandlers();
        string main = string.Empty;

        // using statements for packages
        foreach (var pkg in packages)
        {
            main += $"using {pkg.Name};\n";
        }

        // Sentry logging
        var list = packages.Select(el => el.Name).ToList();
        string open_sentry = (list.Contains("Sentry")) ? @"// Sentry
        using (SentrySdk.Init(Sentry =>
        {
            Sentry.Dsn = ""https://5befa8f2131e4d55b57193308225770e@o1213812.ingest.sentry.io/6353266"";
                                 // Set Sentry logger verbosity
            Sentry.Debug = false;
            // Percentage of captured transactions for performance monitoring.
            Sentry.TracesSampleRate = 1.0;
        }))
            {
                " : "";

        string close_sentry = (list.Contains("Sentry")) ? @"
            }
        }" : "";

        // namespace
        main += $"\nnamespace {GetProperties().Name};\n";

        // Parser class and logger
        main += @$"
internal static class Parser
{{
    /// <summary> Sets and returns a new configured instance of a logger </summary> <param
    /// name=""verbose"">Output verbosity of the application</param>
    internal static ILogger BuildLogger(string verbose = null!)
        {{
            var levelSwitch = new LoggingLevelSwitch
            {{
                MinimumLevel = verbose switch
                {{
                    (""v"") => LogEventLevel.Verbose,
                    (""d"") => LogEventLevel.Debug,
                    _ => LogEventLevel.Information,
                }}
            }};
            return new LoggerConfiguration()
                .MinimumLevel.ControlledBy(levelSwitch)
                .WriteTo.Console(outputTemplate:
                        ""[{{Timestamp:HH:mm:ss:ff}} {{Level:u4}}] {{Message:1j}}{{NewLine}}{{Exception}}"")
                .WriteTo.File(""./logs/{GetProperties().Name}.log.txt"", rollingInterval: RollingInterval.Minute, restrictedToMinimumLevel: LogEventLevel.Verbose)
                .CreateLogger();
        }}

    /// <summary> Async task to parse the array of args as strings </summary> <param
    /// name=""args"">Type : string[]</param>
    internal static async Task Main(string[] args)
    {{
        Log.Logger = (args.Length is not 0) ? BuildLogger(args[^1]) : BuildLogger();
        {open_sentry}
            {GetSourceCode()}

            Log.Verbose(""Invoking args parser."");
            Log.CloseAndFlush();

            await root.InvokeAsync(args);
        {close_sentry}
    }}
}}";
        return main;
    }

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

    #endregion Handlers

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
        catch (Exception ex)
        {
            Log.Error(ex, ex.Message, ex.ToString());
        }
        SetProperties();
        SetPackages();
        SetCommands();
        SetOptions();
        SetArguments();
    }
}