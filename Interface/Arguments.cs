namespace autocli
{
    public static class Arguments
    {
        //ARG file_name (COMMAND creation)
        public static Argument<string>? _file_name(Command command)
        {
            return Builders.MakeArgument(
                command: command,
                symbol: "name",
                defaultvalue: Path.GetDirectoryName(Environment.CurrentDirectory),
                description: "Name of .json configuration file.") as Argument<string>;
        }

        //ARG file_path (COMMAND generation)
        public static Argument<string>? _file_path(Command command)
        {
            return Builders.MakeArgument(
                command: command,
                symbol: "file",
                defaultvalue: Utils.Locate("", "*.json"),
                description: "Path to .json configuration file.") as Argument<string>;
        }
    }
}