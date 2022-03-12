﻿namespace autocli
{
    internal static class Utils
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
            Console.WriteLine($"Configuration file for {name} project created at :\n\n{configfile}\n");
        }

        internal static void Generation(string file_path)
        {
            // Retrieve project name
            string project_name = Path.GetFileNameWithoutExtension(file_path);
            Console.WriteLine($"Starting {project_name} generation at :\n\n{file_path}\n");
        }
    }
}