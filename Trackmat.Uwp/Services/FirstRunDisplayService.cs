using System;
using System.Threading.Tasks;

using Microsoft.Toolkit.Uwp.Helpers;

using Trackmat.Uwp.Views;

namespace Trackmat.Uwp.Services
{
    public static class FirstRunDisplayService
    {
        private static bool shown = false;

        internal static async Task ShowIfAppropriateAsync(bool isConfigured)
        {
            if (!isConfigured || SystemInformation.IsFirstRun && !shown)
            {
                shown = true;
                var dialog = new FirstRunDialog();
                await dialog.ShowAsync();
            }
        }
    }
}
