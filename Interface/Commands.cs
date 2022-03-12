namespace autocli
{
    public static class Commands
    {
        public static Command _generation(RootCommand ROOTCOMMAND)
        {
            var generation = Builders.MakeCommand(
                command: ROOTCOMMAND,
                symbol: "generate",
                description: "Generate the CLI project based on the input .json configuration file.");
            if (generation != null) return generation;
            else throw new ApplicationException("generation command is null");
        }

        public static Command _creation(RootCommand ROOTCOMMAND)
        {
            var creation = Builders.MakeCommand(
                    command: ROOTCOMMAND,
                    symbol: "create",
                    description: "Creates a template of a new .json configuration file with specified name.");
            if (creation != null) return creation;
            else throw new ApplicationException("creation command is null");
        }
    }
}