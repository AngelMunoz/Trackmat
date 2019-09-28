using System;
using System.Threading.Tasks;

using Caliburn.Micro;

using Trackmat.Uwp.Services;
using Trackmat.Uwp.ViewModels;

using Windows.ApplicationModel.Activation;

namespace Trackmat.Uwp.Activation
{
    internal class SchemeActivationHandler : ActivationHandler<ProtocolActivatedEventArgs>
    {
        private INavigationService _navigationService;

        public SchemeActivationHandler(INavigationService navigationService)
        {
            _navigationService = navigationService;
        }

        // By default, this handler expects URIs of the format 'wtsapp:sample?paramName1=paramValue1&paramName2=paramValue2'
        protected override async Task HandleInternalAsync(ProtocolActivatedEventArgs args)
        {
            // Create data from activation Uri in ProtocolActivatedEventArgs
            var data = new SchemeActivationData(args.Uri);
            if (data.IsValid)
            {
                _navigationService.Navigate(data.PageType, data.Parameters);
            }

            await Task.CompletedTask;
        }

        protected override bool CanHandleInternal(ProtocolActivatedEventArgs args)
        {
            // If your app has multiple handlers of ProtocolActivationEventArgs
            // use this method to determine which to use. (possibly checking args.Uri.Scheme)
            return true;
        }
    }
}
