using Newtonsoft.Json;

namespace autocli.Interface;

/// <summary>
/// Option Interface class to deserialize the options of the application.
/// </summary>
internal class IOption
{
    #region Properties

    [JsonProperty("Name")]
    internal string Name { get; set; } = null!;

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
        option.Name = Name;
        option.IsRequired = Required;
        option.Description = Description;
        if (DefaultValue is not null) option.SetDefaultValue(DefaultValue);
        if (Values is not null) option.FromAmong(Values);
        try
        {
            command.AddOption(option);
            Log.Verbose("{O} built and added to {U}.", $"{option}", $"{command}");
        }
        catch (Exception ex)
        {
            Log.Error(ex, ex.Message, ex.ToString);
        }
        return option;
    }

    internal string TOption()
    {
        string name = Name.Replace("-", "_");
        string source = "\n" + @$"Option<{Type}> {name.Replace("-", "_")} = new(""{Aliases[0]}"");" +
            "\n";
        if (Aliases.Length > 1)
        {
            source += @$"{name}.AddAlias(""{Aliases[^1]}"");" + "\n";
        }

        source += @$"{name}.Description = ""{Description}"";" +
            "\n";
        source += $"{name}.IsRequired = {Required.ToString().ToLower(new System.Globalization.CultureInfo("en-US", false))};\n";
        if (DefaultValue is not null)
        {
            source += @$"{name}.SetDefaultValue(({Type})""{DefaultValue}"");" +
            "\n";
        }
        if (Values is not null)
        {
            source += @$"{name}.FromAmong(new string[] {{{string.Join(", ", Values.Select(v => $"\"{v}\""))}}});" +
            "\n";
        }

        source += $"{Command}.AddOption({name});\n";
        return source;
    }
}