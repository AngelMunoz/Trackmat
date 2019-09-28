using System;

using Caliburn.Micro;
using Trackmat.Lib.Models;
using Trackmat.Uwp.Interfaces;
using Trackmat.Uwp.Views;

namespace Trackmat.Uwp.ViewModels
{
    public class PeriodsDetailViewModel : Screen
    {

        public PeriodsDetailViewModel(Period item)
        {
            Item = item;
        }

        public Period Item { get; }
    }
}
