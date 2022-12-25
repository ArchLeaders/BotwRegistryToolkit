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
                Shell ??= Setup();
                return (isEnable) => {
                    if (isEnable) {
                        string flags = command.Flags.Length > 0 ? "--" + string.Join(" --", command.Flags.Select(x => $"{x.Key}={x.Value}")) : "";
                        string? exts = command.FileExtensions != null ? string.Join(" OR ", command.FileExtensions.Split(' ').Select(x => $"System.FileExtension:={x}")) : null;
                        Inject(command.Folder, command.Name, exts, command.IsLast, $"""
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

        public static RegistryKey Shell { get; set; } = null!;
        public static RegistryKey Setup()
        {
            using RegistryKey key = Registry.ClassesRoot.CreateSubKey(BotwKey);
            key.SetValue("MUIVerb", "Botw", RegistryValueKind.String);
            key.SetValue("SubCommands", "", RegistryValueKind.String);
            return key.CreateSubKey("shell");
        }

        public static void Inject(string folderName, string commandName, string? exts, bool isLast, string command)
        {
            // Hard reset to avoid duplicates
            Delete(folderName, commandName);

            string safeFolderName = folderName.ToLower().Replace(' ', '_');
            string safeCommandName = commandName.ToLower().Replace(' ', '_');

            if ((bool)SettingsFactory["SortByFileType"]! == true) {
                using RegistryKey folder = Shell.CreateSubKey(safeFolderName);
                folder.SetValue("MUIVerb", folderName, RegistryValueKind.String);
                folder.SetValue("SubCommands", "", RegistryValueKind.String);

                using RegistryKey folderShell = folder.CreateSubKey("shell");
                using RegistryKey commandParent = folderShell.CreateSubKey(safeCommandName);
                commandParent.SetValue("", commandName);
                if (exts != null) {
                    commandParent.SetValue("AppliesTo", exts);
                }

                using RegistryKey commandKey = commandParent.CreateSubKey("command");
                commandKey.SetValue("", command);
            }
            else {
                using RegistryKey commandParent = Shell.CreateSubKey(safeCommandName);
                commandParent.SetValue("", commandName);
                if (exts != null) {
                    commandParent.SetValue("AppliesTo", exts);
                }

                if (isLast) {
                    commandParent.SetValue("CommandFlags", 0x40, RegistryValueKind.DWord);
                }

                using RegistryKey commandKey = commandParent.CreateSubKey("command");
                commandKey.SetValue("", command);
            }
        }

        public static void Delete(string folderName, string commandName)
        {
            string[] subKeys = Shell.GetSubKeyNames();

            string safeCommandName = commandName.ToLower().Replace(' ', '_');
            if (subKeys.Contains(safeCommandName)) {
                Shell.DeleteSubKeyTree(safeCommandName);
            }

            string safeFolderName = folderName.ToLower().Replace(' ', '_');
            if (subKeys.Contains(safeFolderName)) {
                using RegistryKey? folder = Shell.OpenSubKey(safeFolderName, true);
                if (folder != null) {
                    using RegistryKey? folderShell = folder.OpenSubKey("shell", true);
                    folderShell?.DeleteSubKeyTree(safeCommandName, false);

                    if (folderShell?.GetSubKeyNames().Length == 0) {
                        folder.Dispose();
                        Shell.DeleteSubKeyTree(safeFolderName);
                    }
                }
            }
        }
    }
}
