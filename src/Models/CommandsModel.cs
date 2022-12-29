namespace BotwRegistryToolkit.Models
{
    public record CommandInfo(string Name, string Folder, string? FileExtensions, bool IsNewGroup, params (string Key, object Value)[] Flags);
    public class CommandsModel
    {
        // 
        // Aamp

        public static CommandInfo ConvertAampToYaml => new(
            "Convert Aamp To Yaml", "Aamp", null, false, ("deleteSource", SettingsFactory["AampDeleteSource"]!)
        );

        public static CommandInfo ConvertYamlToAamp => new(
            "Convert Yaml To Aamp", "Aamp", ".yml .yaml", false, ("deleteSource", SettingsFactory["AampDeleteSource"]!)
        );

        // 
        // Byml

        public static CommandInfo ConvertBymlToYaml => new(
            "Convert Byml To Yaml", "Byml", null, true, ("deleteSource", SettingsFactory["BymlDeleteSource"]!)
        );

        public static CommandInfo ConvertYamlToByml => new(
            "Convert Yaml To Byml", "Byml", ".yml .yaml", false, ("deleteSource", SettingsFactory["BymlDeleteSource"]!)
        );
    }
}
