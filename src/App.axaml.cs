#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.

global using static BotwRegistryToolkit.App;

using Avalonia;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Generics;
using Avalonia.Generics.Builders;
using Avalonia.Generics.Controls;
using Avalonia.Markup.Xaml;
using Avalonia.Themes.Fluent;
using BotwRegistryToolkit.ViewModels;
using BotwRegistryToolkit.Views;

namespace BotwRegistryToolkit
{
    public partial class App : Application
    {
        public static SettingsView SettingsFactory { get; set; }
        public static FluentTheme Theme { get; set; } = new(new Uri("avares://BotwRegistryToolkit/Styles"));

        public override void Initialize() => AvaloniaXamlLoader.Load(this);
        public override void OnFrameworkInitializationCompleted()
        {
            // Load the user config
            Config.Load();

            Theme.Mode = Config.Theme == "Dark" ? FluentThemeMode.Dark : FluentThemeMode.Light;
            Current!.Styles[0] = Theme;

            if (ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop) {

                // Get SettingsFactory (SettingsView)
                AppViewModel viewModel = new();
                SettingsFactory = viewModel.SettingsView;

                GenericWindow mainWindow = WindowBuilder.Initialize(new AppView())
                    .WithWindowColors("SystemChromeLowColor", "SystemChromeHighColor", 0.4)
                    .Build();

#if DEBUG
                mainWindow.AttachDevTools();
#endif
                desktop.MainWindow = mainWindow;
                ApplicationLoader.Attach(this);
            }

            base.OnFrameworkInitializationCompleted();
        }
    }
}
