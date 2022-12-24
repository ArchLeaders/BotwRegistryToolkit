using Avalonia;
using Avalonia.Generics.Dialogs;
using Avalonia.SettingsFactory;
using Avalonia.SettingsFactory.Core;
using Avalonia.SettingsFactory.ViewModels;
using Avalonia.Themes.Fluent;
using BotwRegistryToolkit.Models;
using System.Reflection;

namespace BotwRegistryToolkit.Views
{
    public partial class SettingsView : SettingsFactory, ISettingsValidator
    {
        public SettingsView() => InitializeComponent();
        public SettingsView(bool canCancel = true)
        {
            InitializeComponent();

            AfterSaveEvent += async () => {
                Config.Save();
                DialogResult results = await MessageBox.ShowDialog("Exit the configuration tool?", "Notice", DialogButtons.YesNoCancel);
                if (results == DialogResult.Yes) {
                    Environment.Exit(0);
                }
            };

            AfterCancelEvent += async () => {
                DialogResult results = await MessageBox.ShowDialog("Are you sure you wish to exit without saving?", "Warning", DialogButtons.YesNoCancel);
                if (results == DialogResult.Yes) {
                    ValidateSave();
                    Environment.Exit(0);
                }
            };

            SettingsFactoryOptions options = new() {
                AlertAction = (msg) => MessageBox.ShowDialog(msg),
                BrowseAction = async (title) => await new BrowserDialog(BrowserMode.OpenFolder).ShowDialog(),
            };

            // Initialize the settings layout
            InitializeSettingsFactory(new SettingsFactoryViewModel(canCancel), this, Config, options);
        }

        public bool? ValidateBool(string key, bool value)
        {
            return key switch {
                _ => ValidateRegistryInjector(key, value),
            };
        }

        public bool? ValidateString(string key, string? value)
        {
            if (value == null) {
                return null;
            }

            return key switch {
                "Theme" => ValidateTheme(value),
                _ => null,
            };
        }

        public static bool? ValidateRegistryInjector(string key, bool value)
        {
            Func<bool, bool?> inject = RegistryModel.GetRegistrySet(key);
            return inject.Invoke(value);
        }

        public static bool? ValidateTheme(string value)
        {
            App.Theme.Mode = value == "Dark" ? FluentThemeMode.Dark : FluentThemeMode.Light;
            Application.Current!.Styles[0] = App.Theme;
            return null;
        }

        public string? ValidateSave()
        {
            Dictionary<string, bool?> validated = new();
            foreach (var prop in Config.GetType().GetProperties().Where(x => x.GetCustomAttributes<SettingAttribute>(false).Any())) {
                object? value = prop.GetValue(Config);

                if (value is bool boolean) {
                    validated.Add(prop.Name, ValidateBool(prop.Name, boolean));
                }
                else {
                    validated.Add(prop.Name, ValidateString(prop.Name, value as string));
                }
            }

            return ValidateSave(validated);
        }

        public string? ValidateSave(Dictionary<string, bool?> validated)
        {
            return null;
        }
    }
}
