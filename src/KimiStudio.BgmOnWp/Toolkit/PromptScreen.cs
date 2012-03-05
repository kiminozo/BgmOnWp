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
using Action = System.Action;

namespace KimiStudio.BgmOnWp.Toolkit
{
    public abstract class PromptScreen : Screen, IPrompt
    {

        private System.Action hideAction;

        Action IPrompt.HideAction
        {
            get { return hideAction; }
            set { hideAction = value; }
        }

        public void Hide()
        {
            if (hideAction == null) return;
            hideAction();
        }

       // System.Action IPrompt.HideAction { get; set; }
    }
}
