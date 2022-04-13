namespace autocli.Interface;

/// <summary>
/// Interface Command class to deserialize the commands of the interface.
/// </summary>
internal class ICommand
{
    [JsonProperty("Alias")]
    internal string Alias { get; set; }

    [JsonProperty("Parent")]
    internal string Parent { get; set; }

    [JsonProperty("Description")]
    internal string Description { get; set; }

    /// <summary>
    /// Constructs a new instance of the Command class.
    /// </summary>
    /// <param name="parent">Parent Command.</param>
    /// <returns>Corresponding Command.</returns>
    internal Command BuildCommand(Command parent)
    {
        Command cmd = new(Alias);
        cmd.Description = Description;
        try
        {
            parent.AddCommand(cmd);
            Log.Verbose("{C} built and added to {U}.", $"{cmd}", $"{parent}");
        }
        catch (Exception ex)
        {
            Log.Error(ex, ex.Message, ex.ToString);
        }
        return cmd;
    }

    internal string TCommand()
    {
        string source = $"Command {Alias} = new Command({Alias});\n";
        source += $"{Alias}.Description = {Description};\n";
        source += $"{Parent}.AddCommand({Alias});\n";
        return source;
    }
}