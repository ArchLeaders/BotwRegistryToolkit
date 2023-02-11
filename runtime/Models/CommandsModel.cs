using BfevLibrary;
using Nintendo.Aamp;
using Nintendo.Byml;
using SarcLibrary;
using SharpYaml;
using System.Reflection;
using Yaz0Library;

namespace BotwRegistryToolkit.Runtime.Models
{
    public class CommandsModel
    {
        public static Task GetCommand(string[] args)
        {
            // Collect flags
            Dictionary<string, object> flags = args[1..]
                .Select(x => x.Split('='))
                .Where(x => x.Length == 2 && x[0].StartsWith("--"))
                .ToDictionary(x => x[0].Remove(0, 2), x => (object)x[1]);

            // Query the specified command
            MethodInfo funcInfo = typeof(CommandsModel).GetMethod(args[0]) ?? throw new Exception($"Could not find the command '{args[0]}'");

            // Check if the flags satisfy the command args
            ParameterInfo[] parameterInfo = funcInfo.GetParameters();
            object?[] parameters = new object?[parameterInfo.Length];
            for (int i = 1; i < parameterInfo.Length; i++) {
                bool hasParam = flags.TryGetValue(parameterInfo[i].Name!, out parameters[i]);
                if (!hasParam) {
                    throw new Exception($"Could not find a matching flag to the command argument '{parameterInfo[i].Name}'");
                }

                // Parse type
                Type type = parameterInfo[i].ParameterType;
                if (type == typeof(bool)) {
                    parameters[i] = bool.Parse(parameters[i]!.ToString() ?? "");
                }
                else if (type == typeof(int)) {
                    parameters[i] = int.Parse(parameters[i]!.ToString() ?? "");
                }
                else if (type == typeof(float)) {
                    parameters[i] = float.Parse(parameters[i]!.ToString() ?? "");
                }
                else if (type == typeof(decimal)) {
                    parameters[i] = decimal.Parse(parameters[i]!.ToString() ?? "");
                }
            }

            // Return the invoked command
            string? target = args[1..].Where(x => !x.StartsWith("--") && (File.Exists(x) || Directory.Exists(x))).Distinct().FirstOrDefault();
            parameters[0] = target
                ?? throw new ArgumentException($"No valid target was found for the command '{args[0]}'");

            return funcInfo.Invoke(null, parameters) as Task
                ?? throw new NullReferenceException($"The specified command '{args[0]}' returned null");
        }

        //
        // Aamp Tools

        public static Task ConvertAampToYaml(string file, bool deleteSource)
        {
            AampFile aamp = new(file);
            if (deleteSource == true) File.Delete(file);
            return File.WriteAllTextAsync($"{file}.yml", aamp.ToYml());
        }

        public static Task ConvertYamlToAamp(string file, bool deleteSource)
        {
            try {
                AampFile aamp = AampFile.FromYmlFile(file);
                if (deleteSource == true) File.Delete(file);
                File.WriteAllBytes($"{Path.GetDirectoryName(file)}\\{Path.GetFileNameWithoutExtension(file)}", aamp.ToBinary());
            }
            catch (SyntaxErrorException ex) {
                throw new Exception("Invalid YAML file", ex);
            }

            return Task.CompletedTask;
        }

        // 
        // Bfev Tools

        public static Task ConvertBfevToJson(string file, bool deleteSource, bool formatJson)
        {
            BfevFile bfev = BfevFile.FromBinary(file);
            if (deleteSource == true) File.Delete(file);
            return File.WriteAllTextAsync($"{file}.json", bfev.ToJson(formatJson));
        }

        public static Task ConvertJsonToBfev(string file, bool deleteSource)
        {
            BfevFile bfev = BfevFile.FromJson(File.ReadAllText(file));
            if (deleteSource == true) File.Delete(file);
            return File.WriteAllBytesAsync($"{Path.GetDirectoryName(file)}\\{Path.GetFileNameWithoutExtension(file)}", bfev.ToBinary());
        }

        //
        // Byml Tools

        public static Task ConvertBymlToYaml(string file, bool deleteSource)
        {
            BymlFile byml = BymlFile.FromBinary(file);
            if (deleteSource == true) File.Delete(file);
            return File.WriteAllTextAsync($"{file}.yml", byml.ToYaml());
        }

        public static Task ConvertYamlToByml(string file, bool deleteSource)
        {
            try {
                BymlFile byml = BymlFile.FromYamlFile(file);
                if (deleteSource == true) File.Delete(file);
                File.WriteAllBytes($"{Path.GetDirectoryName(file)}\\{Path.GetFileNameWithoutExtension(file)}", byml.ToBinary());
            }
            catch (SyntaxErrorException ex) {
                throw new Exception("Invalid YAML file", ex);
            }

            return Task.CompletedTask;
        }

        //
        // Sarc Tools

        public static async Task ExtractSarc(string file, bool deleteSource)
        {
            Yaz0Helper.IsYaz0(file, out byte[] data);
            SarcFile sarc = SarcFile.FromBinary(data);
            if (deleteSource == true) File.Delete(file);
            await sarc.ExtractToDirectory(deleteSource ? file : string.Join('.', file.Split('.')[..^1]));
        }


        public static Task RepackSarc(string folder, bool deleteSource, bool referenceFile) => RepackSarcHelper(folder, deleteSource, referenceFile, Endian.Big);
        public static Task RepackSarcNx(string folder, bool deleteSource, bool referenceFile) => RepackSarcHelper(folder, deleteSource, referenceFile, Endian.Little);
        public static Task RepackSarcHelper(string folder, bool deleteSource, bool referenceFile, Endian endian)
        {
            SarcFile sarc = SarcFile.LoadFromDirectory(folder);
            sarc.Endian = endian;

            if (deleteSource == true) {
                Directory.Delete(folder, true);
            }

            string path = deleteSource && Path.GetFileName(folder).Contains('.') ? folder : $"{folder}.sarc";
            if (referenceFile == true) {
                IEnumerable<string> matches = Directory.GetFiles(Path.GetDirectoryName(folder) ?? "").Where(x => Path.GetFileNameWithoutExtension(x) == Path.GetFileNameWithoutExtension(folder));
                if (matches.Any()) {
                    path = matches.First();
                    File.Delete(path);
                }
            }

            byte[] data = sarc.ToBinary();
            if (Path.GetExtension(path).StartsWith(".s")) {
                data = Yaz0.Compress(data, out Yaz0SafeHandle _).ToArray();
            }

            return File.WriteAllBytesAsync(path, data);
        }

        //
        // Yaz0 Tools

        public static Task Yaz0Compress(string file, string compressionLevel)
        {
            return File.WriteAllBytesAsync($"{string.Join('.', file.Split('.')[..^1])}.s{Path.GetExtension(file).Remove(0, 1)}", Yaz0.Compress(file, out Yaz0SafeHandle _, int.Parse(compressionLevel)).ToArray());
        }

        public static Task Yaz0Decompress(string file)
        {
            string ext = Path.GetExtension(file);
            return File.WriteAllBytesAsync($"{string.Join('.', file.Split('.')[..^1])}{(ext.ToLower() != ".ssarc" ? ext.Replace(".s", ".") : ext)}", Yaz0.Decompress(file).ToArray());
        }
    }
}
