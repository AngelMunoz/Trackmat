using System;
using System.Collections.Generic;
using System.Threading.Tasks;

using AdaptiveCards;

using Trackmat.Uwp.Activation;
using Trackmat.Uwp.Views;

using Windows.ApplicationModel;
using Windows.UI;
using Windows.UI.Shell;

namespace Trackmat.Uwp.Services
{
    public static partial class UserActivityService
    {
        public static async Task AddStartUserActivity()
        {
            var activityId = nameof(OverviewPage);
            var displayText = "Welcome to Trackmat";
            var description = $"'{Package.Current.DisplayName}' was started at {DateTime.Now.ToShortTimeString()}";

            var activityData = new UserActivityData(activityId, new SchemeActivationData(typeof(OverviewPage)), displayText, Colors.DarkRed);
            var adaptiveCard = CreateAdaptiveCard(displayText, description);

            await UserActivityService.CreateUserActivityAsync(activityData, adaptiveCard);
        }

        // For more info about adaptive cards see http://adaptivecards.io/
        private static IAdaptiveCard CreateAdaptiveCard(string displayText, string description)
        {
            var adaptiveCard = new AdaptiveCard("1.0");
            var columns = new AdaptiveColumnSet();
            var column = new AdaptiveColumn() { Width = "auto" };

            column.Items.Add(new AdaptiveTextBlock()
            {
                Text = displayText,
                Weight = AdaptiveTextWeight.Bolder,
                Size = AdaptiveTextSize.Large
            });

            column.Items.Add(new AdaptiveTextBlock()
            {
                Text = description,
                Size = AdaptiveTextSize.Medium,
                Weight = AdaptiveTextWeight.Lighter,
                Wrap = true
            });

            columns.Columns.Add(column);
            adaptiveCard.Body.Add(columns);

            return AdaptiveCardBuilder.CreateAdaptiveCardFromJson(adaptiveCard.ToJson());
        }
    }
}
