﻿using System;
using System.Diagnostics;
using System.Linq;
using System.Collections.Generic;
using System.ComponentModel;
using System.Collections.ObjectModel;
using Caliburn.Micro;
using KimiStudio.Bangumi.Api.Commands;
using KimiStudio.Bangumi.Api.Models;
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
        private readonly IPromptManager promptManager;
        private readonly ILoadingService loadingService;


        public bool Authed { get; private set; }

        private IEnumerable<WatchedItemModel> watchedItems;
        public IEnumerable<WatchedItemModel> WatchedItems
        {
            get { return watchedItems; }
            set
            {
                watchedItems = value;
                NotifyOfPropertyChange(() => WatchedItems);
            }
        }

        private IEnumerable<WatchedItemModel> todayCalendarItems;
        public IEnumerable<WatchedItemModel> TodayCalendarItems
        {
            get { return todayCalendarItems; }
            set
            {
                todayCalendarItems = value;
                NotifyOfPropertyChange(() => TodayCalendarItems);
            }
        }

        private IEnumerable<WatchedItemModel> tomorrowCalendarItems;
        public IEnumerable<WatchedItemModel> TomorrowCalendarItems
        {
            get { return tomorrowCalendarItems; }
            set
            {
                tomorrowCalendarItems = value;
                NotifyOfPropertyChange(() => TomorrowCalendarItems);
            }
        }

        #endregion

        #region Private
        public MainPageViewModel(INavigationService navigation, IProgressService progressService, IPromptManager promptManager, ILoadingService loadingService)
        {
            this.navigation = navigation;
            this.progressService = progressService;
            this.promptManager = promptManager;
            this.loadingService = loadingService;
        }

        protected override void OnInitialize()
        {
            base.OnInitialize();
            if (AuthStorage.UserName == null)
                navigation.UriFor<LoginViewModel>().Navigate();

        }

        protected override void OnViewLoaded(object view)
        {
            base.OnViewLoaded(view);

            if (AuthStorage.Authed)
            {
                Authed = true;
                GetWatched();
                return;
            }


            loadingService.Show("登录中\u2026");
            var task = CommandTaskFactory.Create(new LoginCommand(AuthStorage.UserName, AuthStorage.Password));
            task.Result(auth =>
                            {
                                loadingService.Hide();
                                AuthStorage.Auth = auth;
                                Authed = true;
                                GetWatched();
                            });
            task.Exception(err =>
                               {
                                   // progressService.Hide();
                                   loadingService.Hide();
                                   promptManager.ToastError(err, "登录失败");
                               });
            task.Start();
        }

        protected override void OnActivate()
        {
            base.OnActivate();
            //切换用户后更新
            if(!Authed)
            {
                if (AuthStorage.Authed)
                {
                    Authed = true;
                    GetWatched();
                }
            }
        }

        private void GetWatched()
        {
            progressService.Show("加载中\u2026");
            var task = CommandTaskFactory.Create(new GetWatchedCommand(AuthStorage.Auth));
            task.Result(result =>
                            {
                                WatchedItems = result.OrderByDescending(p => p.LastTouch)
                                    .Take(8)
                                    .Select(p => WatchedItemModel.FromBagumiData(p.Subject));
                                GetCalendar();
                            });
            task.Exception(err =>
                               {
                                   progressService.Hide();
                                   promptManager.ToastError(err, "加载失败");
                               });
            task.Start();
        }

        private void GetCalendar()
        {
            var task = CommandTaskFactory.Create(new CalendarCommand());
            task.Result(result =>
            {
                progressService.Hide();
                var today = WeekDay.WeekDayIdOfToday;
                //var tomorrow = WeekDay.GetWeekDayId(DateTime.Today.AddDays(1));
                TodayCalendarItems = from p in result
                                     from subject in p.Items
                                     where p.WeekDay.Id == today
                                     select WatchedItemModel.FromBagumiData(subject);
            });
            task.Exception(err =>
            {
                progressService.Hide();
                promptManager.ToastError(err, "加载失败");
            });
            task.Start();
        }

        #endregion

        #region Public

        public void Logout()
        {
            AuthStorage.Clear();
            Authed = false;
            navigation.UriFor<LoginViewModel>().Navigate();
        }

        public void NavWatchings()
        {
            if(!Authed)return;
            navigation.UriFor<WatchingsViewModel>().WithParam(x => x.Index, 0).Navigate();
        }

        public void NavAll()
        {
            if (!Authed) return;
            navigation.UriFor<WatchingsViewModel>().WithParam(x => x.Index, 0).Navigate();
        }

        public void NavAmine()
        {
            if (!Authed) return;
            navigation.UriFor<WatchingsViewModel>().WithParam(x => x.Index, 1).Navigate();
        }

        public void NavReal()
        {
            if (!Authed) return;
            navigation.UriFor<WatchingsViewModel>().WithParam(x => x.Index, 2).Navigate();
        }

        public void NavCalendar()
        {
            if (!Authed) return;
            navigation.UriFor<CalendarViewModel>().Navigate();
        }

        public void OnTapItem(WatchedItemModel item)
        {
            if (!Authed) return;
            navigation.UriFor<SubjectViewModel>()
                .WithParam(x => x.Id, item.Id)
                .WithParam(x => x.DisplayName, item.Name)
                .WithParam(x => x.UriSource, item.UriSource)
                .Navigate();
        }
        #endregion


    }
}