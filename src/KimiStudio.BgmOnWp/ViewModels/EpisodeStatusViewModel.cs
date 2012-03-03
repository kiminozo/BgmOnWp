using System;
using System.Collections.Generic;
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
using KimiStudio.BgmOnWp.Models;
using KimiStudio.BgmOnWp.Toolkit;

namespace KimiStudio.BgmOnWp.ViewModels
{
    public class EpisodeStatusViewModel : Screen, IPrompt
    {
        private string cnName;
        private int selectedIndex;
        private IEnumerable<EpisodeStatusModel> items;

        public string CnName
        {
            get { return cnName; }
            set
            {
                cnName = value;
                NotifyOfPropertyChange(() => CnName);
            }
        }

        /// <summary>
        /// 
        /// <remarks>看过，看到，想看，抛弃</remarks>
        /// </summary>
        public int SelectedIndex
        {
            get { return selectedIndex; }
            set
            {
                selectedIndex = value;
                NotifyOfPropertyChange(() => SelectedIndex);
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

        public void Setup(EpisodeModel episode)
        {
            DisplayName = episode.Name;
            CnName = episode.CnName;
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
        }



        #region Implementation of IPrompt

        public void PromptResult(bool canceled)
        {

        }

        #endregion
    }

    
}
