namespace autocli.Functionnals
{
    public static class Handlers
    {
        /// <summary>
        /// Handler for the create command.
        /// </summary>
        /// <param name="name"></param>
        /// <param name="directory"></param>
        public static void create(string name,
                                  DirectoryInfo directory)
        {
            string? configfile = Path.Combine(directory.FullName, name) + ".Arcitecture.json";
            File.Copy(sourceFileName: @"C:\Users\matte\source\repos\autoCLI\input.json", destFileName: configfile, overwrite: true);
            Console.WriteLine($"Configuration file for {name} project created at :\n{configfile}");
        }

        /// <summary>
        /// Handler for the generate command.
        /// </summary>
        /// <param name="path"></param>
        public static void generate(Interface.Properties AppProperties,
                                    List<Interface.Packages> Packages)
        {
            InstallProject(AppProperties);
            InstallPackages(Packages);
        }

        public static void InstallProject(Interface.Properties AppProperties)
        {
            // Retrieve project name
            string project_name = AppProperties.Name!;
            Log.Information("Creating new console application.");
            Console.WriteLine($"Project name : {project_name}");
            Utils.ExecuteCommandSync("dotnet new console -h");
            Utils.ExecuteCommandSync("dotnet [parse] new console --name " + project_name + ".CLI -f net6.0 --output " + AppProperties.OutputPath);
        }

        public static void InstallPackages(List<Interface.Packages> Packages)
        {
            // TODO : Implement installation of packages
            foreach (Interface.Packages pack in Packages)
            {
                Log.Information("Installing package: " + pack.Name);
                Console.WriteLine("Installing package: " + pack.Name);
                Utils.ExecuteCommandSync("dotnet [parse] add package " + pack.Name + " --prerelease");
            }
        }

        public static void CallHandlers(List<Command> Commands,
                                        List<Argument> Arguments,
                                        List<Option> Options,
                                        Interface.Properties AppProperties,
                                        List<Interface.Packages> Packages)
        {
            Interface.Constructors.GetCommand(Commands, "create")!.SetHandler((string name, DirectoryInfo dir) => Handlers.create(name, dir), Interface.Constructors.GetArgument(Arguments, "name")!, Interface.Constructors.GetOption(Options, new string[] { "--directory", "-d" })!);

            Interface.Constructors.GetCommand(Commands, "generate")!.SetHandler((string _) => Handlers.generate(AppProperties, Packages), Interface.Constructors.GetArgument(Arguments, "file")!);
        }
    }
}