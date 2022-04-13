namespace autocli.Interface;

/// <summary>
/// Properties class to serialize the properties of the application.
/// </summary>
public class IProperty
{
    [JsonProperty("Name")]
    internal string? Name { get; set; }

    [JsonProperty("Title")]
    internal string Title { get; set; }

    [JsonProperty("Description")]
    internal string Description { get; set; }

    [JsonProperty("OutputPath")]
    internal string OutputPath { get; set; }

    [JsonProperty("Repo")]
    internal string Repo { get; set; }

    /// <summary>
    /// Constructs a new instance of the RootCommand class.
    /// </summary>
    /// <returns>Corresponding RootCommand.</returns>
    internal RootCommand BuildRoot()
    {
        var watch = System.Diagnostics.Stopwatch.StartNew();
        RootCommand root = new(Functionnals.Utils.Boxed(Title) + Description + "\n");
        root.SetHandler(() => root.InvokeAsync("-h"));
        watch.Stop();
        Log.Debug("RootCommand built: {t} ms", watch.ElapsedMilliseconds);
        return root;
    }

    internal string TRootCommand()
    {
        string source = $@"RootCommand root = new RootCommand({Functionnals.Utils.Boxed(Title)} + {Description} + ""\n"");" + "\n";
        source += @"root.SetHandler(() => root.InvokeAsync("" - h""));" + "\n";
        return source;
    }
}