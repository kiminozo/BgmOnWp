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
        private bool authed;

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
        public MainPageViewModel(INavigationService navigation, IProgressService progressService, IPromptManager promptManager)
        {
            this.navigation = navigation;
            this.progressService = progressService;
            this.promptManager = promptManager;
        }

        protected override void OnViewLoaded(object view)
        {
            base.OnViewLoaded(view);
            progressService.Show("登录中\u2026");

            var task = CommandTaskFactory.Create(new LoginCommand("piova", "piova@live.com"));
            task.Result(auth =>
                            {
                                AuthStorage.Auth = auth;
                                authed = true;
                                GetWatched();
                            });
            task.Exception(err =>
                               {
                                   progressService.Hide();
                                   promptManager.ToastError(err, "登录失败");
                               });
            task.Start();
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