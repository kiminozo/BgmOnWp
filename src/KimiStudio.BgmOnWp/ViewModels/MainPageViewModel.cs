using System;
using System.Linq;
using System.Collections.Generic;
using System.ComponentModel;
using System.Collections.ObjectModel;
using Caliburn.Micro;
using KimiStudio.BgmOnWp.Api;
using KimiStudio.BgmOnWp.Models;

namespace KimiStudio.BgmOnWp.ViewModels
{
    public sealed class MainPageViewModel : Screen
    {
        private readonly INavigationService navigation;

        public MainPageViewModel(RecentlyWatchedViewModel recentlyWatched, INavigationService navigation)
        {
            RecentlyWatchedItem = recentlyWatched;
            this.navigation = navigation;
        }

        public IScreen RecentlyWatchedItem { get; private set; }

        public void NavWatchings()
        {
            navigation.UriFor<WatchingsViewModel>().Navigate();
        }
        
    }
}