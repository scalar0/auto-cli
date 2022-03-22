namespace autocli.Interface
{
    public static class Templates
    {
        public static string _Commands(
            string name,
            Command command,
            string symbol,
            string description,
            bool setverbosity)
        {
            return
        @$"public static Command _{name}(RootCommand {command})
        {{
            return Constructors.MakeCommand(
                command: {command},
                symbol: {symbol},
                description: {description},
                setverbosity: {setverbosity});
        }}";
        }

        public static string _Arguments(
            Type T,
            string name,
            Command command,
            string symbol,
            string defaultvalue,
            string description)
        {
            return
        $@"public static Argument<{T}> _{name}(Command {command})
        {{
            return Constructors.MakeArgument<{T}>(
                command: {command},
                symbol: {symbol},
                defaultvalue: {defaultvalue},
                description: {description});
        }}";
        }

        public static string _Options(
            Type T,
            string name,
            Command command,
            bool required,
            string symbol,
            string? alias,
            string? defaultvalue,
            string description)
        {
            return
        $@"public static Option<{T}> _{name}(Command {command})
        {{
            return Constructors.MakeOption<{T}>(
                command: {command},
                required: {required},
                symbol: {symbol},
                alias: {alias},
                defaultvalue: {defaultvalue},
                description: {description});
        }}";
        }
    }
}