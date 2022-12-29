global using static BotwRegistryToolkit.Models.DescriptionModel;

namespace BotwRegistryToolkit.Models
{
    public class DescriptionModel
    {
        public const string GlobalSettings_SortByFileType = """
            Organize each file-type tool into sub-menus.
            Known issues: menu items three or more levels deep have an annoying white border (only noticable in dark theme)
            """;

        //
        // Aamp Tools

        public const string AampTools_ConvertToYaml = """
            Botw / Aamp / Convert Aamp to Yaml
            Attempts to converts a binary AAMP file to a YAML text file
            """;

        public const string AampTools_ConvertToAamp = """
            Botw / Aamp / Convert Yaml to Aamp
            Attempts to converts the input file (YAML) to a binary AAMP
            """;

        public const string AampTools_DeleteSource = """
            Deletes the source file after conversion
            """;
    }
}
