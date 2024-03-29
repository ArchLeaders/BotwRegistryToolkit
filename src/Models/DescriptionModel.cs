﻿global using static BotwRegistryToolkit.Models.DescriptionModel;

namespace BotwRegistryToolkit.Models
{
    public class DescriptionModel
    {
        public const string GlobalSettings_SortByFileType = """
            Organize each file-type tool into sub-menus
            Known issues: menu items three or more levels deep have an annoying white border (only noticable in dark theme)
            """;

        public const string GlobalSettings_PrefixWithFolderName = """
            Prefixes menu items with the parent folder name
            Only works when 'Sort By File Type' is disabled
            """;

        //
        // Aamp Tools

        public const string AampTools_ConvertAampToYaml = """
            Botw / Aamp / Convert Aamp to Yaml
            Attempts to converts a binary AAMP file to a YAML text file
            """;

        public const string AampTools_ConvertYamlToAamp = """
            Botw / Aamp / Convert Yaml to Aamp
            Attempts to converts a YAML text file to a binary AAMP
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
            Attempts to converts a YAML text file to a binary BYML
            """;

        public const string BymlTools_DeleteSource = """
            Deletes the source file after conversion
            """;

        //
        // Sarc Tools

        public const string SarcTools_ExtractSarc = """
            Botw / Sarc / Convert Sarc to Yaml
            Attempts to extract a SARC archive to a folder named after the SARC file
            Note: if 'Delete Source' is enabled, the extension will be used when repacking (default is '.sarc')
            """;

        public const string SarcTools_RepackSarc = """
            Botw / Sarc / Repack Sarc
            Packs a folder into a SARC archive
            """;

        public const string SarcTools_RepackSarcNx = """
            Botw / Sarc / Repack Sarc
            Packs a folder into a switch (little endian) SARC archive
            """;

        public const string SarcTools_DeleteSource = """
            Deletes the source file or folder after extracting or repacking
            """;

        public const string SarcTools_ReferenceFile = """
            Looks for a file with the same name as the source folder and overwrites that file using it's extension
            This does not work with 'Delete Source' enabled
            """;

        //
        // Yaz0 Tools

        public const string Yaz0Tools_Yaz0Compress = """
            Botw / Yaz0 / Compress with Yaz0
            Compresses a file with the Yaz0 compression algorithm
            """;

        public const string Yaz0Tools_Yaz0Decompress = """
            Botw / Yaz0 / Decompress with Yaz0
            Decompresses a file compressed with the Yaz0 compression algorithm
            """;

        public const string Yaz0Tools_CompressionLevel = """
            Define the level at which to compress files
            (The higher the value, the slower the compression)
            """;
    }
}
