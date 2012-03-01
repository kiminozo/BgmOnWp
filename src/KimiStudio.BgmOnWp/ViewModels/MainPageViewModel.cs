using System;
using System.Diagnostics;
using System.Linq;
using System.Collections.Generic;
using System.ComponentModel;
using System.Collections.ObjectModel;
using Caliburn.Micro;
using KimiStudio.Bagumi.Api.Commands;
using KimiStudio.BgmOnWp.Models;
using KimiStudio.BgmOnWp.Storages;
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
            loginCommand.BeginExecute(LoginCallBack,loginCommand);
        }

        private void LoginCallBack(IAsyncResult asyncResult)
        {
            progressService.Hide();
            try
            {
                var command = (LoginCommand)asyncResult.AsyncState;
                var auth = command.EndExecute(asyncResult);
                AuthStorage.Auth = auth;
                authed = true;
                progressService.Show("加载中\u2026");
                var watchedCommand = new GetWatchedCommand(auth);
                watchedCommand.BeginExecute(GetWatchedCallBack, watchedCommand);
            }
            catch (Exception err)
            {
                Debug.WriteLine(err);
                //TODO:
            }
        }


        private void GetWatchedCallBack(IAsyncResult asyncResult)
        {
            progressService.Hide();
            try
            {
                var command = (GetWatchedCommand)asyncResult.AsyncState;
                var result = command.EndExecute(asyncResult);
                Items = result.OrderByDescending(p => p.LastTouch)
                    .Take(8)
                    .Select(p => WatchedItemModel.FromBagumiData(p.Subject));
            }
            catch (Exception err)
            {
                Debug.WriteLine(err);
                //TODO:
            }

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

        public void NavCalendar()
        {
            navigation.UriFor<CalendarViewModel>().Navigate();
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