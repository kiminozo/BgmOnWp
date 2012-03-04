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

        public Brush Overlay
        {
            get { return (Brush)GetValue(OverlayProperty); }
            set { SetValue(OverlayProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Overlay.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty OverlayProperty =
            DependencyProperty.Register("Overlay", typeof(Brush), typeof(PopupPrompt), new PropertyMetadata(Application.Current.Resources["PhoneSemitransparentBrush"]));


        public static readonly DependencyProperty IsOpenProperty =
            DependencyProperty.Register("IsOpen", typeof(bool), typeof(PopupPrompt),
                                        new PropertyMetadata(IsOpenPropertyChanged));

        public bool IsOpen
        {
            get { return (bool)GetValue(IsOpenProperty); }
            set { SetValue(IsOpenProperty, value); }
        }

        private static void IsOpenPropertyChanged(DependencyObject o, DependencyPropertyChangedEventArgs args)
        {
            var sender = o as PopupPrompt;
            if (sender == null || args.OldValue == args.NewValue) return;
            ;
            sender.SetIsOpen((bool)args.NewValue);
        }



        private DialogService dialogService;

        public event EventHandler Completed;



        public PopupPrompt()
        {
            DefaultStyleKey = typeof(PopupPrompt);
        }

        public override void OnApplyTemplate()
        {
            Focus();
            base.OnApplyTemplate();
        }

        private void SetIsOpen(bool isOpen)
        {
            if (isOpen)
            {
                Show();
            }
            else
            {
                Hide();
            }
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
            dialogService.Closed += DialogServiceOnClosed;
            dialogService.Show();
        }

        public void Hide()
        {
            if (dialogService == null) return;

            dialogService.Hide();
            ResetWorldAndDestroyPopUp();
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

            dialogService.Child = null;
            dialogService = null;
        }


    }
}
