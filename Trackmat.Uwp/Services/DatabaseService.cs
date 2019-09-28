using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Trackmat.Uwp.Helpers;
using Trackmat.Uwp.Interfaces;
using Windows.Storage;

namespace Trackmat.Uwp.Services
{
    public class DatabaseService : IDatabaseService
    {
        public string DbConnStrKey { get; } = "trackmat:database-constr";

        public Task<string> GetConnectionStringASync()
        {
            return ApplicationData.Current.RoamingSettings.ReadAsync<string>(DbConnStrKey);
        }

        public Task<bool> IsConfiguredAsync()
        {
            return GetConnectionStringASync().ContinueWith(res => !string.IsNullOrEmpty(res.Result));
        }

        public Task SaveConnectionStringAsync(string connection)
        {
            return ApplicationData.Current.RoamingSettings.SaveAsync(DbConnStrKey, connection);
        }
    }
}
