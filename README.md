# autoCLI

auto-CLI aims to automate .NET 6.0.* CLI applications development based on an input architecture stored in a .json file.
The configuration file stores the architecture for the project's commands, subcommands, options and arguments.

<img src="data:image/png;base64,iVBORw0KGgoAAAANSUhEUgAAAhMAAABHCAYAAAC9I8XsAAAAAXNSR0IArs4c6QAABLN0RVh0bXhmaWxlACUzQ214ZmlsZSUyMGhvc3QlM0QlMjJFbGVjdHJvbiUyMiUyMG1vZGlmaWVkJTNEJTIyMjAyMi0wMy0xMlQxMCUzQTQ5JTNBNDEuMDk1WiUyMiUyMGFnZW50JTNEJTIyNS4wJTIwKFdpbmRvd3MlMjBOVCUyMDEwLjAlM0IlMjBXaW42NCUzQiUyMHg2NCklMjBBcHBsZVdlYktpdCUyRjUzNy4zNiUyMChLSFRNTCUyQyUyMGxpa2UlMjBHZWNrbyklMjBkcmF3LmlvJTJGMTYuNS4xJTIwQ2hyb21lJTJGOTYuMC40NjY0LjExMCUyMEVsZWN0cm9uJTJGMTYuMC43JTIwU2FmYXJpJTJGNTM3LjM2JTIyJTIwZXRhZyUzRCUyMlU3VnRlOVQ4WkRaX3pobU9wcUNQJTIyJTIwdmVyc2lvbiUzRCUyMjE2LjUuMSUyMiUyMHR5cGUlM0QlMjJkZXZpY2UlMjIlM0UlM0NkaWFncmFtJTIwaWQlM0QlMjJNRDg5Qm1mUFZwT082a3NPMEhnOCUyMiUyMG5hbWUlM0QlMjJQYWdlLTElMjIlM0U1VlpkYjVzd0ZQMDFQTGJpSTRUdHNTWHBPcWxydFdaVDIwY1hic0dwNFZyR0ZOaXZuNEZMQ0VMSk9xbUtKdTBwdnNmWDl2VTU1NXBZWHBqVlh4U1Q2VGVNUVZpdUhkZVd0N0pjMTdIZGhmbHBrYVpIZ3NEcGdVVHhtSkpHWU1OJTJGd2JDUzBKTEhVRXdTTmFMUVhFN0JDUE1jSWozQm1GSllUZE5lVUV4UGxTeUJHYkNKbUppakR6eldhWTklMkJjb01SdndhZXBNUEp6dkp6UDVPeElabTJLRklXWTlWRDNlVzh0ZVdGQ2xIM282d09RYlRrRGJ6MERGd2RtTjBWcGlEWDcxbHdGWDYlMkZYYnclMkIzZnI2OGY0NkVxczZ1OSUyQmVMYWsyM1F3WGh0amNuMEpVT3NVRWN5YldJM3Fwc014amFIZTFUVFRtM0NCS0F6b0czSUxXRFluSlNvMEdTblVtYU5ZVXJKcEhXdDhGVDIxdzdnJTJGaHF0NmZYRFVVOWJXMkJSNmtZT0FhU3hYQmtY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" style="cursor:pointer;max-width:100%;" onclick="(function(img){if(img.wnd!=null&&!img.wnd.closed){img.wnd.focus();}else{var r=function(evt){if(evt.data=='ready'&&evt.source==img.wnd){img.wnd.postMessage(decodeURIComponent(img.getAttribute('src')),'*');window.removeEventListener('message',r);}};window.addEventListener('message',r);img.wnd=window.open('https://viewer.diagrams.net/?client=1&page=0&edit=_blank');}})(this);"/>

## Configuration file

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
