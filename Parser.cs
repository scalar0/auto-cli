// This file is supposed to be auto-generated

namespace autocli
{
    public static class Parser
    {
        public static async Task Main(string[] args)
        {
            var levelSwitch = new LoggingLevelSwitch(); // .ControlledBy(levelSwitch)
            Log.Logger = new LoggerConfiguration()
                .MinimumLevel.Debug()
                .WriteTo.Console(outputTemplate:
                                "[{Timestamp:HH:mm:ss:ff} {Level:u10}] {Message:1j}{NewLine}{Exception}")
                .WriteTo.File("./logs/autocli.log.txt", rollingInterval: RollingInterval.Minute)
                .CreateLogger();

            // ===========================================COMMANDS===========================================

            RootCommand command = Builders.MakeRootCommand(
                title: "AUTOCLI : automation for CLI applications interface creation",
                description: "[autocli] aims to automate .NET 6.0.* CLI applications development based on an input architecture stored in a .json file.\nThe configuration file stores the architecture for the project's commands, subcommands, options, arguments and properties.",
                setverbosity: true);

            Command creation = Commands._creation(command);
            Command generation = Commands._generation(command);

            // ===========================================OPTIONS===========================================

            Option<DirectoryInfo> dir_choice = Options._dir_choice(creation);
            Option<string> pushing = Options._pushing(generation);

            // ===========================================ARGUMENTS===========================================

            Argument file_name = Arguments._file_name(creation);
            Argument file_path = Arguments._file_path(generation);

            // ===========================================HANDLERS===========================================

            creation.Handler = CommandHandler.Create<string, DirectoryInfo>(Handlers.Create_Template);
            generation.Handler = CommandHandler.Create<string>(Handlers.Generate);

            // ===========================================INVOKE===========================================

            // Parse the incoming args and invoke the handler
            Log.CloseAndFlush();
            await command.InvokeAsync(args);
        }
    }
}