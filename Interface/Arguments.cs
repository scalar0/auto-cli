// This file is supposed to be auto-generated

namespace autocli.Interface
{
    public static class Arguments
    {
        public static Argument<string> _file_name(Command creation)
        {
            return Constructors.MakeArgument<string>(
                command: creation,
                symbol: "name",
                defaultvalue: null,
                description: "Name of .json configuration file.");
        }

        public static Argument<string> _file_path(Command generation)
        {
            return Constructors.MakeArgument<string>(
                command: generation,
                symbol: "file",
                defaultvalue: null,
                description: "Path to .json configuration file.");
        }
    }
}