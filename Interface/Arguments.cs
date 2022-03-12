namespace autocli
{
    public static class Arguments
    {
        //ARG file_name (COMMAND creation)
        public static Argument<string>? _file_name(RootCommand ROOTCOMMAND)
        {
            return Builders.MakeArgument(
                command: Commands._creation(ROOTCOMMAND),
                symbol: "name",
                defaultvalue: Path.GetDirectoryName(Environment.CurrentDirectory),
                description: "Name of .json configuration file.") as Argument<string>;
        }

        //ARG file_path (COMMAND generation)
        public static Argument<string>? _file_path(RootCommand ROOTCOMMAND)
        {
            return Builders.MakeArgument(
                command: Commands._generation(ROOTCOMMAND),
                symbol: "file",
                defaultvalue: Utils.Locate("", "*.json"),
                description: "Path to .json configuration file.") as Argument<string>;
        }
    }
}