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
        // Bfev

        public static CommandInfo ConvertBfevToJson => new(
            "*", "Convert Bfev To Json", "Bfev", ".bfevfl .bfevtm", ("deleteSource", SettingsFactory["BfevDeleteSource"]!), ("formatJson", SettingsFactory["BfevFormatJson"]!)
        );

        public static CommandInfo ConvertJsonToBfev => new(
            "*", "Convert Json To Bfev", "Bfev", ".json", ("deleteSource", SettingsFactory["BfevDeleteSource"]!)
        );

        // 
        // Byml

        public static CommandInfo ConvertBymlToYaml => new(
            "*", "Convert Byml To Yaml", "Byml", "b", ("deleteSource", SettingsFactory["BymlDeleteSource"]!)
        );

        public static CommandInfo ConvertYamlToByml => new(
            "*", "Convert Yaml To Byml", "Byml", ".yml .yaml", ("deleteSource", SettingsFactory["BymlDeleteSource"]!)
        );

        // 
        // Sarc

        public static CommandInfo ExtractSarc => new (
            "*", "Extract Sarc", "Sarc", null, ("deleteSource", SettingsFactory["SarcDeleteSource"]!)
        );

        public static CommandInfo RepackSarc => new(
            "Folder", "Repack Sarc", "Sarc", null, ("deleteSource", SettingsFactory["SarcDeleteSource"]!)
        );

        public static CommandInfo RepackSarcNx => new(
            "Folder", "Repack Sarc NX", "Sarc", null, ("deleteSource", SettingsFactory["SarcDeleteSource"]!)
        );
    }
}
