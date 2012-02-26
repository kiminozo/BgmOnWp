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
    public sealed class MainPageViewModel : Screen
    {
        #region Property
        private readonly INavigationService navigation;
        private readonly IProgressService progressService;
        private bool authed;

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
        
        #endregion

        #region Private
        public MainPageViewModel(INavigationService navigation, IProgressService progressService)
        {
            this.navigation = navigation;
            this.progressService = progressService;
        }

        protected override void OnViewLoaded(object view)
        {
            base.OnViewLoaded(view);
            progressService.Show("登录中\u2026");
            var loginCommand = new LoginCommand("kiminozo", "haruka");
            loginCommand.Execute(Handle);
        }

        private void Handle(LoginMessage message)
        {
            progressService.Hide();

            if (message.Cancelled)
            {
                //TODO:login
            }
            else
            {
                authed = true;
                progressService.Show("加载中\u2026");
                var getWatchedCommand = new GetWatchedCommand();
                getWatchedCommand.Execute(Handle);
            }
        }

        private void Handle(WatchedsMessage message)
        {
            progressService.Hide();

            if (message.Cancelled) return;
            Items = message.Watcheds.OrderByDescending(p => p.LastTouch)
               .Take(8)
               .Select(WatchedItemModel.FromBagumiData);

        } 
        #endregion

        #region Public

        public void NavWatchings()
        {
            navigation.UriFor<WatchingsViewModel>().WithParam(x => x.Index, 0).Navigate();
        }
        
        public void NavAll()
        {
            navigation.UriFor<WatchingsViewModel>().WithParam(x => x.Index, 0).Navigate();
        }

        public void NavAmine()
        {
            navigation.UriFor<WatchingsViewModel>().WithParam(x => x.Index, 1).Navigate();
        }

        public void NavReal()
        {
            navigation.UriFor<WatchingsViewModel>().WithParam(x => x.Index, 2).Navigate();
        }

        public void OnTapItem(WatchedItemModel item)
        {
            navigation.UriFor<SubjectViewModel>()
                .WithParam(x => x.Id, item.Id)
                .WithParam(x => x.DisplayName, item.Name)
                .WithParam(x => x.UriSource, item.UriSource)
                .Navigate();
        } 
        #endregion


    }
}