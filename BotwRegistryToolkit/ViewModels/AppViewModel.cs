using BotwRegistryToolkit.Views;

namespace BotwRegistryToolkit.ViewModels
{
    public class AppViewModel : ReactiveObject
    {
        public SettingsView SettingsView { get; set; } = new(true);
    }
}
