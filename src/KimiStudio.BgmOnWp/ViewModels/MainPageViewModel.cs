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
        public MainPageViewModel()
        {
            RecentlyWatchedItem = new RecentlyWatchedViewModel();
        }

        public IScreen RecentlyWatchedItem { get; private set; }

        
    }
}