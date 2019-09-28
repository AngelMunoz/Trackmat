using System;

using Trackmat.Uwp.ViewModels;

namespace Trackmat.Uwp.Views.PeriodsDetail
{
    public sealed partial class MasterView
    {
        public MasterView()
        {
            InitializeComponent();
        }

        public PeriodsDetailViewModel ViewModel => DataContext as PeriodsDetailViewModel;
    }
}
