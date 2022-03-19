using autocli.Interface;
using Newtonsoft.Json;

namespace autocli.Functionnals
{
    public static class Utils
    {
        /// <summary>
        /// Method to box the title between 2 "___...___" lines.
        /// </summary>
        /// <param name="title">Title of the application to be boxed and displayed in the cli.</param>
        /// <example></example>
        public static string Boxed(string title)
        {
            int l = title.Length;
            string box = "";
            for (int c = 1; c < l; c++)
            {
                box += "_";
            }
            box += "\n\n";
            return box + title + "\n\n" + box +
                $"Author : scalar-tns.\nHost name : {Environment.MachineName}\n" +
                $"OS : {Environment.OSVersion}\n" +
                $"Host version : .NET {Environment.Version}\n\n";
        }

        /// <summary>
        /// Method to locate a file.
        /// </summary>
        /// <param name="input">User input of the name of the file to locate in the current directory.</param>
        /// <param name="keyword">
        /// If the input is null, based on a keyword like the extension of the file or a part of its
        /// name, the method automatically locates it.
        /// </param>
        /// <returns>The corresponding file path.</returns>
        public static string Locate(string input, string keyword)
        {
            string crit = string.IsNullOrEmpty(input) ? keyword : input;
            string[] data_file = Directory.GetFiles(Directory.GetCurrentDirectory(), crit);
            return data_file[^1];
        }

        /// <summary>
        /// Method to retrieve SubCommands from .json file.
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public static List<SubCommands>? GetSubCommands(string path)
        {
            return JsonConvert.DeserializeObject<List<SubCommands>>(File.ReadAllText(path));
        }
    }
}