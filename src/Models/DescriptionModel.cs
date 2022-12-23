global using static BotwRegistryToolkit.Models.DescriptionModel;

namespace BotwRegistryToolkit.Models
{
    public class DescriptionModel
    {
        public const string AampTools_ConvertToAamp = """
            Botw / Aamp / Convert to Aamp
            Attempts to converts the input file (YAML) to a binary AAMP
            """;

        public const string AampTools_ConvertToYaml = """
            Botw / Aamp / Convert to Aamp
            Attempts to converts a binary AAMP file to a YAML text file
            """;

        public const string AampTools_DeleteSource = """
            Deletes the source file after conversion
            """;
    }
}
