using System;

using Trackmat.Uwp.ViewModels;

namespace Trackmat.Uwp.Views.PeriodsDetail
{
    public sealed partial class DetailsView
    {
        public DetailsView()
        {
            InitializeComponent();
        }

        public PeriodsDetailViewModel ViewModel => DataContext as PeriodsDetailViewModel;
    }
}
