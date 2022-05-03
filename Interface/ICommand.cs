using Newtonsoft.Json;

namespace autocli.Interface;

/// <summary>
/// Interface Command class to deserialize the commands of the interface.
/// </summary>
internal class ICommand
{
    #region Properties

    [JsonProperty("Alias")]
    internal string Alias { get; set; } = null!;

    [JsonProperty("Parent")]
    internal string Parent { get; set; } = null!;

    [JsonProperty("Description")]
    internal string Description { get; set; } = null!;

    #endregion Properties

    /// <summary>
    /// Constructs a new instance of the Command class.
    /// </summary>
    /// <param name="parent">Parent Command.</param>
    /// <returns>Corresponding Command.</returns>
    internal Command BuildCommand(Command parent)
    {
        Command cmd = new(Alias);
        cmd.Description = Description;
        parent.AddCommand(cmd);
        Log.Verbose("{C} built and added to {U}.", $"{cmd}", $"{parent}");
        return cmd;
    }

    internal string TCommand()
    {
        StringBuilder source = new();
        source.AppendLine("\n" + @$"Command {Alias} = new(""{Alias}"");");
        source.AppendLine(@$"{Alias}.Description = ""{Description}"";");
        source.AppendLine($"{Parent}.AddCommand({Alias});");
        return source.ToString();
    }
}