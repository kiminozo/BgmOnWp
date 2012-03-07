using System;
using System.Collections.Generic;
using System.Linq;
using Caliburn.Micro;
using KimiStudio.Bangumi.Api.Commands;
using KimiStudio.BgmOnWp.Models;
using KimiStudio.BgmOnWp.Storages;
using KimiStudio.BgmOnWp.Toolkit;

namespace KimiStudio.BgmOnWp.ViewModels
{
    public sealed class WatchingsViewModel : Conductor<SubjectListViewModel>.Collection.OneActive
    {

        private readonly INavigationService navigation;
        private readonly IProgressService progressService;
        private readonly IPromptManager promptManager;

        public int Index { get; set; }

        public WatchingsViewModel(INavigationService navigation, IProgressService progressService, IPromptManager promptManager)
        {
            DisplayName = "收視進度";
            this.navigation = navigation;
            this.progressService = progressService;
            this.promptManager = promptManager;
            Items.Add(new SubjectListViewModel { DisplayName = "全部" });
            Items.Add(new SubjectListViewModel { DisplayName = "动画", Filter = p => p.Type == 2 });
            Items.Add(new SubjectListViewModel { DisplayName = "三次元", Filter = p => p.Type == 6 });
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
            var task = CommandTaskFactory.Create(new GetWatchedCommand(AuthStorage.Auth));
            task.Result(result =>
                            {
                                var query = result.OrderByDescending(p => p.LastTouch);
                                Items.Apply(x => x.UpdateWatchingItems(query.Select(p => p.Subject)));
                                progressService.Hide();
                            });
            task.Exception(err =>
                               {
                                   progressService.Hide();
                                   promptManager.ToastError(err);
                               });
            task.Start();
        }

        public void OnTapItem(WatchedItemModel item)
        {
            navigation.UriFor<SubjectViewModel>()
                .WithParam(x => x.Id, item.Id)
                .WithParam(x => x.DisplayName, item.Name)
                .WithParam(x => x.UriSource, item.UriSource)
                .Navigate();
        }
    }
}
