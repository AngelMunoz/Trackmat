using System;
using System.Collections.ObjectModel;
using System.Threading.Tasks;

using Caliburn.Micro;
using Trackmat.Lib.Models;
using Trackmat.Lib.Services;
using Trackmat.Uwp.Interfaces;
using Trackmat.Uwp.Services;
using Trackmat.Uwp.Views;

namespace Trackmat.Uwp.ViewModels
{
    public class TrackItemsViewModel : Screen
    {
        private readonly IDatabaseService _dbs;
        private readonly INavigationService _nav;
        public ObservableCollection<TrackItem> Source { get; } = new ObservableCollection<TrackItem>();

        public TrackItemsViewModel(IDatabaseService database, INavigationService nav)
        {
            _dbs = database;
            _nav = nav;
        }

        public async Task<bool> CheckConfigAsync()
        {
            var isConfigured = await _dbs.IsConfiguredAsync();
            if (!isConfigured)
            {
                _nav.Navigate<SettingsPage>();
                await FirstRunDisplayService.ShowIfAppropriateAsync(false);
                return false;
            }
            return true;
        }

        public async Task LoadDataAsync()
        {
            Source.Clear();


            var connStr = await _dbs.GetConnectionStringASync();
            using var trackitems = new TrackItemService(connStr);
            var data = trackitems.FindAll(new PaginationValues { Page = 1, Limit = 10 });

            foreach (var item in data.List)
            {
                Source.Add(item);
            }
        }
    }
}
