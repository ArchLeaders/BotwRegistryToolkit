using System.Reflection;

namespace BotwRegistryToolkit.Models
{
    public class ResourceModel
    {
        public static string? ExtractIcon(string name)
        {
            string export = $"{Config.DataFolder}\\{name}.ico";

            if (File.Exists(export)) {
                return export;
            }

            Stream? res = Assembly.GetCallingAssembly().GetManifestResourceStream($"{nameof(BotwRegistryToolkit)}.Assets.{name.ToLower()}.ico");
            if (res != null) {
                using FileStream fs = File.Create(export);
                Span<byte> buffer = new byte[res.Length];
                res?.Read(buffer);
                fs.Write(buffer);
                return export;
            }

            return null;
        }
    }
}
