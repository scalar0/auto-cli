/// <!Do Not Modify>
using Serilog.Core;
using Serilog.Events;

namespace autocli.Interface
{
    /// <summary>
    /// The IConstructor interface creates each entity needed for the application interface on the
    /// CLI. For each method the arguments must be parsed from .json configuration file.
    /// </summary>
    public interface IConstructor
    {
        /// <summary>
        /// Constructs a new instance of the RootCommand class.
        /// </summary>
        /// <param name="AppProperties">The application properties.</param>
        /// <returns>Corresponding RootCommand.</returns>
        public static RootCommand BuildRoot(IProperty AppProperties)
        {
            RootCommand rcom = new(Functionnals.Utils.Boxed(AppProperties.Title) + AppProperties.Description + "\n");
            rcom.Name = AppProperties.Name;
            Log.Debug("RootCommand built.");
            return rcom;
        }

        /// <summary>
        /// Retrieves the command or argument from the input list.
        /// </summary>
        /// <param name="L">Input list.</param>
        /// <param name="alias">Alias of the searched entity.</param>
        public static T? Get<T>(List<T> L, string alias)
        {
            try
            {
                foreach (dynamic? item in L)
                    if (item!.Name == alias)
                    {
                        Log.Verbose("Accessing {C}.", $"{item}");
                        return item;
                    }
            }
            catch (Exception ex) { Log.Error(ex, ex.Message, ex.ToString); }
            return default;
        }

        /// <summary>
        /// Constructs a new instance of the SubCommand class.
        /// </summary>
        /// <param name="parent">Parent Command.</param>
        /// <param name="Interface">The SubCommand binder.</param>
        /// <returns>Corresponding SubCommand.</returns>
        public static Command BuildCommand(
            Command parent, ICommand Interface)
        {
            Command cmd = new(Interface.Alias);
            try
            {
                cmd.Description = Interface.Description;
                parent.AddCommand(cmd);
                Log.Verbose("{C} built and added to {U}.", $"{cmd}", $"{parent}");
            }
            catch (Exception ex)
            {
                Log.Error(ex, ex.Message, ex.ToString);
            }
            return cmd;
        }

        /// <summary>
        /// Constructs a new instance of the Argument class.
        /// </summary>
        /// <typeparam name="T">Type fo the argument.</typeparam>
        /// <param name="command">Parent command for the argument.</param>
        /// <param name="Interface">The argument binder.</param>
        /// <returns>Corresponding Argument.</returns>
        public static Argument<T> BuildArgument<T>(
            Command command, IArgument Interface)
        {
            Argument<T> argument = new(Interface.Alias);
            try
            {
                argument.Description = Interface.Description;
                if (Interface.DefaultValue is not null) argument.SetDefaultValue(Interface.DefaultValue);
                command.AddArgument(argument);
                Log.Verbose("{A} built and added to {U}.", $"{argument}", $"{command}");
            }
            catch (Exception ex)
            {
                Log.Error(ex, ex.Message, ex.ToString);
            }
            return argument;
        }

        /// <summary>
        /// Constructs a new instance of the Option class.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="command">Parent command.</param>
        /// <param name="Interface">The option binder.</param>
        /// <returns>Corresponding Option.</returns>
        public static Option<T> BuildOption<T>(
            Command command, IOption Interface)
        {
            Option<T> option = new(Interface.Aliases);
            if (Interface.DefaultValue is not null) option.SetDefaultValue(Interface.DefaultValue);
            try
            {
                option.Name = Interface.Name;
                option.IsRequired = Interface.Required;
                option.Description = Interface.Description;
                command.AddOption(option);
                Log.Verbose("{O} built and added to {U}.", $"{option}", $"{command}");
            }
            catch (Exception ex)
            {
                Log.Error(ex, ex.Message, ex.ToString);
            }
            return option;
        }

        /// <summary>
        /// Sets and returns a new configured instance of a logger
        /// </summary>
        /// <param name="verbose">Output verbosity of the application</param>
        public static ILogger BuildLogger(string verbose = null!)
        {
            var levelSwitch = new LoggingLevelSwitch
            {
                MinimumLevel = verbose switch
                {
                    ("v") => Serilog.Events.LogEventLevel.Verbose,
                    ("d") => Serilog.Events.LogEventLevel.Debug,
                    _ => Serilog.Events.LogEventLevel.Information,
                }
            };
            return new LoggerConfiguration()
                .MinimumLevel.ControlledBy(levelSwitch)
                .WriteTo.Console(outputTemplate:
                        "[{Timestamp:HH:mm:ss:ff} {Level:u4}] {Message:1j}{NewLine}{Exception}")
                .WriteTo.File("./logs/autocli.log.txt", rollingInterval: RollingInterval.Minute, restrictedToMinimumLevel: LogEventLevel.Verbose)
                .CreateLogger();
        }
    }
}