using System;

using Trackmat.Uwp.ViewModels;

using Windows.UI.Xaml.Controls;

namespace Trackmat.Uwp.Views
{
    public sealed partial class OverviewPage : Page
    {
        public OverviewPage()
        {
            InitializeComponent();
        }

        private OverviewViewModel ViewModel
        {
            get { return DataContext as OverviewViewModel; }
        }
    }
}
