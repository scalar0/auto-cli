global using Newtonsoft.Json;
using System.Reflection;

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
        public static Dictionary<string, Newtonsoft.Json.Linq.JArray> JsonParser(string path)
        {
            return JsonConvert.DeserializeObject<Dictionary<string, Newtonsoft.Json.Linq.JArray>>(File.ReadAllText(path))!;
        }

        /// <summary> Method to retrieve properties of the application. </summary> <param
        /// name="json">Dictionnary extracted from the json file.</param>
        /// <returns>Dictionary<property, string></returns>
        public static Dictionary<string, string> GetProperties(Dictionary<string, Newtonsoft.Json.Linq.JArray> json)
        {
            string name = MethodBase.GetCurrentMethod()!.Name.Remove(0, 3);
            Log.Debug("Extracting {entity}...", name);
            return json[$"{name}"].ToObject<List<Dictionary<string, string>>>()![0];
        }

        /// <summary> Method to deserialize Packages from .json file. </summary> <param
        /// name="json">Dictionnary extracted from the json file.</param>
        /// <returns>List<Dictionary<property, string>></returns>
        public static List<Dictionary<string, string>> GetPackages(Dictionary<string, Newtonsoft.Json.Linq.JArray> json)
        {
            //TODO:     Implement deserialization of Packages
            string name = MethodBase.GetCurrentMethod()!.Name.Remove(0, 3);
            Log.Debug("Extracting {entity}...", name);
            return json[$"{name}"].ToObject<List<Dictionary<string, string>>>()!;
        }

        /// <summary> Method to deserialize SubCommands from .json file. </summary> <param
        /// name="json">Dictionnary extracted from the json file.</param>
        /// <returns>List<Dictionary<property, string>></returns>
        public static List<Dictionary<string, object>> GetCommands(Dictionary<string, Newtonsoft.Json.Linq.JArray> json)
        {
            //TODO:     Implement deserialization of SubCommands
            string name = MethodBase.GetCurrentMethod()!.Name.Remove(0, 3);
            Log.Debug("Extracting {entity}...", name);
            return json[$"{name}"].ToObject<List<Dictionary<string, object>>>()!;
        }

        /// <summary> Method to deserialize Arguments from .json file. </summary> <param
        /// name="json">Dictionnary extracted from the json file.</param>
        /// <returns>List<Dictionary<property, string>></returns>
        public static List<Dictionary<string, object>> GetArguments(Dictionary<string, Newtonsoft.Json.Linq.JArray> json)
        {
            //TODO:     Implement deserialization of Arguments
            string name = MethodBase.GetCurrentMethod()!.Name.Remove(0, 3);
            Log.Debug("Extracting {entity}...", name);
            return json[$"{name}"].ToObject<List<Dictionary<string, object>>>()!;
        }

        /// <summary> Method to deserialize Options from .json file. </summary> <param
        /// name="json">Dictionnary extracted from the json file.</param>
        /// <returns>List<Dictionary<property, string[] or string>></returns>
        public static List<Dictionary<string, object>> GetOptions(Dictionary<string, Newtonsoft.Json.Linq.JArray> json)
        {
            //TODO:     Implement deserialization of Options
            string name = MethodBase.GetCurrentMethod()!.Name.Remove(0, 3);
            Log.Debug("Extracting {entity}...", name);
            return json[$"{name}"].ToObject<List<Dictionary<string, object>>>()!;
        }
    }

    //TODO:     Study polymorphic serialization
}