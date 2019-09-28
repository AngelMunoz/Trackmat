using System;
using System.Collections.Generic;
using System.Threading.Tasks;

using Caliburn.Micro;
using Trackmat.Uwp.Interfaces;
using Trackmat.Uwp.Views;
using Windows.ApplicationModel.Activation;

namespace Trackmat.Uwp.Activation
{
    internal class DefaultActivationHandler : ActivationHandler<IActivatedEventArgs>
    {
        private readonly Type _navElement;
        private readonly INavigationService _navigationService;
        private readonly IDatabaseService _dbs;

        public DefaultActivationHandler(Type navElement, INavigationService navigationService, IDatabaseService database)
        {
            _navElement = navElement;
            _navigationService = navigationService;
            _dbs = database;
        }

        protected override async Task HandleInternalAsync(IActivatedEventArgs args)
        {
            // When the navigation stack isn't restored navigate to the first page,
            // configuring the new page by passing required information as a navigation
            // parameter
            object arguments = null;
            if (args is LaunchActivatedEventArgs launchArgs)
            {
                arguments = launchArgs.Arguments;
            }

            var connStr = await _dbs.GetConnectionStringASync();

            if (string.IsNullOrEmpty(connStr))
            {
                _navigationService.Navigate<SettingsPage>();
            }
            else
            {
                _navigationService.NavigateToViewModel(_navElement, arguments);
            }

            await Task.CompletedTask;
        }

        protected override bool CanHandleInternal(IActivatedEventArgs args)
        {
            // None of the ActivationHandlers has handled the app activation
            return _navigationService.SourcePageType == null && _navElement != null;
        }
    }
}
