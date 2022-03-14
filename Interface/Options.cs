// This file is supposed to be auto-generated

namespace autocli
{
    public static class Options
    {
        //TODO:    Implement verbosity option
        public static Option _verbosity(Command generation)
        {
            return Builders.MakeOption<string>(
                command: generation,
                required: false,
                symbol: "--verbosity",
                alias: "-v",
                defaultvalue: "m",
                description: "Choix de verbosité de sortie : q[uiet]; m[inimal]; diag[nostic].");
        }

        public static Option _pushing(Command generation)
        {
            return Builders.MakeOption<string>(
                command: generation,
                required: false,
                symbol: "--push",
                alias: "-p",
                defaultvalue: "n",
                description: "Push to GitHub with repo-name ? (y/n)");
        }

        public static Option _dir_choice(Command creation)
        {
            return Builders.MakeOption<DirectoryInfo>(
                command: creation,
                required: true,
                symbol: "--directory",
                alias: "-d",
                defaultvalue: null,
                description: "Specify the directory output");
        }
    }
}