using System.Text.Json;

namespace AutomationExercise.Tests.Support;

/// <summary>
/// Reads appsettings.json once. Environment variables override the file, which is handy in CI:
///   BROWSER=firefox HEADLESS=true dotnet test          (Linux / macOS)
///   $env:BROWSER="firefox"; dotnet test                (Windows PowerShell)
/// </summary>
public static class ConfigReader
{
    private static readonly Settings FileSettings = LoadFile();

    public static string BaseUrl => Environment.GetEnvironmentVariable("BASE_URL") ?? FileSettings.BaseUrl;

    public static string Browser =>
        (Environment.GetEnvironmentVariable("BROWSER") ?? FileSettings.Browser).ToLowerInvariant();

    public static bool Headless =>
        bool.TryParse(Environment.GetEnvironmentVariable("HEADLESS"), out var headless)
            ? headless
            : FileSettings.Headless;

    public static int TimeoutSeconds => FileSettings.TimeoutSeconds;

    private static Settings LoadFile()
    {
        var path = Path.Combine(AppContext.BaseDirectory, "appsettings.json");
        if (!File.Exists(path))
        {
            throw new FileNotFoundException("appsettings.json was not copied to the output folder", path);
        }

        return JsonSerializer.Deserialize<Settings>(File.ReadAllText(path))
               ?? throw new InvalidOperationException("appsettings.json is empty or invalid");
    }

    /// <summary>The shape of appsettings.json.</summary>
    private sealed class Settings
    {
        public string BaseUrl { get; set; } = "https://automationexercise.com";
        public string Browser { get; set; } = "chrome";
        public bool Headless { get; set; }
        public int TimeoutSeconds { get; set; } = 15;
    }
}
