using autocli.Interface;
using Newtonsoft.Json;

namespace autocli.Functionnals
{
    internal static class ParseArchitecture
    {
        /// <summary>
        /// Method to polymorphically deserialize SubCommands from .json file.
        /// </summary>
        /// <param name="json"></param>
        /// <returns></returns>
        public static List<SubCommand>? GetSubCommands(string json)
        {
            //TODO:     Implement deserialization of SubCommands
            return JsonConvert.DeserializeObject<List<SubCommand>>(File.ReadAllText(json));
        }

        /// <summary>
        /// Method to polymorphically deserialize Arguments from .json file.
        /// </summary>
        /// <param name="json"></param>
        /// <returns></returns>
        public static List<Arguments>? GetArguments(string json)
        {
            //TODO:     Implement deserialization of Arguments
            return JsonConvert.DeserializeObject<List<Arguments>>(File.ReadAllText(json));
        }

        /// <summary>
        /// Method to polymorphically deserialize Options from .json file.
        /// </summary>
        /// <param name="json"></param>
        /// <returns></returns>
        public static List<Options>? GetOptions(string json)
        {
            //TODO:     Implement deserialization of Options
            return JsonConvert.DeserializeObject<List<Options>>(File.ReadAllText(json));
        }

        /// <summary>
        /// Method to retrieve properties of the application.
        /// </summary>
        /// <param name="path">Path of .json configuration file.</param>
        /// <returns>String array of properties.</returns>
        public static string[] GetProperties(string path)
        {
            List<string> properties = new();
            //TODO:     Implement GetProperties method
            return properties.ToArray();
        }
    }
}