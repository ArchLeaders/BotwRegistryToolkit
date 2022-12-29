global using static BotwRegistryToolkit.Models.DescriptionModel;

namespace BotwRegistryToolkit.Models
{
    public class DescriptionModel
    {
        public const string GlobalSettings_SortByFileType = """
            Organize each file-type tool into sub-menus.
            Known issues: menu items three or more levels deep have an annoying white border (only noticable in dark theme)
            """;

        public const string GlobalSettings_PrefixWithFolderName = """
            Prefixes menu items with the parent folder name.
            Only works when 'Sort By File Type' is disabled.
            """;

        //
        // Aamp Tools

        public const string AampTools_ConvertAampToYaml = """
            Botw / Aamp / Convert Aamp to Yaml
            Attempts to converts a binary AAMP file to a YAML text file
            """;

        public const string AampTools_ConvertYamlToAamp = """
            Botw / Aamp / Convert Yaml to Aamp
            Attempts to converts the input file (YAML) to a binary AAMP
            """;

        public const string AampTools_DeleteSource = """
            Deletes the source file after conversion
            """;

        //
        // Bfev Tools

        public const string BfevTools_ConvertBfevToJson = """
            Botw / Bfev / Convert Bfev to Json
            Attempts to converts a binary BFEV file to a JSON text file
            """;

        public const string BfevTools_ConvertJsonToBfev = """
            Botw / Bfev / Convert Json to Bfev
            Attempts to converts a JSON text file to a binary BFEV file
            """;

        public const string BfevTools_DeleteSource = """
            Deletes the source file after conversion
            """;

        public const string BfevTools_FormatJson = """
            Format the converted JSON text files
            """;

        //
        // Byml Tools

        public const string BymlTools_ConvertBymlToYaml = """
            Botw / Byml / Convert Byml to Yaml
            Attempts to converts a binary BYML file to a YAML text file
            """;

        public const string BymlTools_ConvertYamlToByml = """
            Botw / Byml / Convert Yaml to Byml
            Attempts to converts the input file (YAML) to a binary BYML
            """;

        public const string BymlTools_DeleteSource = """
            Deletes the source file after conversion
            """;
    }
}
