#pragma warning disable CA1416 // Validate platform compatibility

using Microsoft.Win32;

namespace BotwRegistryToolkit.Models
{
    public static class RegistryModel
    {
        private const string BotwKey = "*\\shell\\botw";

        public static Func<bool, bool?>? GetRegistryInjector(string key)
        {
            CommandInfo? command = (typeof(CommandsModel).GetProperty(key)?.GetValue(null) as CommandInfo);
            if (command != null) {
                return (isEnable) => {
                    if (isEnable) {
                        string flags = command.Flags.Length > 0 ? "--" + string.Join(" --", command.Flags.Select(x => $"{x.Key}={x.Value}")) : "";
                        string? exts = command.FileExtensions != null ? string.Join(" OR ", command.FileExtensions.Split(' ').Select(x => $"System.FileExtension:={x}")) : null;
                        Inject(command.Folder, command.Name, exts, $"""
                            "{Config.DataFolder}\Runtime.exe" {key} "%1" {flags}
                            """);
                    }
                    else {
                        Delete(command.Folder, command.Name);
                    }

                    return null;
                };
            }
            else {
                return null;
            }
        }

        public static RegistryKey? Shell { get; set; }

        public static RegistryKey Setup()
        {
            using RegistryKey key = Registry.ClassesRoot.CreateSubKey(BotwKey);
            key.SetValue("MUIVerb", "Botw", RegistryValueKind.String);
            key.SetValue("SubCommands", "", RegistryValueKind.String);
            return key.CreateSubKey("shell");
        }

        public static void Inject(string folder, string commandName, string? exts, string command)
        {
            Shell ??= Setup();
            using RegistryKey folderKey = Shell.CreateSubKey(folder.ToLower().Replace(' ', '_'));
            folderKey.SetValue("MUIVerb", folder, RegistryValueKind.String);
            folderKey.SetValue("SubCommands", "", RegistryValueKind.String);

            using RegistryKey folderShell = folderKey.CreateSubKey("shell");
            using RegistryKey commandParentKey = folderShell.CreateSubKey(commandName.ToLower().Replace(' ', '_'));
            commandParentKey.SetValue("", commandName);
            if (exts != null) {
                commandParentKey.SetValue("AppliesTo", exts);
            }

            using RegistryKey commandKey = commandParentKey.CreateSubKey("command");
            commandKey.SetValue("", command);
        }

        public static void Delete(string folder, string commandName)
        {
            Shell ??= Setup();
            using RegistryKey? folderKey = Shell.CreateSubKey(folder.ToLower().Replace(' ', '_'));
            if (folderKey != null) {
                using RegistryKey folderShell = folderKey.CreateSubKey("shell");
                folderShell.DeleteSubKeyTree(commandName.ToLower().Replace(' ', '_'), false);

                if (folderKey.GetSubKeyNames().Length == 0) {
                    folderKey.Dispose();
                    Shell.DeleteSubKeyTree(folder.ToLower().Replace(' ', '_'));
                }
            }
        }
    }
}
