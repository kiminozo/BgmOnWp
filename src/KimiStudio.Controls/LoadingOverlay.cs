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
using Clarity.Phone.Extensions;
using Microsoft.Phone.Shell;

namespace KimiStudio.Controls
{
    public class LoadingOverlay : Control
    {
        public static readonly DependencyProperty OverlayProperty =
           DependencyProperty.Register("Overlay", typeof(Brush), typeof(LoadingOverlay),
           new PropertyMetadata(Application.Current.Resources["PhoneSemitransparentBrush"]));

        public Brush Overlay
        {
            get { return (Brush)GetValue(OverlayProperty); }
            set { SetValue(OverlayProperty, value); }
        }

        public static readonly DependencyProperty TitleProperty =
            DependencyProperty.Register("Title", typeof (string), typeof (LoadingOverlay),
                                        new PropertyMetadata("loading"));

        public string Title
        {
            get { return (string)GetValue(TitleProperty); }
            set { SetValue(TitleProperty, value); }
        }

        private DialogService dialogService;
        private IApplicationBar applicationBar;

        public event EventHandler Completed;

        public LoadingOverlay()
        {
            DefaultStyleKey = typeof(LoadingOverlay);
        }

        public override void OnApplyTemplate()
        {
            Focus();
            base.OnApplyTemplate();
        }

        public void Show()
        {
            if (dialogService != null && dialogService.IsOpen) return;

            dialogService = new DialogService
            {
                AnimationType = DialogService.AnimationTypes.Slide,
                BackgroundBrush = Overlay,
                Child = this,
                IsBackKeyOverride = true,
                HasPopup = true,
            };
            dialogService.Opened += DialogServiceOnOpened;
            dialogService.Closed += DialogServiceOnClosed;
            dialogService.Show();
        }

        public void Hide()
        {
            if (dialogService == null) return;

            dialogService.Hide();
            ResetWorldAndDestroyPopUp();
        }

        private void DialogServiceOnOpened(object sender, EventArgs args)
        {
            applicationBar = dialogService.Page.ApplicationBar;
            if (applicationBar != null)
            {
                dialogService.Page.ApplicationBar = null;
            }
        }

        private void DialogServiceOnClosed(object sender, EventArgs args)
        {
            ResetWorldAndDestroyPopUp();
            OnCompleted();
        }

        private void OnCompleted()
        {
            EventHandler handler = Completed;
            if (handler == null) return;
            handler(this, EventArgs.Empty);
        }

        private void ResetWorldAndDestroyPopUp()
        {
            if (dialogService == null) return;

            if (applicationBar != null)
            {
                dialogService.Page.ApplicationBar = applicationBar;
            }

            dialogService.Child = null;
            dialogService = null;
        }
    }
}
