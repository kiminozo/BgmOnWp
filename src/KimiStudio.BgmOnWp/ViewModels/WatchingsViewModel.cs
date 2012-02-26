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
using KimiStudio.BgmOnWp.ModelMessages;
using KimiStudio.BgmOnWp.Models;
using KimiStudio.BgmOnWp.Toolkit;

namespace KimiStudio.BgmOnWp.ViewModels
{
    public sealed class WatchingsViewModel : Conductor<WatchingsItemViewModel>.Collection.OneActive
    {

        private readonly INavigationService navigation;
        private readonly IProgressService progressService;

        public int Index { get; set; }

        public WatchingsViewModel(INavigationService navigation, IProgressService progressService)
        {
            DisplayName = "收視進度";
            this.navigation = navigation;
            this.progressService = progressService;
            Items.Add(new WatchingsItemViewModel { DisplayName = "全部" });
            Items.Add(new WatchingsItemViewModel { DisplayName = "动画", Filter = p => p.Subject.Type == 2 });
            Items.Add(new WatchingsItemViewModel { DisplayName = "三次元", Filter = p => p.Subject.Type == 6 });
            ActivateItem(Items[Index]);
        }

        protected override void OnViewLoaded(object view)
        {
            base.OnViewLoaded(view);
            ActivateItem(Items[Index]);
        }

        protected override void OnInitialize()
        {
            progressService.Show("加载中\u2026");
            var getWatchedCommand = new GetWatchedCommand();
            getWatchedCommand.Execute(Handle);
        }

        public void OnTapItem(WatchedItemModel item)
        {
            navigation.UriFor<SubjectViewModel>()
                .WithParam(x => x.Id, item.Id)
                .WithParam(x => x.DisplayName, item.Name)
                .WithParam(x => x.UriSource, item.UriSource)
                .Navigate();
        }

        private void Handle(WatchedsMessage message)
        {
            try
            {
                if (message.Cancelled) return;
                var query = message.Watcheds.OrderByDescending(p => p.LastTouch);
                Items.Apply(x => x.UpdateWatchingItems(query));
            }
            finally
            {

                progressService.Hide();
            }
        }
    }
}
