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
using Clarity.Phone.Extensions;
using Microsoft.Phone.Shell;

namespace KimiStudio.Controls
{
    public class PopupPrompt : ContentControl
    {

        public static readonly DependencyProperty TitleProperty =
            DependencyProperty.Register("Title", typeof(string), typeof(PopupPrompt), new PropertyMetadata(default(string)));

        public string Title
        {
            get { return (string)GetValue(TitleProperty); }
            set { SetValue(TitleProperty, value); }
        }



        // Using a DependencyProperty as the backing store for Overlay.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty OverlayProperty =
            DependencyProperty.Register("Overlay", typeof(Brush), typeof(PopupPrompt),
            new PropertyMetadata(Application.Current.Resources["PhoneSemitransparentBrush"]));

        public Brush Overlay
        {
            get { return (Brush)GetValue(OverlayProperty); }
            set { SetValue(OverlayProperty, value); }
        }

        public static readonly DependencyProperty IsCancelVisibleProperty =
            DependencyProperty.Register("IsCancelVisible", typeof(bool), typeof(PopupPrompt), new PropertyMetadata(default(bool)));

        public bool IsCancelVisible
        {
            get { return (bool)GetValue(IsCancelVisibleProperty); }
            set { SetValue(IsCancelVisibleProperty, value); }
        }

        public static readonly DependencyProperty TitleBackgroundProperty =
            DependencyProperty.Register("TitleBackground", typeof(Brush), typeof(PopupPrompt),
            new PropertyMetadata(Application.Current.Resources["PhoneAccentBrush"]));

        public Brush TitleBackground
        {
            get { return (Brush)GetValue(TitleBackgroundProperty); }
            set { SetValue(TitleBackgroundProperty, value); }
        }


        public static readonly DependencyProperty IsShowButtonInAppBarProperty =
            DependencyProperty.Register("IsShowButtonInAppBar", typeof(bool), typeof(PopupPrompt), new PropertyMetadata(false));

        public bool IsShowButtonInAppBar
        {
            get { return (bool)GetValue(IsShowButtonInAppBarProperty); }
            set { SetValue(IsShowButtonInAppBarProperty, value); }
        }

        private DialogService dialogService;
        private IApplicationBar applicationBar;

        public event EventHandler Completed;


        private Button okButton;
        private Button cancelButton;


        public PopupPrompt()
        {
            DefaultStyleKey = typeof(PopupPrompt);


            SetCancelButtonVisibility(IsCancelVisible);
        }

        public bool? Result { get; set; }


        public override void OnApplyTemplate()
        {
            Focus();
            base.OnApplyTemplate();

            okButton = (Button)GetTemplateChild("okButton");
            cancelButton = (Button)GetTemplateChild("cancelButton");
            SetCancelButtonVisibility(IsCancelVisible);
            okButton.Click += (s, e) =>
                                  {
                                      Result = true;
                                      Hide();
                                  };
            cancelButton.Click += (s, e) =>
                                      {
                                          Result = false;
                                          Hide();
                                      };
        }


        public void Show()
        {
            if (dialogService != null && dialogService.IsOpen) return;

            dialogService = new DialogService
                                {
                                    AnimationType = DialogService.AnimationTypes.Slide,
                                    BackgroundBrush = Overlay,
                                    Child = this,
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



        private void SetCancelButtonVisibility(bool isCancelVisible)
        {
            if (cancelButton == null) return;

            cancelButton.Visibility = isCancelVisible ? Visibility.Visible : Visibility.Collapsed;
        }

    }
}
