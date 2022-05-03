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
        command.AddArgument(argument);
        Log.Verbose("{A} built and added to {U}.", $"{argument}", $"{command}");
        return argument;
    }

    internal string TArgument()
    {
        string alias = Alias.Replace("-", "_");
        StringBuilder source = new();
        source.AppendLine(@$"Argument<{Type}> {alias} = new(""{alias}"");");
        source.AppendLine(@$"{alias}.Description = ""{Description}"";");
        if (DefaultValue is not null)
            source.AppendLine(@$"{alias}.SetDefaultValue(({Type})""{DefaultValue}"")");
        source.AppendLine($"{Command}.AddArgument({alias});");
        return source.ToString();
    }
}