using System.Collections.Generic;
using System.Linq;
using Caliburn.Micro;
using KimiStudio.BgmOnWp.Api;
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

        protected override void OnActivate()
        {
            base.OnActivate();
            var getWatchedCommand = new GetWatchedCommand(WatchedCallBack);
            getWatchedCommand.Execute();
        }

        private void WatchedCallBack(IList<BagumiData> list)
        {
            WatchingItems = list.OrderByDescending(p => p.LastTouch)
                .Select(p => new WatchedItemModel
                                 {
                                     Id = p.Subject.Id,
                                     Name = p.Name,
                                     UriSource = p.Subject.Images.Large
                                 });
        }
    }
}