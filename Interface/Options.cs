﻿// This file is supposed to be auto-generated

namespace autocli
{
    public static class Options
    {
        public static Option<string> _pushing(Command generation)
        {
            return Builders.MakeOption<string>(
                command: generation,
                required: false,
                symbol: "--push",
                alias: "-p",
                defaultvalue: "n",
                description: "Push to GitHub with repo-name ? (y/n)");
        }

        public static Option<DirectoryInfo> _dir_choice(Command creation)
        {
            return Builders.MakeOption<DirectoryInfo>(
                command: creation,
                required: true,
                symbol: "--directory",
                alias: "-d",
                defaultvalue: null,
                description: "Specify the directory output.");
        }
    }
}