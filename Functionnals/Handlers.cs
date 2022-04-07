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
                                  string verbosity)
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
        public static void generate(Interface.IProperty AppProperties,
                                    List<Interface.IPackage> Packages,
                                    string verbosity)
        {
            // TODO : Implement verbosity level of generate
            InstallProject(AppProperties);
            InstallPackages(Packages);
        }

        public static void InstallProject(Interface.IProperty AppProperties)
        {
            // Retrieve project name
            string project_name = AppProperties.Name!;
            Log.Information("Creating new console application. Target framework : net6.0.");
            Console.WriteLine($"Project name : {project_name}");
            Utils.ExecuteCommandSync("dotnet new console --name " + project_name + ".CLI --framework net6.0 --output " + AppProperties.OutputPath + @"\" + project_name + ".CLI");
        }

        public static void InstallPackages(List<Interface.IPackage> Packages)
        {
            // TODO : Implement installation of packages
            foreach (Interface.IPackage pack in Packages)
            {
                Log.Information("Installing package: " + pack.Name);
                Console.WriteLine("Installing package: " + pack.Name);
                Utils.ExecuteCommandSync("dotnet [parse] add package " + pack.Name + " --prerelease");
            }
        }

        public static void CallHandlers(Interface.IJsonApp self)
        {
            Option verbose = self.GetOptions()[^1];

            self.GetCommand("create")!.SetHandler(
                (string name, string directory, string verbosity) => create(name, directory, verbosity),
                self.GetArgument("name")!,
                self.GetOption("directory")!,
                verbose);

            self.GetCommand("generate")!.SetHandler(
                (string verbosity) => generate(self.GetProperties(), self.GetPackages(), verbosity),
                self.GetArgument("file")!,
                verbose);

            Log.Debug("Handlers implemented.");
        }
    }
}