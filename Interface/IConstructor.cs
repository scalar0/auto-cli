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
        /// Retrieves the command or argument from the input list.
        /// </summary>
        /// <param name="L">Input list.</param>
        /// <param name="alias">Alias of the searched entity.</param>
        public static T? Get<T>(List<T> L, string alias)
        {
            foreach (dynamic? item in L)
                if (item!.Name == alias)
                {
                    Log.Verbose("Accessing {C}.", $"{item}");
                    return item;
                }
            return default;
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
                    ("v") => LogEventLevel.Verbose,
                    ("d") => LogEventLevel.Debug,
                    _ => LogEventLevel.Information,
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