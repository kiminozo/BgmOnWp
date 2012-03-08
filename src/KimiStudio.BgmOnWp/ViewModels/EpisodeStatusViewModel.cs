﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using Caliburn.Micro;
using KimiStudio.Bangumi.Api.Commands;
using KimiStudio.Bangumi.Api.Models;
using KimiStudio.BgmOnWp.Models;
using KimiStudio.BgmOnWp.Storages;
using KimiStudio.BgmOnWp.Toolkit;

namespace KimiStudio.BgmOnWp.ViewModels
{
    public class EpisodeStatusViewModel : Screen, IPrompt
    {
        #region Property
        private string cnName;
        private IEnumerable<EpisodeStatusModel> items;
        private EpisodeModel episodeModel;

        public string CnName
        {
            get { return cnName; }
            set
            {
                cnName = value;
                NotifyOfPropertyChange(() => CnName);
            }
        }

        private EpisodeStatusModel selected;

        /// <summary>
        /// 
        /// <remarks>看过，看到，想看，抛弃</remarks>
        /// </summary>
        public EpisodeStatusModel Selected
        {
            get { return selected; }
            set
            {
                selected = value;
                NotifyOfPropertyChange(() => Selected);
            }
        }

        public IEnumerable<EpisodeStatusModel> Items
        {
            get { return items; }
            set
            {
                items = value;
                NotifyOfPropertyChange(() => Items);
            }
        }


        #endregion

        private IEnumerable<int> episodeIdList;

        private readonly IProgressService progressService;
        private readonly IPromptManager promptManager;

        public EpisodeStatusViewModel(IProgressService progressService, IPromptManager promptManager)
        {
            this.progressService = progressService;
            this.promptManager = promptManager;
        }

        public void Setup(EpisodeModel episode, IEnumerable<int> episodeIds = null)
        {
            episodeModel = episode;
            DisplayName = episode.Name;
            CnName = episode.CnName;
            episodeIdList = episodeIds;
            SetSelectType(episode.WatchState);
        }

        public void SetSelectType(WatchState watchState)
        {
            switch (watchState)
            {
                case WatchState.Queue:
                    Items = EpisodeStatuses.Queue;
                    break;
                case WatchState.Watched:
                    Items = EpisodeStatuses.Watched;
                    break;
                case WatchState.Drop:
                    Items = EpisodeStatuses.Drop;
                    break;
                default:
                    Items = EpisodeStatuses.None;
                    break;
            }
            Selected = items.First();
        }

        #region Implementation of IPrompt

        public void PromptResult(bool canceled)
        {
            if (canceled) return;

            if (Selected != null)
            {
                progressService.Show("提交中\u2026");

                
               
                var updateInfo = new ProgressUpdateInfo
                                     {
                                         EpisodeId = episodeModel.Id,
                                         Method = Selected.Method,
                                     };
                if (selected.IsEnd)
                {
                    updateInfo.Episodes = episodeIdList.Where(p => p <= episodeModel.Id);
                }
                var task = CommandTaskFactory.Create(new ProgressUpdateCommand(updateInfo, AuthStorage.Auth));
                task.Result(result =>
                                {
                                    if (result.IsSuccess())
                                        episodeModel.Update(GetState(updateInfo));
                                    progressService.Hide();
                                    promptManager.ToastInfo("进度保存成功");
                                });
                task.Exception(err =>
                                   {
                                       progressService.Hide();
                                       promptManager.ToastError(err, "进度保存失败");
                                   });
                task.Start();
            }
        }


        #endregion

        private WatchState GetState(ProgressUpdateInfo updateInfo)
        {
            switch (updateInfo.Method)
            {
                case ProgressUpdateInfo.Drop: return WatchState.Drop;
                case ProgressUpdateInfo.Queue: return WatchState.Queue;
                case ProgressUpdateInfo.Watched: return WatchState.Watched;
                default: return WatchState.None;
            }

        }
    }


}
