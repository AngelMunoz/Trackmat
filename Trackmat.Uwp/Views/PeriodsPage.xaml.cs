using System;
using System.Linq;

using Microsoft.Toolkit.Uwp.UI.Controls;

using Trackmat.Uwp.ViewModels;

using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace Trackmat.Uwp.Views
{
    public sealed partial class PeriodsPage : Page
    {
        public PeriodsPage()
        {
            InitializeComponent();
        }

        private PeriodsViewModel ViewModel
        {
            get { return DataContext as PeriodsViewModel; }
        }

        private void MasterDetailsViewControl_Loaded(object sender, RoutedEventArgs e)
        {
            if (MasterDetailsViewControl.ViewState == MasterDetailsViewState.Both)
            {
                ViewModel.ActiveItem = ViewModel.Items.FirstOrDefault();
            }
        }
    }
}
