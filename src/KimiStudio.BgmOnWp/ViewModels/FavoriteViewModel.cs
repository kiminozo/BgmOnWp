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
    public sealed class FavoriteViewModel : Screen, IPrompt
    {
        public FavoriteViewModel()
        {
            DisplayName = "收藏";
        }

        #region Implementation of IPrompt

        public void PromptResult(bool canceled)
        {
            
        }

        #endregion
    }
}
