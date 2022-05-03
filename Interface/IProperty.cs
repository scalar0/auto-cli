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
    internal string? Description { get; set; }

    [JsonProperty("OutputPath")]
    internal string OutputPath { get; set; } = null!;

    [JsonProperty("Framework")]
    internal string Framework { get; set; } = "net6.0";

    [JsonProperty("Repo")]
    internal string Repo { get; set; } = null!;

    [JsonProperty("APIs")]
    internal List<Dictionary<string, string>>? APIs { get; set; }

    #endregion Properties

    /// <summary>
    /// App description customization.
    /// </summary>
    internal string UsageOutput()
    {
        StringBuilder box = new();
        StringBuilder line = new();
        int c = 0;
        do
        {
            line.Append("=");
            c++;
        }
        while (c < Console.WindowWidth - 2);
        for (int i = 0; i < (Console.WindowWidth - Title.Length) / 2 - 1; i++)
        {
            box.Append(" ");
        }
        box.Append(Title);
        box.AppendLine();
        box.AppendLine(Description);
        box.AppendLine($"Target framework: .{Framework}");
        box.AppendLine($"Repository: {Repo}");
        if (APIs is not null)
        {
            box.AppendLine("APIs dependencies:");
            foreach (var api in APIs)
            {
                box.AppendLine($"\t{api["Name"]} v{api["Version"]}");
            }
        }
        box.AppendLine(line.ToString());
        box.AppendLine($"Host name: {Environment.MachineName}");
        box.AppendLine($"OS: {Environment.OSVersion}");
        box.AppendLine($"Host version: .net{Environment.Version}");
        return box.ToString();
    }

    /// <summary>
    /// Constructs a new instance of the RootCommand class.
    /// </summary>
    internal RootCommand BuildRoot()
    {
        RootCommand root = new(UsageOutput());
        root.SetHandler(() => root.InvokeAsync("-h"));

        Log.Debug("RootCommand built.");
        return root;
    }

    internal string TRootCommand()
    {
        StringBuilder source = new();
        source.AppendLine();
        source.AppendLine($@"RootCommand root = new(@""{UsageOutput()}"");");
        source.AppendLine(@"root.SetHandler(() => root.InvokeAsync("" -h""));");
        return source.ToString();
    }
}