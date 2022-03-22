global using Newtonsoft.Json;
using autocli.Interface;

namespace autocli.Functionnals
{
    /// <summary>
    /// Class API to parse the architecture from the configuration file.
    /// </summary>
    internal static class ParseArchitecture
    {
        /// <summary> Serialize the Json configuration file and parses it to a dictionnary.
        /// </summary> <param name="path">Path of configuration file.</param>
        /// <returns>Dictionary<property, string></returns>
        public static Dictionary<string, dynamic> JsonParser(string path)
        {
            return JsonConvert.DeserializeObject<Dictionary<string, dynamic>>(File.ReadAllText(path))!;
        }

        /// <summary> Method to retrieve properties of the application. </summary> <param
        /// name="json">Dictionnary extracted from the json file.</param>
        /// <returns>Dictionary<property, string></returns>
        public static Properties GetProperties(dynamic json)
        {
            const string name = "Properties";
            Log.Debug("Extracting {entity}...", name);
            return json[$"{name}"].ToObject<List<Properties>>()[0];
        }

        /// <summary> Method to deserialize Packages from .json file. </summary> <param
        /// name="json">Dictionnary extracted from the json file.</param>
        /// <returns>List<Dictionary<property, string>></returns>
        public static List<Package>? GetPackages(dynamic json)
        {
            const string name = "Packages";
            Log.Debug("Extracting {entity}...", name);
            return json[name].ToObject<List<Package>>();
        }

        /// <summary> Method to deserialize SubCommands from .json file. </summary> <param
        /// name="json">Dictionnary extracted from the json file.</param>
        /// <returns>List<Dictionary<property, string>></returns>
        public static List<SubCommand> GetCommands(dynamic json)
        {
            //TODO:     Implement deserialization of SubCommands
            const string name = "SubCommands";
            Log.Debug("Extracting {entity}...", name);
            return json[name].ToObject<List<SubCommand>>();
        }

        /// <summary> Method to deserialize Arguments from .json file. </summary> <param
        /// name="json">Dictionnary extracted from the json file.</param>
        /// <returns>List<Dictionary<property, string>></returns>
        public static List<Arguments> GetArguments(dynamic json)
        {
            //TODO:     Implement deserialization of Arguments
            const string name = "Arguments";
            Log.Debug("Extracting {entity}...", name);
            return json[name].ToObject<List<Arguments>>();
        }

        /// <summary> Method to deserialize Options from .json file. </summary> <param
        /// name="json">Dictionnary extracted from the json file.</param>
        /// <returns>List<Dictionary<property, string[] or string>></returns>
        public static List<Options> GetOptions(dynamic json)
        {
            //TODO:     Implement deserialization of Options
            const string name = "Options";
            Log.Debug("Extracting {entity}...", name);
            return json[name].ToObject<List<Options>>();
        }
    }
}