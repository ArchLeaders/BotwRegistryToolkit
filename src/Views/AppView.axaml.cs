using Avalonia.Controls;
using BotwRegistryToolkit.ViewModels;

namespace BotwRegistryToolkit.Views
{
    public partial class AppView : Window
    {
        public AppView() => InitializeComponent();
        public AppView(AppViewModel context)
        {
            InitializeComponent();
            DataContext = context;
        }
    }
}
