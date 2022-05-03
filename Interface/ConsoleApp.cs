using Newtonsoft.Json;

namespace autocli.Interface;

public class ConsoleApp
{
    #region Source Code

    internal StringBuilder sourceCode { get; set; } = new StringBuilder();

    #endregion Source Code

    #region Configuration

    internal readonly Dictionary<string, dynamic> configuration;

    #endregion Configuration

    #region Properties

    internal IProperty properties { get; set; }

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
        Parser.ExecuteCommandSync($"dotnet new console --name {project_name} --framework {properties.Framework} --output {properties.OutputPath}{project_name}");
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
            StringBuilder param = new();
            foreach (var child in cmd.Children)
            {
                // TODO: Implement different argument types Exclude child-commands from arguments
                if (child is not Command)
                {
                    param.Append($"string {child.Name.Replace("-", "_")},");
                }
            }
            File.AppendAllText(Handlers, $@"
    public static void {cmd.Name}({param.ToString().Remove(param.Length - 1, 1)})
    {{
    }}");
        }
        File.AppendAllText(Handlers, "\n}}");
    }

    #endregion Properties

    #region Packages

    internal List<IPackage> packages { get; set; }

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
            Parser.ExecuteCommandSync("dotnet add " + properties.OutputPath + properties.Name + " package " + pack.Name + " " + pack.Version);
            i += 1;
        }
        Console.WriteLine(">> PACKAGES INSTALLED <<");
        Parser.ExecuteCommandSync($"dotnet list {properties.OutputPath}{properties.Name} package");
    }

    #endregion Packages

    #region Commands

    internal List<Command> commands { get; } = new List<Command>();

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

    internal void SetCommands(bool gen)
    {
        commands.Add(properties.BuildRoot());
        if (gen)
        {
            sourceCode.AppendLine("//Commands");
            sourceCode.AppendLine();
            sourceCode.AppendLine(properties.TRootCommand());
            sourceCode.AppendLine();
        }
        var ListCommands = configuration["Commands"].ToObject<List<ICommand>>();
        foreach (ICommand cmd in ListCommands)
        {
            Command parent = (cmd.Parent == "root") ? commands[0] : GetCommand(cmd.Parent);
            commands.Add(cmd.BuildCommand(parent));
            if (gen) sourceCode.AppendLine(cmd.TCommand());
        }

        Log.Debug("Commands built.");
    }

    #endregion Commands

    #region Options

    internal List<Option> options { get; } = new List<Option>();

    internal Option GetOption(string name)
    {
        foreach (Option item in options)
        {
            if (item.Name == name)
            {
                Log.Verbose("Accessing {C}.", $"{item}");
                return item;
            }
        }
        Log.Error("No option of name: {alias} found.", name);
        return default!;
    }

    internal void SetOptions(bool gen)
    {
        if (gen)
        {
            sourceCode.AppendLine("//Options");
            sourceCode.AppendLine();
        }

        var ListOptions = configuration["Options"].ToObject<List<IOption>>();
        foreach (IOption option in ListOptions)
        {
            Command parent = (option.Command == "root") ? commands[0] : GetCommand(option.Command);
            options.Add(option.BuildOption(parent));
            if (gen) sourceCode.AppendLine(option.TOption());
        }

        Log.Debug("Options built.");
    }

    #endregion Options

    #region Arguments

    internal List<Argument> arguments { get; } = new List<Argument>();

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

    internal void SetArguments(bool gen)
    {
        if (gen)
        {
            sourceCode.AppendLine("//Arguments");
            sourceCode.AppendLine();
        }

        var ListArguments = configuration["Arguments"].ToObject<List<IArgument>>();
        foreach (IArgument arg in ListArguments)
        {
            Command parent = (arg.Command == "root") ? commands[0] : GetCommand(arg.Command);
            arguments.Add(arg.BuildArgument(parent));
            if (gen) sourceCode.AppendLine(arg.TArgument());
        }

        Log.Debug("Arguments built.");
    }

    #endregion Arguments

    #region Handlers

    internal void SourceHandlers()
    {
        // Root command handler
        sourceCode.AppendLine("//Handlers");
        sourceCode.AppendLine();
        sourceCode.AppendLine(@"root.SetHandler(() => root.InvokeAsync(""-h""));");
        sourceCode.AppendLine();

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
            sourceCode.AppendLine();
            sourceCode.AppendLine(template);
        }
        Log.Debug("Handlers implemented.");
    }

    internal string SourceMain()
    {
        SourceHandlers();
        StringBuilder main = new();

        // using statements for packages
        foreach (var pkg in packages)
        {
            main.AppendLine($"using {pkg.Name};");
        }

        // Sentry logging
        var list = new List<string>();
        if (properties.APIs is not null)
        {
            list = properties.APIs.Select(el => el["Name"]).ToList();
        }

        // namespace
        main.AppendLine($"namespace {properties.Name};");

        // Parser class and logger
        main.AppendLine(@$"
internal static class Parser
{{
    #region Static strings");
        if (list.Count > 0)
        {
            main.Append("\t" + $@"internal static string DsnToken = ""{properties.APIs.Where(el => el["Name"] == "Sentry").ToList()[0]["DsnToken"]}"";");
        }
        main.AppendLine();
        main.AppendLine("\t" +
        @"internal static string LogFormat = ""{{Timestamp:yyyy-MM-dd HH:mm:ss.fff zzz}} [{{Level:u4}}] {{Message:1j}}{{NewLine}}{{Exception}}"";
    #endregion

    /// <summary>
    /// Sets and returns a new configured instance of a logger
    /// </summary>
    internal static ILogger BuildLogger(string[] args)
        {{
            // Default values of args
            char vv = 'm';
            char vs = 'm';

            // Custom parser for logger verbosity configuration 
            foreach (var arg in args)
            {{
                if (arg.StartsWith(""-v"") || arg.StartsWith(""--verbose"")) vv = arg[^1];
                else if (arg.StartsWith(""-s"") || arg.StartsWith(""--sentry"")) vs = arg[^1];
            }}

            var levelSwitch = new LoggingLevelSwitch
            {{
                MinimumLevel = verbose switch
                {{
                    ('v') => LogEventLevel.Verbose,
                    ('d') => LogEventLevel.Debug,
                    _ => LogEventLevel.Information,
                }}
            }};
            return new LoggerConfiguration()
                .MinimumLevel.ControlledBy(levelSwitch)");
        if (list.Count > 0)
        {
            main.AppendLine("\t\t" +
                    @".WriteTo.Sentry(o =>
                {{o.MinimumBreadcrumbLevel = LogEventLevel.Debug;
                    o.MinimumEventLevel = LogEventLevel.Error;
                    o.StackTraceMode = StackTraceMode.Enhanced;
                    o.AttachStacktrace = true;
                    o.Dsn = DsnToken;
                    o.Debug = ((vs == 'd') || (vs == 'd')) ? true : false;
                }})");
        }

        main.AppendLine("\t\t");
        main.Append(
                @".WriteTo.Console(outputTemplate: LogFormat)
                .WriteTo.File(""./logs/{properties.Name}.log"", rollingInterval: RollingInterval.Minute, restrictedToMinimumLevel: LogEventLevel.Verbose)
                .CreateLogger();
        }}

    /// <summary> Async task to parse the array of args as strings </summary> <param
    /// name=""args"">Type : string[]</param>
    internal static async Task Main(string[] args)
    {{
        Log.Logger = BuildLogger(args);
        try
        {{");
        main.AppendLine(sourceCode.ToString());
        main.AppendLine(
            @"Log.Verbose(""Invoking args parser."");
            Log.CloseAndFlush();

            await root.InvokeAsync(args);
        }}
        catch (Exception ex)
        {{
            Log.Error(ex, ex.Message, ex.ToString());
            Log.CloseAndFlush();
        }}
    }}
}}");
        return main.ToString();
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
        Log.CloseAndFlush();
    }

    #endregion Handlers

    /// <summary>
    /// Class Constructor.
    /// </summary>
    /// <param name="file">Path to configuration file for deserialization.</param>
    internal ConsoleApp(string file, bool gen = false)
    {
        configuration = JsonConvert.DeserializeObject<Dictionary<string, dynamic>>(File.ReadAllText(file))!;
        Log.Debug("Architecture deserialized.");
        SetProperties();
        SetPackages();
        SetCommands(gen);
        SetOptions(gen);
        SetArguments(gen);
    }
}