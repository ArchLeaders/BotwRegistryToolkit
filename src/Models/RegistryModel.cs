using System.Reflection;

namespace BotwRegistryToolkit.Models
{
    public static class RegistryModel
    {
        public static Func<bool, bool?> GetRegistrySet(string key)
        {
            MethodInfo? func = typeof(RegistryModel).GetMethod(key, BindingFlags.Static | BindingFlags.Public);
            return (toggle) => (bool?)func?.Invoke(null, new object[] { toggle });
        }

        // 
        // Aamp Tools

        public static bool? ConvertYamlToAamp(bool toggle)
        {
            bool deleteSource = (bool)ViewModel.SettingsView["AampDeleteSource"]!;
            return null;
        }

        public static bool? ConvertAampToYaml(bool toggle)
        {
            bool deleteSource = (bool)ViewModel.SettingsView["AampDeleteSource"]!;
            return null;
        }
    }
}
