namespace autocli
{
    public static class Commands
    {
        public static Command _generation(RootCommand ROOTCOMMAND)
        {
            return Builders.MakeCommand(
                command: ROOTCOMMAND,
                symbol: "generate",
                description: "Generate the CLI project based on the input .json configuration file.");
        }

        public static Command _creation(RootCommand ROOTCOMMAND)
        {
            return Builders.MakeCommand(
                    command: ROOTCOMMAND,
                    symbol: "create",
                    description: "Creates a template of a new .json configuration file with specified name.");
        }

        public static Command _testing(RootCommand ROOTCOMMAND)
        {
            return Builders.MakeCommand(
                    command: ROOTCOMMAND,
                    symbol: "testing",
                    description: "Test command.");
        }
    }
}