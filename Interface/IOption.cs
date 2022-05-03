using Newtonsoft.Json;

namespace autocli.Interface;

/// <summary>
/// Option Interface class to deserialize the options of the application.
/// </summary>
internal class IOption
{
    #region Properties

    [JsonProperty("Aliases")]
    internal string[] Aliases { get; set; } = null!;

    [JsonProperty("Type")]
    internal string Type { get; set; } = null!;

    [JsonProperty("Command")]
    internal string Command { get; set; } = null!;

    [JsonProperty("Required")]
    internal bool Required { get; set; }

    [JsonProperty("DefautlValue")]
    internal string? DefaultValue { get; set; }

    [JsonProperty("Global")]
    internal bool? Global { get; set; }

    [JsonProperty("Description")]
    internal string Description { get; set; } = null!;

    [JsonProperty("Values")]
    internal string[] Values { get; set; } = null!;

    #endregion Properties

    /// <summary>
    /// Constructs a new instance of the IOption class.
    /// </summary>
    /// <param name="command">Parent command.</param>
    /// <returns>Corresponding Option.</returns>
    internal Option BuildOption(Command command)
    {
        Option<string> option = new(Aliases);
        option.IsRequired = Required;
        option.Description = Description;
        if (DefaultValue is not null) option.SetDefaultValue(DefaultValue);
        if (Values is not null) option.FromAmong(Values);
        switch (Global)
        {
            case true:
                command.AddGlobalOption(option);
                break;

            default:
                command.AddOption(option);
                break;
        }
        Log.Verbose("{O} built and added to {U}.", $"{option}", $"{command}");
        return option;
    }

    internal string TOption()
    {
        string name = Aliases[0].Replace("-", "");
        StringBuilder source = new();
        source.AppendLine("\n" + @$"Option<{Type}> {name} = new(""{Aliases[0]}"");");
        if (Aliases.Length > 1)
        {
            source.AppendLine(@$"{name}.AddAlias(""{Aliases[^1]}"");");
        }

        source.AppendLine(@$"{name}.Description = ""{Description}"";");
        source.AppendLine($"{name}.IsRequired = {Required.ToString().ToLower(new System.Globalization.CultureInfo("en-US", false))};");
        if (DefaultValue is not null)
        {
            source.AppendLine(@$"{name}.SetDefaultValue(({Type})""{DefaultValue}"");");
        }
        if (Values is not null)
        {
            source.AppendLine(@$"{name}.FromAmong(new string[] {{{string.Join(", ", Values.Select(v => $"\"{v}\""))}}});");
        }
        switch (Global)
        {
            case true:
                source.AppendLine($"{Command}.AddGlobalOption({name});");
                break;

            default:
                source.AppendLine($"{Command}.AddOption({name});");
                break;
        }
        return source.ToString();
    }
}