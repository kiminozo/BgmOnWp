using System;
using System.ComponentModel;
using System.Collections.ObjectModel;
using Caliburn.Micro;

namespace KimiStudio.BgmOnWp.ViewModels
{
    public sealed class MainPageViewModel : Screen
    {
        public MainPageViewModel()
        {
            RecentlyWatchedItem = new RecentlyWatchedViewModel();
        }

        public IScreen RecentlyWatchedItem { get; private set; }
        ///// <summary>
        ///// ItemViewModel 对象的集合。
        ///// </summary>
        //public ObservableCollection<ItemViewModel> Items { get; private set; }
    }
}