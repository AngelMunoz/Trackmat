using System;

using Trackmat.Uwp.ViewModels;

using Windows.UI.Xaml.Controls;

namespace Trackmat.Uwp.Views
{
    public sealed partial class SettingsPage : Page
    {
        public SettingsPage()
        {
            InitializeComponent();
            Loaded += SettingsPage_Loaded;
            Unloaded -= SettingsPage_Loaded;
        }

        private void SettingsPage_Loaded(object sender, Windows.UI.Xaml.RoutedEventArgs e)
        {
            ViewModel.OnNotification += ViewModel_OnNotification;
        }

        private void ViewModel_OnNotification(object sender, (string, int) e)
        {
            SettingsNotification.Show(e.Item1, e.Item2);
        }

        private SettingsViewModel ViewModel
        {
            get { return DataContext as SettingsViewModel; }
        }
    }
}
