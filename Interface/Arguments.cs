namespace autocli
{
    public static class Arguments
    {
        //ARG file_name (COMMAND creation)
        public static Argument<string>? _file_name(Command creation)
        {
#pragma warning disable CS8604 // Possible null reference argument.
            return Builders.MakeArgument(
                command: creation,
                symbol: "name",
                defaultvalue: Path.GetDirectoryName(Environment.CurrentDirectory),
                description: "Name of .json configuration file.") as Argument<string>;
#pragma warning restore CS8604 // Possible null reference argument.
        }

        //ARG file_path (COMMAND generation)
        public static Argument<string>? _file_path(Command generation)
        {
            return Builders.MakeArgument(
                command: generation,
                symbol: "file",
                defaultvalue: Utils.Locate("", "*.json"),
                description: "Path to .json configuration file.") as Argument<string>;
        }
    }
}