namespace BotwRegistryToolkit.Models
{
    public record CommandInfo(string Class, string Name, string Folder, string? FileExtensions, params (string Key, object Value)[] Flags);
    public class CommandsModel
    {
        // 
        // Aamp

        public static CommandInfo ConvertAampToYaml => new(
            "*", "Convert Aamp To Yaml", "Aamp", "b", ("deleteSource", SettingsFactory["AampDeleteSource"]!)
        );

        public static CommandInfo ConvertYamlToAamp => new(
            "*", "Convert Yaml To Aamp", "Aamp", ".yml .yaml", ("deleteSource", SettingsFactory["AampDeleteSource"]!)
        );

        // 
        // Byml

        public static CommandInfo ConvertBymlToYaml => new(
            "*", "Convert Byml To Yaml", "Byml", "b", ("deleteSource", SettingsFactory["BymlDeleteSource"]!)
        );

        public static CommandInfo ConvertYamlToByml => new(
            "*", "Convert Yaml To Byml", "Byml", ".yml .yaml", ("deleteSource", SettingsFactory["BymlDeleteSource"]!)
        );
    }
}
