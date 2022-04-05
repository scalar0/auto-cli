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
                                  string directory,
                                  string verbosity = null)
        {
            // TODO : Implement verbosity level of create
            string? configfile = Path.Combine(directory, "Arcitecture." + name) + ".json";
            File.Copy(sourceFileName: @"C:\Users\matte\source\repos\autoCLI\input.json", destFileName: configfile, overwrite: true);
            Console.WriteLine($"Configuration file for {name} project created at :\n{configfile}");
        }

        /// <summary>
        /// Handler for the generate command.
        /// </summary>
        /// <param name="path"></param>
        public static void generate(Interface.Properties AppProperties,
                                    List<Interface.Packages> Packages,
                                    string verbosity = null)
        {
            // TODO : Implement verbosity level of generate
            InstallProject(AppProperties);
            InstallPackages(Packages);
        }

        public static void InstallProject(Interface.Properties AppProperties)
        {
            // Retrieve project name
            string project_name = AppProperties.Name!;
            Log.Information("Creating new console application. Target framework : net6.0.");
            Console.WriteLine($"Project name : {project_name}");
            Utils.ExecuteCommandSync("dotnet new console --name " + project_name + ".CLI --framework net6.0 --output " + AppProperties.OutputPath + @"\" + project_name + ".CLI");
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
                                        Option verbose,
                                        Interface.Properties AppProperties,
                                        List<Interface.Packages> Packages)
        {
            Interface.Constructors.Get(Commands, "create")!.SetHandler(
                (string name, string directory, string verbosity) => Handlers.create(name, directory, verbosity),
                Interface.Constructors.Get(Arguments, "name")!,
                Interface.Constructors.Get(Options, "directory")!,
                verbose);

            Interface.Constructors.Get(Commands, "generate")!.SetHandler(
                (string verbosity) => Handlers.generate(AppProperties, Packages, verbosity),
                Interface.Constructors.Get(Arguments, "file")!,
                verbose);
        }
    }
}