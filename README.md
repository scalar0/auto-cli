# Automated Command Line Interface Creation

The auto-CLI tool aims to automate .NET 6.0.\* CLI applications development based on an input architecture of the project's commands, subcommands, options, arguments and properties.
The choice was made to store said architecture in a .json configuration file, whose structure is described in the following sections.

## Configuration file

The configuration file stores the architecture for the project's commands, subcommands, options, arguments and properties in a .json format, deserializable by the [JSON.NET](https://www.newtonsoft.com) library.

### _Properties_

The properties array, stores in a json array the name of the project, the title and the description of the project that will be displayed in the CLI. The output path property is the path to the project repository. The framework property is the target framework for the project. The repo property is the name of the git repository.

<details>
<summary><b><u>
Properties Json Array
</u></b></summary>

```json
"Properties": [
    {
      "Name": "name",
      "Title": "boxed title",
      "Description": "project description",
      "OutputPath": "/repos/name/",
      "Framework": "net6.0",
      "Repo": "repo-name"
    }
  ],
```

</details>

### _Packages_

The package array, stores in a json array the name and version of each NuGet package required by the project. The array must at least contain the following packages in order for the auto-generated interface to work and to implement logging.

<details>
<summary><b><u>
Packages Json Array
</u></b></summary>

```json
"Packages": [
    {
      "Name": "System.CommandLine",
      "Version": "--prerelease"
    },
    {
      "Name": "Newtonsoft.Json",
      "Version": "--prerelease"
    },
    {
      "Name": "Serilog",
      "Version": "--prerelease"
    },
    {
      "Name": "Serilog.Sinks.Console",
      "Version": "--prerelease"
    },
    {
      "Name": "Serilog.Sinks.File",
      "Version": "--prerelease"
    },
    {
      "Name": "Sentry",
      "Version": null
    },
    {
      "Name": "Package.Name",
      "Version": "--version"
    }
  ]
```

</details>

Every CLI application built using this tool will rely on the [System.CommandLine API](https://github.com/dotnet/command-line-api) for command line parsing, on [Newtonsoft.Json](https://www.newtonsoft.com/json) to deserialize and build the interface and on [Serilog](https://serilog.net/) for logging.

### _Commands_

The commands array stores in a json array the alias, description, the verbosity option setting and parent of each command of the interface. The alias is the command let to call on the CLI in order to invoke the command and parse its arguments. The description is the text displayed in the help menu of the CLI application. The parent is the parent of the command (e.g. : the root command).

<details>
<summary><b><u>
Commands Json Array
</u></b></summary>

```json
  "Commands": [
    {
      "Alias": "alias",
      "Parent": "parent",
      "Description": "description"
    }
  ]
```

</details>

### _Arguments_

The arguments array stores in a json array the alias, description, the type of the argument and the parent of each argument of the interface. The alias is the argument to call on the CLI in order to invoke the argument. The description is the text displayed in the help menu of the CLI application. The type is the type of the argument (e.g. : string, int, bool, etc.). The parent is the parent of the argument (e.g. : the command). The defaultvalue property is used to set the value of the argument if no value is provided.

<details>
<summary><b><u>
Arguments Json Array
</u></b></summary>

```json
  "Arguments": [
    {
      "Alias": "<name>",
      "Type": "Type",
      "Command": "command-alias",
      "Defaultvalue": null,
      "Description": "description"
    }
  ]
```

</details>

### _Options_

The options array stores in a json array the alias, description, the type of the option and the parent of each option of the interface. The name of the option is used to uniquely bind it instead of relying on the aliases string array. The description is the text displayed in the help menu of the CLI application. The type is the type of the option (e.g. : string, int, bool, etc.). The parent is the parent of the option (e.g. : the command). The defaultvalue property is used to set the value of the option if no value is provided. The requiered property is used to set the option as required or not.
The values array stores the accepted values of the option to be parsed.

<details>
<summary><b><u>
Options Json Array
</u></b></summary>

```json
  "Options": [
    {
      "Name": "name",
      "Aliases": ["--option", "-o"],
      "Type": "Type",
      "Command": "command-alias",
      "Required": "bool",
      "Defaultvalue": "string",
      "Description": "description",
      "Values": ["value1", "value2"]
    }
  ]
```

</details>
