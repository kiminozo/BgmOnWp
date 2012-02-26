using System;
using System.Linq;
using System.Collections.Generic;
using System.ComponentModel;
using System.Collections.ObjectModel;
using Caliburn.Micro;
using KimiStudio.BgmOnWp.Api;
using KimiStudio.BgmOnWp.ModelMessages;
using KimiStudio.BgmOnWp.Models;
using KimiStudio.BgmOnWp.Toolkit;

namespace KimiStudio.BgmOnWp.ViewModels
{
    public sealed class MainPageViewModel : Screen, IHandle<LoginMessage>
    {
        private readonly INavigationService navigation;
        private readonly IProgressService progressService;
        private readonly IEventAggregator eventAggregator;

        private IEnumerable<WatchedItemModel> items;
        public IEnumerable<WatchedItemModel> Items
        {
            get { return items; }
            set
            {
                items = value;
                NotifyOfPropertyChange(() => Items);
            }
        }

        public MainPageViewModel(INavigationService navigation, IProgressService progressService, IEventAggregator eventAggregator)
        {
            this.navigation = navigation;
            this.progressService = progressService;
            this.eventAggregator = eventAggregator;
        }

        protected override void OnActivate()
        {
            eventAggregator.Subscribe(this);
            base.OnActivate();
        }

        protected override void OnDeactivate(bool close)
        {
            eventAggregator.Unsubscribe(this);
            base.OnDeactivate(close);
        }


        protected override void OnViewLoaded(object view)
        {
            base.OnViewLoaded(view);
            progressService.Show("登录中\u2026");
            var loginCommand = new LoginCommand("kiminozo", "haruka", eventAggregator.Publish);
            loginCommand.Execute();
        }

        private void WatchedCallBack(IEnumerable<BagumiData> list)
        {
            Items = list.OrderByDescending(p => p.LastTouch)
                .Take(8)
                .Select(WatchedItemModel.FromBagumiData);
            progressService.Hide();
        }

        void IHandle<LoginMessage>.Handle(LoginMessage message)
        {
            if (message.Cancelled)
            {
                progressService.Hide();
            }
            else
            {
                progressService.Show("加载中\u2026");
                //var getWatchedCommand = new GetWatchedCommand(WatchedCallBack);
                //getWatchedCommand.Execute();
            }
        }



        public void NavWatchings()
        {
            navigation.UriFor<WatchingsViewModel>().Navigate();
        }

        public void OnTap(WatchedItemModel item)
        {
            navigation.UriFor<SubjectViewModel>()
                .WithParam(x => x.Id, item.Id)
                .WithParam(x => x.DisplayName, item.Name)
                .WithParam(x => x.UriSource, item.UriSource)
                .Navigate();
        }
    }
}