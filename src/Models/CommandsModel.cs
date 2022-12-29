﻿namespace BotwRegistryToolkit.Models
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
    }
}
