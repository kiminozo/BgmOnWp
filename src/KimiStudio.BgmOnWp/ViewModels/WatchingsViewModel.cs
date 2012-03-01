using System;
using System.Collections.Generic;
using System.Linq;
using Caliburn.Micro;
using KimiStudio.Bagumi.Api.Commands;
using KimiStudio.BgmOnWp.Models;
using KimiStudio.BgmOnWp.Storages;
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
            Items.Add(new WatchingsItemViewModel { DisplayName = "动画", Filter = p => p.Type == 2 });
            Items.Add(new WatchingsItemViewModel { DisplayName = "三次元", Filter = p => p.Type == 6 });
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
            var watchedCommand = new GetWatchedCommand(AuthStorage.Auth);
            watchedCommand.BeginExecute(GetWatchedCallBack, watchedCommand);
        }

        public void OnTapItem(WatchedItemModel item)
        {
            navigation.UriFor<SubjectViewModel>()
                .WithParam(x => x.Id, item.Id)
                .WithParam(x => x.DisplayName, item.Name)
                .WithParam(x => x.UriSource, item.UriSource)
                .Navigate();
        }

        private void GetWatchedCallBack(IAsyncResult asyncResult)
        {
            try
            {
                var command = (GetWatchedCommand)asyncResult.AsyncState;
                var result = command.EndExecute(asyncResult);
                var query = result.OrderByDescending(p => p.LastTouch);
                Items.Apply(x => x.UpdateWatchingItems(query.Select(p => p.Subject)));
            }
            catch (Exception)
            {
                //TODO:
            }
            finally
            {
                progressService.Hide();
            }
        } 
    }
}
