using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Trackmat.Uwp.Interfaces
{
    public interface IDatabaseService
    {
        public string DbConnStrKey { get; }
        public Task<string> GetConnectionStringASync();
        public Task SaveConnectionStringAsync(string connection);
        public Task<bool> IsConfiguredAsync();
    }
}
