// This file is supposed to be auto-generated

namespace autocli
{
    public static class Arguments
    {
        public static Argument _file_name(Command creation)
        {
            return Builders.MakeArgument<string>(
                command: creation,
                symbol: "name",
                defaultvalue: null,
                description: "Name of .json configuration file.");
        }

        public static Argument _file_path(Command generation)
        {
            return Builders.MakeArgument<string>(
                command: generation,
                symbol: "file",
                defaultvalue: null,
                description: "Path to .json configuration file.");
        }
    }
}