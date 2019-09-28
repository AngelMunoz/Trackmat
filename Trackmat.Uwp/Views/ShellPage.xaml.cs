using Caliburn.Micro;

using Trackmat.Uwp.ViewModels;

using Windows.UI.Xaml.Controls;

using WinUI = Microsoft.UI.Xaml.Controls;

namespace Trackmat.Uwp.Views
{
    public sealed partial class ShellPage : IShellView
    {
        private ShellViewModel ViewModel => DataContext as ShellViewModel;

        public ShellPage()
        {
            InitializeComponent();
        }

        public INavigationService CreateNavigationService(WinRTContainer container)
        {
            var navigationService = container.RegisterNavigationService(shellFrame);
            return navigationService;
        }

        public WinUI.NavigationView GetNavigationView()
        {
            return navigationView;
        }

        public Frame GetFrame()
        {
            return shellFrame;
        }
    }
}
