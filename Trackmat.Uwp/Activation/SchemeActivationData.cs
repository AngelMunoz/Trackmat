﻿using System;
using System.Collections.Generic;
using System.Web;

namespace Trackmat.Uwp.Activation
{
    public class SchemeActivationData
    {
        // More details about this functionality can be found at https://github.com/Microsoft/WindowsTemplateStudio/blob/master/docs/features/deep-linking.md
        // Also update this protocol name with the same value as package.appxmanifest.
        private const string ProtocolName = "tun-trackmat";

        public Type PageType { get; private set; }

        public Uri Uri { get; private set; }

        public Dictionary<string, string> Parameters { get; private set; } = new Dictionary<string, string>();

        public bool IsValid => PageType != null;

        public SchemeActivationData(Uri activationUri)
        {
            PageType = SchemeActivationConfig.GetPage(activationUri.AbsolutePath);

            if (!IsValid || string.IsNullOrEmpty(activationUri.Query))
            {
                return;
            }

            var uriQuery = HttpUtility.ParseQueryString(activationUri.Query);
            foreach (var paramKey in uriQuery.AllKeys)
            {
                Parameters.Add(paramKey, uriQuery.Get(paramKey));
            }
        }

        public SchemeActivationData(Type pageType, Dictionary<string, string> parameters = null)
        {
            PageType = pageType;
            Parameters = parameters;
            Uri = BuildUri();
        }

        private Uri BuildUri()
        {
            var pageKey = SchemeActivationConfig.GetPageKey(PageType);
            var uriBuilder = new UriBuilder($"{ProtocolName}:{pageKey}");
            var query = HttpUtility.ParseQueryString(string.Empty);
            if (Parameters != null)
            {
                foreach (var parameter in Parameters)
                {
                    query.Set(parameter.Key, parameter.Value);
                }
            }

            uriBuilder.Query = query.ToString();
            return new Uri(uriBuilder.ToString());
        }
    }
}
