namespace autocli
{
    public static class Handlers
    {
        // Method to create a template configuration file
        public static void Create_Template(string name, DirectoryInfo directory)
        {
            string? configfile = Path.Combine(directory.FullName, name) + ".Arcitecture.json";
            File.Copy(sourceFileName: @"C:\Users\matte\source\repos\autoCLI\input.json", destFileName: configfile, overwrite: true);
            Console.WriteLine($"Configuration file for {name} project created at :\n\n{configfile}\n");
        }

        // Method to generate the project
        public static void Generate(string path)
        {
            // Retrieve project name
            string project_name = Path.GetFileNameWithoutExtension(path);
            Console.WriteLine($"Starting {project_name} generation at :\n\n{path}\n");
        }
    }
}