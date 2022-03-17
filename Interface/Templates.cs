namespace autocli.Interface
{
    // TODO:    Template for each entity
    public static class Templates
    {
        public static void _RootCommand(
            string title,
            string description,
            bool setverbosity)
        {
        }

        public static void _Command(
            Command command,
            string symbol,
            string? defaultvalue,
            string description,
            bool setverbosity)
        {
        }

        public static void _Argument(
            Type T,
            Command command,
            string symbol,
            string? defaultvalue,
            string description)
        {
        }

        public static void _Option(
            Type T,
            Command command,
            bool required,
            string symbol,
            string? alias,
            string? defaultvalue,
            string description)
        {
        }
    }
}