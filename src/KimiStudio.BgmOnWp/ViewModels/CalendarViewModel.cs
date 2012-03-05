using System;
using System.Collections.Generic;
using System.Linq;
using Caliburn.Micro;
using KimiStudio.Bagumi.Api.Commands;
using KimiStudio.Bagumi.Api.Models;
using KimiStudio.BgmOnWp.Models;
using KimiStudio.BgmOnWp.Storages;
using KimiStudio.BgmOnWp.Toolkit;

namespace KimiStudio.BgmOnWp.ViewModels
{
    public sealed class CalendarViewModel : Conductor<SubjectListViewModel>.Collection.OneActive
    {

        private readonly INavigationService navigation;
        private readonly IProgressService progressService;
        private readonly IPromptManager promptManager;

        public int Index { get; set; }

        public CalendarViewModel(INavigationService navigation, IProgressService progressService, IPromptManager promptManager)
        {
            DisplayName = "每日放送";
            this.navigation = navigation;
            this.progressService = progressService;
            this.promptManager = promptManager;
            Items.Add(new SubjectListViewModel { DisplayName = "星期一" });
            Items.Add(new SubjectListViewModel { DisplayName = "星期二" });
            Items.Add(new SubjectListViewModel { DisplayName = "星期三" });
            Items.Add(new SubjectListViewModel { DisplayName = "星期四" });
            Items.Add(new SubjectListViewModel { DisplayName = "星期五" });
            Items.Add(new SubjectListViewModel { DisplayName = "星期六" });
            Items.Add(new SubjectListViewModel { DisplayName = "星期日" });

            ActivateItem(Items[Index]);
        }

        protected override void OnViewLoaded(object view)
        {
            base.OnViewLoaded(view);
            int index = WeekDay.WeekDayIdOfToday - 1;
            ActivateItem(Items[index]);
        }

        protected override void OnInitialize()
        {
            progressService.Show("加载中\u2026");
            var task = CommandTaskFactory.Create(new CalendarCommand());
            task.Result(result =>
                            {
                                progressService.Hide();
                                result.Apply(x =>
                                                 {
                                                     var item = Items[x.WeekDay.Id - 1];
                                                     item.DisplayName = x.WeekDay.Cn;
                                                     item.UpdateWatchingItems(x.Items);
                                                 });
                            });
            task.Exception(err =>
                               {
                                   progressService.Hide();
                                   promptManager.ToastError(err);
                               });
            task.Start();
            //var command = new CalendarCommand();
            //command.BeginExecute(GetWatchedCallBack, command);
        }

        public void OnTapItem(WatchedItemModel item)
        {
            navigation.UriFor<SubjectViewModel>()
                .WithParam(x => x.Id, item.Id)
                .WithParam(x => x.DisplayName, item.Name)
                .WithParam(x => x.UriSource, item.UriSource)
                .Navigate();
        }

        //private void GetWatchedCallBack(IAsyncResult asyncResult)
        //{
        //    try
        //    {
        //        var command = (CalendarCommand)asyncResult.AsyncState;
        //        var result = command.EndExecute(asyncResult);

        //        result.Apply(x =>
        //                        {
        //                            var item = Items[x.WeekDay.Id - 1];
        //                            item.DisplayName = x.WeekDay.Cn;
        //                            item.UpdateWatchingItems(x.Items);
        //                        });

        //    }
        //    catch (Exception)
        //    {
        //        //TODO:
        //    }
        //    finally
        //    {
        //        progressService.Hide();
        //    }
        //}
    }
}
