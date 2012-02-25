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
            Items.Add(new WatchingsItemViewModel { DisplayName = "动画", Filter = p => p.Subject.Type == 2 });
            Items.Add(new WatchingsItemViewModel { DisplayName = "三次元", Filter = p => p.Subject.Type == 6 });
            ActivateItem(Items[0]);
        }

        protected override void OnInitialize()
        {
            var getWatchedCommand = new GetWatchedCommand(WatchedCallBack);
            getWatchedCommand.Execute();
        }
        
        private void WatchedCallBack(IList<BagumiData> list)
        {
            var query = list.OrderByDescending(p => p.LastTouch);
            Items.Apply(x => x.UpdateWatchingItems(query));
        }

        public void OnTap(WatchedItemModel item)
        {
            navigation.UriFor<SubjectViewModel>()
                .WithParam(x => x.Id, item.Id)
                .Navigate();
        }
    }
}
