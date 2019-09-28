using System;
using System.Linq;
using System.Threading.Tasks;

using Caliburn.Micro;
using Trackmat.Lib.Models;
using Trackmat.Lib.Services;
using Trackmat.Uwp.Helpers;
using Trackmat.Uwp.Interfaces;
using Trackmat.Uwp.Services;
using Trackmat.Uwp.Views;

namespace Trackmat.Uwp.ViewModels
{
    public class PeriodsViewModel : Conductor<PeriodsDetailViewModel>.Collection.OneActive
    {
        private readonly IDatabaseService _dbs;
        private readonly INavigationService _nav;
        private int page;
        private int limit;

        public int Page { get => page; set => Set(ref page, value); }
        public int Limit { get => limit; set => Set(ref limit, value); }

        public PeriodsViewModel(IDatabaseService database, INavigationService nav)
        {
            _dbs = database;
            _nav = nav;
        }

        protected override async void OnInitialize()
        {
            base.OnInitialize();
            var isConfigured = await _dbs.IsConfiguredAsync();
            if (!isConfigured)
            {
                _nav.Navigate<SettingsPage>();
                await FirstRunDisplayService.ShowIfAppropriateAsync(false);
            }
            else
            {
                await LoadDataAsync();
            }
        }

        public async Task LoadDataAsync()
        {
            Items.Clear();

            var connString = await _dbs.GetConnectionStringASync();
            using var periods = new PeriodService(connString);
            var data = periods.FindAll(new PaginationValues { Page = 1, Limit = 10 });

            Items.AddRange(data.List.Select(p => new PeriodsDetailViewModel(p)));
        }
    }
}
