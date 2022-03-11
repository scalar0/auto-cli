# autoCLI

## Configuration file .json

### 1 Packages

```json
  "packages": [
    {
      "name": "System.CommandLine",
      "cli": "dotnet add package System.CommandLine --prerelease",
      "replace": "PACKAGE"
    },
    {
      "name": "System.CommandLine.Hosting",
      "cli": "dotnet add package System.CommandLine.Hosting --prerelease",
      "replace": "PACKAGE"
    }
  ]
```

### 2 Commands

```json
"commands": [
    {
      "name": "cmd",
      "type": "string",
      "call": "cmdlet",
      "decription": {
        "object": "description",
        "replace": "cmd.DESCSRIPTION"
      }
    }
  ]
  ```

### 3 Subcommands

```json
  "subcommands": [
    {
      "name": "subcmd",
      "parent": "cmd",
      "type": "string",
      "call": "subcmdlet",
      "alias": {
        "present": "true",
        "value": "-c"
      },
      "default": {
        "present": "true",
        "value": "value"
      },
      "decription": {
        "object": "description",
        "replace": "subcmd.DESCSRIPTION"
      }
    }
  ]
  ```

### 4 Arguments

```json
  "arguments": [
    {
      "name": "arg",
      "parent": "cmd",
      "type": "string",
      "cmdlet": "arg",
      "default": {
        "present": "true",
        "value": "value"
      },
      "decription": {
        "object": "description",
        "replace": "arg.DESCSRIPTION"
      }
    }
  ]
  ```

### 5 Options

```json
  "options": [
    {
      "name": "option",
      "parent": "cmd",
      "type": "string",
      "required": "true",
      "cmdlet": "--option",
      "alias": {
        "present": "true",
        "value": "-o"
      },
      "default": {
        "present": "true",
        "value": "value"
      },
      "decription": {
        "object": "description",
        "replace": "option.DESCSRIPTION"
      }
    }
  ]
  ```
