using System;
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
using KimiStudio.BgmOnWp.Toolkit;

namespace KimiStudio.BgmOnWp.ViewModels
{
    public class EpisodeStatusViewModel : Screen, IPrompt
    {
        private string cnName;

        public string CnName
        {
            get { return cnName; }
            set
            {
                cnName = value;
                NotifyOfPropertyChange(() => CnName);
            }
        }

        public int SelectIndex
        {
            get { return selectIndex; }
            set
            {
                selectIndex = value;
                NotifyOfPropertyChange(() => SelectIndex);
            }
        }

        private int selectIndex;

        #region Implementation of IPrompt

        public void PromptResult(bool canceled)
        {

        }

        #endregion
    }
}
