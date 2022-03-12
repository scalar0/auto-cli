namespace autocli
{
    public static class Options
    {
        public static Option<string>? _pushing(Command command)
        {
            //OPTION push to github (COMMAND generation)
            return Builders.MakeOption(
                command: command,
                required: false,
                symbol: "--push",
                alias: "-p",
                defaultvalue: "n",
                description: "Push to GitHub with project name ? (y/n)") as Option<string>;
        }

        // TEMPLATE
        /*        "public static Option<{type}>? _{name}(RootCommand ROOTCOMMAND)
                {
                    //OPTION push to github (COMMAND generation)
                    return Builders.MakeOption(
                        command: Commands.Construct_{command}(ROOTCOMMAND),
                        required: {required},
                        symbol: "{symbol}",
                        alias: "{alias}",
                        defaultvalue: "{defaultvalue}",
                        description: "{description}") as Option<{type}>;
                }"*/
    }
}