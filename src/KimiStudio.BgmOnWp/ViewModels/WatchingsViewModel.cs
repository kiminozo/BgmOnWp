using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using Caliburn.Micro;
using KimiStudio.BgmOnWp.Api;
using KimiStudio.BgmOnWp.Models;

namespace KimiStudio.BgmOnWp.ViewModels
{
    public sealed class WatchingsViewModel : Conductor<WatchingsItemViewModel>.Collection.OneActive
    {
        private readonly INavigationService navigation;

        public WatchingsViewModel(INavigationService navigation)
        {
            this.navigation = navigation;
            Items.Add(new WatchingsItemViewModel { DisplayName = "全部" });
        }

        protected override void OnInitialize()
        {
            ActivateItem(Items[0]);
        }

        public void OnTap(WatchedItemModel item)
        {
            navigation.UriFor<SubjectViewModel>()
                .WithParam(x => x.Id, item.Id)
                .Navigate();
        }
    }

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
