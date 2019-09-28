using System;
using System.Collections.Generic;
using System.Linq;

namespace Trackmat.Uwp.Activation
{
    public static class SchemeActivationConfig
    {
        private static readonly Dictionary<string, Type> _activationPages = new Dictionary<string, Type>()
        {
            { "items", typeof(Views.TrackItemsPage) },
            { "periods", typeof(Views.PeriodsPage) }
        };

        public static Type GetPage(string pageKey)
        {
            return _activationPages
                    .FirstOrDefault(p => p.Key == pageKey)
                    .Value;
        }

        public static string GetPageKey(Type pageType)
        {
            return _activationPages
                    .FirstOrDefault(v => v.Value == pageType)
                    .Key;
        }
    }
}
