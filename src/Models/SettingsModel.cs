#pragma warning disable CA1822 // Mark members as static

global using static BotwRegistryToolkit.Models.SettingsModel;

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
            => RuntimeInformation.IsOSPlatform(OSPlatform.Windows) ? $"{GetFolderPath(SpecialFolder.LocalApplicationData)}/{nameof(BotwRegistryToolkit)}" : $"{GetFolderPath(SpecialFolder.ApplicationData)}/{nameof(BotwRegistryToolkit)}";

        public void Load()
        {
            if (File.Exists(Path.Combine(DataFolder, "Config.json"))) {
                using FileStream fs = File.OpenRead(Path.Combine(DataFolder, "Config.json"));
                Config = JsonSerializer.Deserialize<SettingsModel>(fs) ?? (SettingsModel)Save();
            }
            else {
                Config = (SettingsModel)Save();
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

        //
        // Aamp Tools

        [Setting("Convert To Aamp", AampTools_ConvertToAamp, Category = "Aamp Tools", Folder = "Registry Tools")]
        public bool ConvertYamlToAamp { get; set; } = true;

        [Setting("Convert To Yaml", AampTools_ConvertToAamp, Category = "Aamp Tools", Folder = "Registry Tools")]
        public bool ConvertAampToYaml { get; set; } = true;

        [Setting("Delete Source", AampTools_DeleteSource, Category = "Aamp Tools", Folder = "Registry Tools")]
        public bool AampDeleteSource { get; set; } = false;

        // 
        // App Settings

        [Setting(UiType.Dropdown, "Dark", "Light", Category = "Appearance")]
        public string Theme { get; set; } = "Dark";
    }
}
