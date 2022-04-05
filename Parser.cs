global using Newtonsoft.Json;
global using Serilog;
global using System.CommandLine;

namespace autocli
{
    public static class Parser
    {
        private const string config = @"C:\Users\matte\source\repos\autoCLI\Properties\Architecture.json";

        /// <summary>
        /// Async task to parse the array of args as strings
        /// </summary>
        /// <param name="args">Type : string[]</param>
        public static async Task Main(string[] args)
        {
            // Logger
            Log.Logger = (args.Length is not 0) ? Interface.Constructors.BuildLogger(args[^1]) : Interface.Constructors.BuildLogger();

            /// <summary>
            /// Deserializes the Json configuration file and parses it to a dictionnary.
            /// </summary>
            Dictionary<string, dynamic> dict = JsonConvert.DeserializeObject<Dictionary<string, dynamic>>(File.ReadAllText(config))!;

            //Properties
            Interface.Properties AppProperties = Interface.Getter.GetProperties(dict);

            // Packages
            List<Interface.Packages> Packages = Interface.Getter.GetPackages(dict);

            // Commands and RootCommand
            List<Command> Commands = Interface.Getter.GetListCommands(dict, AppProperties);
            RootCommand root = (RootCommand)Commands[0];

            // Options
            List<Option> Options = Interface.Getter.GetListOptions(Commands, dict);

            // Arguments
            List<Argument> Arguments = Interface.Getter.GetListArguments(Commands, dict);

            // Handlers
            root.SetHandler(() => root.InvokeAsync("-h"));
            Functionnals.Handlers.CallHandlers(Commands, Arguments, Options, AppProperties, Packages);

            Log.Information("Automated CLI creation tool built.");
            Log.Verbose("Invoking args parser.");
            //Log.CloseAndFlush();

            await root.InvokeAsync(args);
        }
    }
}