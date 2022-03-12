using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace autocli
{
    public static class Building
    {
        // Method to create a template configuration file
        public static void Create(string name)
        {
            string? configfile = Path.Combine(Environment.CurrentDirectory, name) + ".json";
            File.Copy(sourceFileName: @"C:\Users\matte\source\repos\autoCLI\input.json", destFileName: configfile, overwrite: true);
            Console.WriteLine($"Configuration file for {name} project created at :\n\n{configfile}\n");
        }

        // Method to generate the project
        public static void Generation(string file_path)
        {
            // Retrieve project name
            string project_name = Path.GetFileNameWithoutExtension(file_path);
            Console.WriteLine($"Starting {project_name} generation at :\n\n{file_path}\n");
        }
    }
}