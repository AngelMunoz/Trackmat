using System;

using Trackmat.Uwp.ViewModels;

using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;

namespace Trackmat.Uwp.Views
{
    public sealed partial class TrackItemsPage : Page
    {
        // For more details see the documentation at https://docs.microsoft.com/windows/communitytoolkit/controls/datagrid
        public TrackItemsPage()
        {
            InitializeComponent();
        }

        private TrackItemsViewModel ViewModel
        {
            get { return DataContext as TrackItemsViewModel; }
        }

        protected override async void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            var canContinue = await ViewModel.CheckConfigAsync();
            if (canContinue)
            {
                await ViewModel.LoadDataAsync();
            }
        }
    }
}
