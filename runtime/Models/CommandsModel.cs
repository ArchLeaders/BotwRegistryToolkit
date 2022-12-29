using Nintendo.Aamp;
using Nintendo.Byml;
using SharpYaml;
using System.Reflection;

namespace BotwRegistryToolkit.Runtime.Models
{
    public class CommandsModel
    {
        public static IEnumerable<Action> GetCommands(string[] args)
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

            // Yield return the invoked command
            IEnumerable<string> targets = args[1..].Where(x => !x.StartsWith("--") && (File.Exists(x) || Directory.Exists(x))).Distinct();
            if (!targets.Any()) {
                throw new Exception($"No valid targets were found for the command '{args[0]}'");
            }

            foreach (string target in targets) {
                parameters[0] = target;
                yield return () => funcInfo.Invoke(null, parameters);
            }
        }

        public static void ConvertAampToYaml(string file, bool deleteSource)
        {
            Yaz0Helper.IsYaz0(file, out byte[] data);
            AampFile aamp = new(data);
            if (deleteSource == true) File.Delete(file);
            File.WriteAllText($"{file}.yml", aamp.ToYml());
        }

        public static void ConvertYamlToAamp(string file, bool deleteSource)
        {
            try {
                AampFile aamp = AampFile.FromYmlFile(file);
                if (deleteSource == true) File.Delete(file);
                File.WriteAllBytes($"{Path.GetDirectoryName(file)}\\{Path.GetFileNameWithoutExtension(file)}", aamp.ToBinary());
            }
            catch (SyntaxErrorException ex) {
                throw new Exception("Invalid YAML file", ex);
            }
        }

        public static void ConvertBymlToYaml(string file, bool deleteSource)
        {
            Yaz0Helper.IsYaz0(file, out byte[] data);
            BymlFile byml = BymlFile.FromBinary(data);
            if (deleteSource == true) File.Delete(file);
            File.WriteAllText($"{file}.yml", byml.ToYaml());
        }

        public static void ConvertYamlToByml(string file, bool deleteSource)
        {
            try {
                BymlFile byml = BymlFile.FromYamlFile(file);
                if (deleteSource == true) File.Delete(file);
                File.WriteAllBytes($"{Path.GetDirectoryName(file)}\\{Path.GetFileNameWithoutExtension(file)}", byml.ToBinary());
            }
            catch (SyntaxErrorException ex) {
                throw new Exception("Invalid YAML file", ex);
            }
        }
    }
}
