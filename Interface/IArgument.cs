using Newtonsoft.Json;

namespace autocli.Interface;

/// <summary>
/// Interface Argument class to deserialize the args of the interface.
/// </summary>
internal class IArgument
{
    #region Properties

    [JsonProperty("Alias")]
    internal string Alias { get; set; } = null!;

    [JsonProperty("Type")]
    internal string Type { get; set; } = null!;

    [JsonProperty("Command")]
    internal string Command { get; set; } = null!;

    [JsonProperty("DefautlValue")]
    internal string? DefaultValue { get; set; }

    [JsonProperty("Description")]
    internal string Description { get; set; } = null!;

    #endregion Properties

    internal Argument BuildArgument(Command command)
    {
        Argument<string> argument = new(Alias);
        argument.Description = Description;
        if (DefaultValue is not null) argument.SetDefaultValue(DefaultValue);
        try
        {
            command.AddArgument(argument);
            Log.Verbose("{A} built and added to {U}.", $"{argument}", $"{command}");
        }
        catch (Exception ex)
        {
            Log.Error(ex, ex.Message, ex.ToString);
        }
        return argument;
    }

    internal string TArgument()
    {
        string alias = Alias.Replace("-", "_");
        string source = "\n" + @$"Argument<{Type}> {alias} = new(""{alias}"");" +
            "\n";
        source += @$"{alias}.Description = ""{Description}"";" +
            "\n";
        if (DefaultValue is not null)
            source += @$"{alias}.SetDefaultValue(({Type})""{DefaultValue}"")" +
            "\n";
        source += $"{Command}.AddArgument({alias});\n";
        return source;
    }
}