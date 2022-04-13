namespace autocli.Interface;

/// <summary>
/// Interface Argument class to deserialize the args of the interface.
/// </summary>
internal class IArgument
{
    [JsonProperty("Alias")]
    internal string Alias { get; set; }

    [JsonProperty("Type")]
    internal string Type { get; set; }

    [JsonProperty("Command")]
    internal string Command { get; set; }

    [JsonProperty("DefautlValue")]
    internal string? DefaultValue { get; set; }

    [JsonProperty("Description")]
    internal string Description { get; set; }

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
        string source = $"Argument<{Type}> {Alias} = new Argument<{Type}>({Alias});\n";
        source += $"{Alias}.Description = {Description};\n";
        source += $"{Alias}.SetDefaultValue(({Type}){DefaultValue})\n";
        source += $"{Command}.AddArgument({Alias});\n";
        return source;
    }
}