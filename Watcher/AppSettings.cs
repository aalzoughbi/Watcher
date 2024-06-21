using System;
using System.IO;
using System.Text.Json;

public class AppSettings
{
    public string SourceFolder { get; set; }
    public string ProcessedFolder { get; set; }
    public string EmailFrom { get; set; }
    public string EmailTo { get; set; }
    public string SmtpServer { get; set; }
    public int SmtpPort { get; set; }
    public string SmtpUser { get; set; }
    public string SmtpPass { get; set; }
    public bool UseSsl { get; set; }

    private static readonly string SettingsDirectory = @"C:\Scripts";
    public static readonly string SettingsFilePath = Path.Combine(SettingsDirectory, "appsettings.json");

    public static AppSettings Load()
    {
        EnsureDirectoryExists();
        Console.WriteLine($"Loading settings from: {SettingsFilePath}");
        if (File.Exists(SettingsFilePath))
        {
            string json = File.ReadAllText(SettingsFilePath);
            Console.WriteLine("Settings loaded successfully.");
            return JsonSerializer.Deserialize<AppSettings>(json);
        }

        Console.WriteLine("Settings file not found, using default settings.");
        return new AppSettings();
    }

    public void Save()
    {
        EnsureDirectoryExists();
        Console.WriteLine($"Saving settings to: {SettingsFilePath}");
        string json = JsonSerializer.Serialize(this, new JsonSerializerOptions { WriteIndented = true });
        File.WriteAllText(SettingsFilePath, json);
        Console.WriteLine("Settings saved successfully.");
    }

    private static void EnsureDirectoryExists()
    {
        if (!Directory.Exists(SettingsDirectory))
        {
            Directory.CreateDirectory(SettingsDirectory);
        }
    }
}
