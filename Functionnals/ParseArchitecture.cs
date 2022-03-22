global using Newtonsoft.Json;
using autocli.Interface;

namespace autocli.Functionnals
{
    /// <summary>
    /// Class API to parse the architecture from the configuration file.
    /// </summary>
    internal static class ParseArchitecture
    {
        /// <summary>
        /// Serializes the Json configuration file and parses it to a dictionnary.
        /// </summary>
        /// <param name="path">Path of configuration file.</param>
        /// <returns>Dictionary of keys : property and values : dynamic object.</returns>
        public static Dictionary<string, dynamic> JsonParser(string path)
        {
            return JsonConvert.DeserializeObject<Dictionary<string, dynamic>>(File.ReadAllText(path))!;
        }

        /// <summary>
        /// Method to retrieve properties of the application.
        /// </summary>
        /// <param name="json">Dictionnary parsed from the json file.</param>
        /// <returns>Properties of the application.</returns>
        public static Properties GetProperties(dynamic json)
        {
            const string name = "Properties";
            Log.Debug("Extracting {entity}...", name);
            return json[$"{name}"].ToObject<List<Properties>>()[0];
        }

        /// <summary>
        /// Method to deserialize Packages from .json file.
        /// </summary>
        /// <param name="json">Dictionnary parsed from the json file.</param>
        /// <returns>List of packages to be installed within the project.</returns>
        public static List<Packages>? GetPackages(dynamic json)
        {
            const string name = "Packages";
            Log.Debug("Extracting {entity}...", name);
            return json[name].ToObject<List<Packages>>();
        }

        /// <summary>
        /// Method to deserialize SubCommands from .json file.
        /// </summary>
        /// <param name="json">Dictionnary parsed from the json file.</param>
        /// <returns>List of SubCommand that will constitute the interface of the application.</returns>
        public static List<Commands> GetCommands(dynamic json)
        {
            //TODO:     Implement deserialization of SubCommands
            const string name = "Commands";
            Log.Debug("Extracting {entity}...", name);
            return json[name].ToObject<List<Commands>>();
        }

        /// <summary>
        /// Method to deserialize Arguments from .json file.
        /// </summary>
        /// <param name="json">Dictionnary parsed from the json file.</param>
        /// <returns>List of arguments that will constitute the interface of the application.</returns>
        public static List<Arguments> GetArguments(dynamic json)
        {
            //TODO:     Implement deserialization of Arguments
            const string name = "Arguments";
            Log.Debug("Extracting {entity}...", name);
            return json[name].ToObject<List<Arguments>>();
        }

        /// <summary>
        /// Method to deserialize Options from .json file.
        /// </summary>
        /// <param name="json">Dictionnary parsed from the json file.</param>
        /// <returns>List of options that will constitute the interface of the application.</returns>
        public static List<Options> GetOptions(dynamic json)
        {
            //TODO:     Implement deserialization of Options
            const string name = "Options";
            Log.Debug("Extracting {entity}...", name);
            return json[name].ToObject<List<Options>>();
        }
    }
}