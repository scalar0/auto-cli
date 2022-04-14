using Newtonsoft.Json;

namespace autocli.Interface;

/// <summary>
/// Properties class to serialize the properties of the application.
/// </summary>
public class IProperty
{
    #region Properties

    [JsonProperty("Name")]
    internal string? Name { get; set; }

    [JsonProperty("Title")]
    internal string Title { get; set; } = null!;

    [JsonProperty("Description")]
    internal string Description { get; set; } = null!;

    [JsonProperty("OutputPath")]
    internal string OutputPath { get; set; } = null!;

    [JsonProperty("Repo")]
    internal string Repo { get; set; } = null!;

    #endregion Properties

    /// <summary>
    /// Constructs a new instance of the RootCommand class.
    /// </summary>
    /// <returns>Corresponding RootCommand.</returns>
    internal RootCommand BuildRoot()
    {
        RootCommand root = new(Functionnals.Utils.Boxed(Title) + Description + "\n");
        root.SetHandler(() => root.InvokeAsync("-h"));
        Log.Debug("RootCommand built.");
        return root;
    }

    internal string TRootCommand()
    {
        string source = "\n" + $@"RootCommand root = new(@""{Functionnals.Utils.Boxed(Title)}"" + @""{Description}"" + ""\n"");";
        source += "\n" + @"root.SetHandler(() => root.InvokeAsync("" -h""));" + "\n";
        return source;
    }
}