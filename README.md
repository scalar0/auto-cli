# autoCLI

auto-CLI aims to automate .NET 6.0.* CLI applications development based on an input architecture stored in a .json file.
The configuration file stores the architecture for the project's commands, subcommands, options, arguments and properties.

## Configuration file

### 1 Packages

```json
  "packages": [
    {
      "name": "System.CommandLine",
      "cli": "dotnet add package System.CommandLine --prerelease"
    },
    {
      "name": "System.CommandLine.Hosting",
      "cli": "dotnet add package System.CommandLine.Hosting --prerelease"
    },
    {
      "name": "System.CommandLine.NamingConventionBinder",
      "cli": "dotnet add package System.CommandLine.NamingConventionBinder --prerelease"
    }
  ]
```

### 2 Commands

```json
  "commands": [
    {
      "name": "subcommand",
      "command": "upcommand",
      "symbol": "cmdlet",
      "decription": "description"
    }
  ]
  ```

### 3 Arguments

```json
  "arguments": [
    {
      "name": "argument",
      "type": "string",
      "command": "cmd",
      "symbol": "<arg>",
      "defaultvalue": "value",
      "decription": "description"
    }
  ]
  ```

### 4 Options

```json
  "options": [
    {
      "name": "option",
      "type": "string",
      "command": "cmd",
      "required": "true",
      "symbol": "--option",
      "alias": "-o",
      "defaultvalue": "value",
      "decription": "description"
    }
  ]
  ```
