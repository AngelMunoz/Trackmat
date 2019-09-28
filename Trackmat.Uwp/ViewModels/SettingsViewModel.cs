using System;
using System.Windows.Input;

using Caliburn.Micro;

using Trackmat.Uwp.Helpers;
using Trackmat.Uwp.Services;

using Windows.ApplicationModel;
using Windows.Storage;
using Windows.UI.Xaml;
using System.Threading.Tasks;
using Trackmat.Uwp.Interfaces;
using System.IO;
using Windows.Storage.Pickers;
using System.Linq;
using Windows.Storage.AccessCache;
using Windows.UI.Popups;
using Windows.UI.Xaml.Controls;
using Microsoft.Toolkit.Uwp.UI.Controls;

namespace Trackmat.Uwp.ViewModels
{
    public class SettingsViewModel : Screen, IDeactivate, IGuardClose
    {
        public event EventHandler<(string, int)> OnNotification;
        private ElementTheme _elementTheme = ThemeSelectorService.Theme;
        private readonly IDatabaseService _dbs;

        private string _dbpath;

        public string DbPath { get => _dbpath; set => Set(ref _dbpath, value); }

        public ElementTheme ElementTheme
        {
            get { return _elementTheme; }

            set { Set(ref _elementTheme, value); }
        }

        private string _versionDescription;

        public string VersionDescription
        {
            get { return _versionDescription; }

            set { Set(ref _versionDescription, value); }
        }

        public string _databaseConnStr;

        public string DatabaseConStr
        {
            get { return _databaseConnStr; }
            set { Set(ref _databaseConnStr, value); }
        }

        public SettingsViewModel(IDatabaseService database)
        {
            _dbs = database;
        }

        protected override async void OnInitialize()
        {
            base.OnInitialize();

            VersionDescription = GetVersionDescription();
            DatabaseConStr = await _dbs.GetConnectionStringASync();
            DbPath = ((DatabaseConStr ?? ";").Split(';').FirstOrDefault() ?? "=").Split('=').ElementAtOrDefault(1);
        }

        protected override async void OnDeactivate(bool close)
        {
            var isConfigured = await _dbs.IsConfiguredAsync();
            if (!isConfigured)
            {
                base.OnDeactivate(false);
            }
            else
            {
                base.OnDeactivate(close);
            }
        }

        public async void SetDefault()
        {
            var inLocal = await ApplicationData.Current.LocalFolder.CreateFileAsync("trackmat.db");
            var connstr = $"Filename={ApplicationData.Current.LocalFolder.Path}\\trackmat.db;Async=true";
            await _dbs.SaveConnectionStringAsync(connstr);
            DatabaseConStr = connstr;
            DbPath = inLocal.Path;
            StorageApplicationPermissions.FutureAccessList.AddOrReplace(_dbs.DbConnStrKey, inLocal);
            OnNotification?.Invoke(this, ("Database Created Successfully", 2500));
        }

        public async void OnMigrateExistingDatabase()
        {
            var dialog = new ContentDialog()
            {
                Title = "Warning!",
                Content = "This operation will overwrite your local database are you sure to continue?",
                CloseButtonText = "No, Cancel",
                PrimaryButtonText = "Yes, Continue",
                DefaultButton = ContentDialogButton.Close,
                RequestedTheme = ThemeSelectorService.Theme
            };
            var result = await dialog.ShowAsync();
            if (result == ContentDialogResult.None) return;
            var picker = new FileOpenPicker
            {
                SuggestedStartLocation = PickerLocationId.ComputerFolder
            };
            picker.FileTypeFilter.Add(".db");
            var file = await picker.PickSingleFileAsync();
            if (file == null)
            {
                return;
            }
            StorageFile inLocal = await ApplicationData.Current.LocalFolder.GetFileAsync("trackmat.db");
            if (inLocal == null)
            {
                inLocal = await ApplicationData.Current.LocalFolder.CreateFileAsync("trackmat.db");
            }

            await file.CopyAndReplaceAsync(inLocal);
            var connstr = $"Filename={ApplicationData.Current.LocalFolder.Path}\\trackmat.db;Async=true";
            await _dbs.SaveConnectionStringAsync(connstr);
            DatabaseConStr = connstr;
            DbPath = inLocal.Path;
            StorageApplicationPermissions.FutureAccessList.AddOrReplace(_dbs.DbConnStrKey, inLocal);
            OnNotification?.Invoke(this, ("Database Imported Successfully", 3000));
        }


        public async void SwitchTheme(ElementTheme theme)
        {
            await ThemeSelectorService.SetThemeAsync(theme);
        }

        private void ShowNotification(string message, int duration = 3000)
        {
            var notification = new InAppNotification() { ShowDismissButton = true };
            notification.Show(message, duration);
        }


        private string GetVersionDescription()
        {
            var appName = "AppDisplayName".GetLocalized();
            var package = Package.Current;
            var packageId = package.Id;
            var version = packageId.Version;

            return $"{appName} - {version.Major}.{version.Minor}.{version.Build}.{version.Revision}";
        }
    }
}
