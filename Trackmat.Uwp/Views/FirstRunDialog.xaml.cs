using System;

using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace Trackmat.Uwp.Views
{
    public sealed partial class FirstRunDialog : ContentDialog
    {
        public FirstRunDialog()
        {
            RequestedTheme = (Window.Current.Content as FrameworkElement).RequestedTheme;
            InitializeComponent();
        }
    }
}
