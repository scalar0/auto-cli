namespace autocli
{
    internal static class Func
    {
        // Method to automatically locate app.json input file
        internal static string Locate(string input, string keyword)
        {
            string crit = string.IsNullOrEmpty(input) ? keyword : input;
            string[] data_file = Directory.GetFiles(Directory.GetCurrentDirectory(), crit);
            return data_file[^1];
        }

        internal static void Create(string name)
        {
            string? configfile = Path.Combine(Environment.CurrentDirectory, name) + ".json";
            File.Copy(sourceFileName: @"C:\Users\matte\source\repos\autoCLI\input.json", destFileName: configfile, overwrite: true);
            Console.WriteLine($"Configuration file created at {configfile}");
        }

        internal static void Generation(string file_path)
        {
            string project_name = Path.GetFileName(file_path);
            Console.WriteLine($"Starting {file_path} : {project_name} generation...");
        }
    }
}