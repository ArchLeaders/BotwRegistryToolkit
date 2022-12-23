#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.

using Avalonia;
using Avalonia.Controls.ApplicationLifetimes;
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
        public static AppView View { get; set; }
        public static AppViewModel ViewModel { get; set; }
        public static FluentTheme Theme { get; set; } = new(new Uri("avares://BotwActorTool/Styles"));

        public override void Initialize() => AvaloniaXamlLoader.Load(this);
        public override void OnFrameworkInitializationCompleted()
        {
            // Load the user config
            Config.Load();

            Theme.Mode = Config.Theme == "Dark" ? FluentThemeMode.Dark : FluentThemeMode.Light;
            Current!.Styles[0] = Theme;

            if (ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop) {

                // Create default view
                View = new();
                ViewModel = new();
                View.DataContext = ViewModel;

                GenericWindow mainWindow = WindowBuilder.Initialize(View)
                    .WithWindowColors("SystemChromeLowColor", "SystemChromeMediumColor")
                    .Build();

#if DEBUG
                mainWindow.AttachDevTools();
#endif
                desktop.MainWindow = mainWindow;
            }

            base.OnFrameworkInitializationCompleted();
        }
    }
}
