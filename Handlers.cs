﻿namespace autocli.Functionnals;

public static class Handlers
{
    /// <summary>
    /// Handler for the create command.
    /// </summary>
    /// <param name="name"></param>
    /// <param name="directory"></param>
    public static void create(string name,
                              string directory)
    {
        string? configfile = Path.Combine(directory, "Architecture." + name) + ".json";
        File.Copy(sourceFileName: "\\input.json", destFileName: configfile, overwrite: true);
        Console.WriteLine($">> Configuration file for {name} project created at :\n>> {configfile}");
    }

    /// <summary>
    /// Handler for the generate command.
    /// </summary>
    /// <param name="file"></param>
    public static void generate(string file)
    {
        Interface.ConsoleApp Interface = new(file, gen: true);
        Interface.InstallProject();
        Interface.InstallPackages();
    }
}