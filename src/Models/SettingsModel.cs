global using static BotwRegistryToolkit.Models.SettingsModel;
using Avalonia.Generics.Dialogs;
using Avalonia.SettingsFactory.Core;
using System.Runtime.InteropServices;
using System.Text.Json;
using System.Text.Json.Serialization;
using static System.Environment;

namespace BotwRegistryToolkit.Models
{
    public class SettingsModel : ISettingsBase
    {
        public static SettingsModel Config { get; set; } = new();
        public bool RequiresInput { get; set; } = true;

        [JsonIgnore]
        public string DataFolder
            => RuntimeInformation.IsOSPlatform(OSPlatform.Windows) ? $"{GetFolderPath(SpecialFolder.LocalApplicationData)}\\{nameof(BotwRegistryToolkit)}" : $"{GetFolderPath(SpecialFolder.ApplicationData)}/{nameof(BotwRegistryToolkit)}";

        public async void Load()
        {
            if (File.Exists(Path.Combine(DataFolder, "Config.json"))) {
                using FileStream fs = File.OpenRead(Path.Combine(DataFolder, "Config.json"));
                Config = JsonSerializer.Deserialize<SettingsModel>(fs) ?? (SettingsModel)Save();
            }
            else {
                Config = (SettingsModel)Save();
            }

            // Collect runtime
            if (File.Exists(".\\Runtime.exe")) {
                File.Move(".\\Runtime.exe", $"{DataFolder}\\Runtime.exe", true);
            }
            else {
                try {
                    using HttpClient client = new();
                    client.DefaultRequestHeaders.Add("user-agent", $"{nameof(BotwRegistryToolkit).ToLower()}");

                    string request = $"https://api.github.com/repos/ArchLeaders/{nameof(BotwRegistryToolkit)}/releases";
                    List<Dictionary<string, JsonElement>> releases = JsonSerializer.Deserialize<List<Dictionary<string, JsonElement>>>(
                        await client.GetStringAsync(request)) ?? throw new Exception($"Could not parse GitHub release info from '{request}'");

                    string download = releases[0]["assets"][0].Deserialize<Dictionary<string, JsonElement>>()?["browser_download_url"].ToString()
                        ?? throw new Exception($"Invalid download url from '{request}# [0] > assets > [0]'");

                    File.WriteAllBytes($"{DataFolder}\\Runtime.exe", await client.GetByteArrayAsync(download));
                }
                catch {
                    await MessageBox.ShowDialog("""
                        Failed to retrive the runtime from the server, please download it manually and place it next to the main executable.

                        [https://github.com/ArchLeaders/BotwRegistryToolkit](https://github.com/ArchLeaders/BotwRegistryToolkit/releases/latest)
                        """, "Error", formatting: Formatting.Markdown);
                    Exit(1);
                }
            }
        }

        public ISettingsBase Save()
        {
            Directory.CreateDirectory(DataFolder);
            using (FileStream fs = File.Create(Path.Combine(DataFolder, "Config.json"))) {
                JsonSerializer.Serialize(fs, Config);
            }

            return this;
        }


        [Setting("Sort by File Type", GlobalSettings_SortByFileType, Category = "Global Settings", Folder = "Registry Tools")]
        public bool SortByFileType { get; set; } = true;

        [Setting("Prefix with Folder Name", GlobalSettings_PrefixWithFolderName, Category = "Global Settings", Folder = "Registry Tools")]
        public bool PrefixWithFolderName { get; set; } = false;

        //
        // Aamp Tools

        [Setting("Convert To Yaml", AampTools_ConvertAampToYaml, Category = "Aamp Tools", Folder = "Registry Tools")]
        public bool ConvertAampToYaml { get; set; } = true;

        [Setting("Convert To Aamp", AampTools_ConvertYamlToAamp, Category = "Aamp Tools", Folder = "Registry Tools")]
        public bool ConvertYamlToAamp { get; set; } = true;

        [Setting("Delete Source", AampTools_DeleteSource, Category = "Aamp Tools", Folder = "Registry Tools")]
        public bool AampDeleteSource { get; set; } = false;

        //
        // Bfev Tools

        [Setting("Convert To Json", BfevTools_ConvertBfevToJson, Category = "Bfev Tools", Folder = "Registry Tools")]
        public bool ConvertBfevToJson { get; set; } = true;

        [Setting("Convert To Bfev", BfevTools_ConvertJsonToBfev, Category = "Bfev Tools", Folder = "Registry Tools")]
        public bool ConvertJsonToBfev { get; set; } = true;

        [Setting("Delete Source", BfevTools_DeleteSource, Category = "Bfev Tools", Folder = "Registry Tools")]
        public bool BfevDeleteSource { get; set; } = false;

        [Setting("Format Json", BfevTools_FormatJson, Category = "Bfev Tools", Folder = "Registry Tools")]
        public bool BfevFormatJson { get; set; } = true;

        //
        // Byml Tools

        [Setting("Convert To Yaml", BymlTools_ConvertBymlToYaml, Category = "Byml Tools", Folder = "Registry Tools")]
        public bool ConvertBymlToYaml { get; set; } = true;

        [Setting("Convert To Byml", BymlTools_ConvertYamlToByml, Category = "Byml Tools", Folder = "Registry Tools")]
        public bool ConvertYamlToByml { get; set; } = true;

        [Setting("Delete Source", BymlTools_DeleteSource, Category = "Byml Tools", Folder = "Registry Tools")]
        public bool BymlDeleteSource { get; set; } = false;

        //
        // Sarc Tools

        [Setting("Extract Sarc", SarcTools_ExtractSarc, Category = "Sarc Tools", Folder = "Registry Tools")]
        public bool ExtractSarc { get; set; } = true;

        [Setting("Repack Sarc", SarcTools_RepackSarc, Category = "Sarc Tools", Folder = "Registry Tools")]
        public bool RepackSarc { get; set; } = true;

        [Setting("Repack Sarc NX", SarcTools_RepackSarcNx, Category = "Sarc Tools", Folder = "Registry Tools")]
        public bool RepackSarcNx { get; set; } = true;

        [Setting("Delete Source", SarcTools_DeleteSource, Category = "Sarc Tools", Folder = "Registry Tools")]
        public bool SarcDeleteSource { get; set; } = false;

        //
        // Yaz0 Tools

        [Setting("Compress With Yaz0", Yaz0Tools_Yaz0Compress, Category = "Yaz0 Tools", Folder = "Registry Tools")]
        public bool Yaz0Compress { get; set; } = true;

        [Setting("Decompress With Yaz0", Yaz0Tools_Yaz0Decompress, Category = "Yaz0 Tools", Folder = "Registry Tools")]
        public bool Yaz0Decompress { get; set; } = true;

        [Setting(UiType.Dropdown, "1", "2", "3", "4", "5", "6", "7", "8", "9", Name = "Compression Level", Description = Yaz0Tools_CompressionLevel, Category = "Yaz0 Tools", Folder = "Registry Tools")]
        public string Yaz0CompressionLevel { get; set; } = "7";

        // 
        // App Settings

        [Setting(UiType.Dropdown, "Dark", "Light", Category = "Appearance")]
        public string Theme { get; set; } = "Dark";
    }
}
