using System;
using System.Collections.Generic;
using System.Linq;
using Caliburn.Micro;
using KimiStudio.BgmOnWp.Models;


namespace KimiStudio.BgmOnWp.ViewModels
{
    public class WatchingsItemViewModel : Screen
    {
        private IEnumerable<WatchedItemModel> watchingItems;

        public IEnumerable<WatchedItemModel> WatchingItems
        {
            get { return watchingItems; }
            set
            {
                watchingItems = value;
                NotifyOfPropertyChange(() => WatchingItems);
            }
        }

        public Func<BagumiData, bool> Filter { get; set; }

        public void UpdateWatchingItems(IEnumerable<BagumiData> itemModels)
        {
            var query = Filter == null ? itemModels : itemModels.Where(Filter);
            WatchingItems = query.Select(WatchedItemModel.FromBagumiData);
        }

    }
}