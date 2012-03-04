using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Threading;
using Clarity.Phone.Extensions;
using Microsoft.Phone.Controls;

namespace KimiStudio.Controls
{
    public class ToastPrompt : Control
    {
        private DialogService popUp;
        protected Image ToastImage;
        private const string ToastImageName = "ToastImage";
        private readonly DispatcherTimer timer;

        private TranslateTransform translate;

        public ToastPrompt()
        {
            DefaultStyleKey = typeof(ToastPrompt);
            Overlay = (Brush)Application.Current.Resources["TransparentBrush"];
            timer = new DispatcherTimer();
            timer.Tick += _timer_Tick;
        }

        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();

            translate = new TranslateTransform();
            RenderTransform = translate;

            ToastImage = GetTemplateChild(ToastImageName) as Image;

            if (ToastImage != null && ImageSource != null)
            {
                ToastImage.Source = ImageSource;
                SetImageVisibility(ImageSource);
            }

            SetTextOrientation(TextWrapping);
        }

        public void Show()
        {
            if (!IsTimerEnabled)
                return;

            popUp = new DialogService
            {
                AnimationType = DialogService.AnimationTypes.Vetical,
                Child = this,
                IsBackKeyOverride = true
            };

            popUp.Opened += _popUp_Opened;
            Dispatcher.BeginInvoke(() => popUp.Show());
        }


        private void _popUp_Opened(object sender, EventArgs e)
        {
            timer.Interval = TimeSpanUntilHidden;
            timer.Start();
        }

        private void _timer_Tick(object state, EventArgs e)
        {
            timer.Stop();
            if (popUp == null) return;
            popUp.Hide();
            ResetWorldAndDestroyPopUp();
        }

        private void ResetWorldAndDestroyPopUp()
        {
            if (popUp != null)
            {
                popUp.Child = null;
                popUp = null;
            }
        }


        #region DependencyProperty
        public TimeSpan TimeSpanUntilHidden
        {
            get { return (TimeSpan)GetValue(TimeSpanUntilHiddenProperty); }
            set { SetValue(TimeSpanUntilHiddenProperty, value); }
        }

        // Using a DependencyProperty as the backing store for MillisecondsUntilHidden.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty TimeSpanUntilHiddenProperty =
            DependencyProperty.Register("TimeSpanUntilHidden", typeof(TimeSpan), typeof(ToastPrompt),
                                        new PropertyMetadata(new TimeSpan(0, 0, 2)));

        public bool IsTimerEnabled
        {
            get { return (bool)GetValue(IsTimerEnabledProperty); }
            set { SetValue(IsTimerEnabledProperty, value); }
        }

        // Using a DependencyProperty as the backing store for IsTimerEnabled.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty IsTimerEnabledProperty =
            DependencyProperty.Register("IsTimerEnabled", typeof(bool), typeof(ToastPrompt), new PropertyMetadata(true));

        public string Title
        {
            get { return (string)GetValue(TitleProperty); }
            set { SetValue(TitleProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Title.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty TitleProperty =
            DependencyProperty.Register("Title", typeof(string), typeof(ToastPrompt), new PropertyMetadata(""));

        public string Message
        {
            get { return (string)GetValue(MessageProperty); }
            set { SetValue(MessageProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Message.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty MessageProperty =
            DependencyProperty.Register("Message", typeof(string), typeof(ToastPrompt), new PropertyMetadata(""));

        public ImageSource ImageSource
        {
            get { return (ImageSource)GetValue(ImageSourceProperty); }
            set { SetValue(ImageSourceProperty, value); }
        }

        // Using a DependencyProperty as the backing store for ImageSource.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ImageSourceProperty =
            DependencyProperty.Register("ImageSource", typeof(ImageSource), typeof(ToastPrompt),
            new PropertyMetadata(OnImageSource));

        public Orientation TextOrientation
        {
            get { return (Orientation)GetValue(TextOrientationProperty); }
            set { SetValue(TextOrientationProperty, value); }
        }

        // Using a DependencyProperty as the backing store for TextOrientation.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty TextOrientationProperty =
            DependencyProperty.Register("TextOrientation", typeof(Orientation), typeof(ToastPrompt), new PropertyMetadata(Orientation.Horizontal));

        public TextWrapping TextWrapping
        {
            get { return (TextWrapping)GetValue(TextWrappingProperty); }
            set { SetValue(TextWrappingProperty, value); }
        }

        // Using a DependencyProperty as the backing store for TextWrapping.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty TextWrappingProperty =
            DependencyProperty.Register("TextWrapping", typeof(TextWrapping), typeof(ToastPrompt), new PropertyMetadata(TextWrapping.NoWrap, OnTextWrapping));

        public Brush Overlay
        {
            get { return (Brush)GetValue(OverlayProperty); }
            set { SetValue(OverlayProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Overlay.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty OverlayProperty =
            DependencyProperty.Register("Overlay", typeof(Brush), typeof(ToastPrompt), new PropertyMetadata(Application.Current.Resources["PhoneSemitransparentBrush"]));

        private static void OnTextWrapping(DependencyObject o, DependencyPropertyChangedEventArgs e)
        {
            var sender = o as ToastPrompt;

            if (sender == null || sender.ToastImage == null)
                return;

            sender.SetTextOrientation((TextWrapping)e.NewValue);
        }

        private static void OnImageSource(DependencyObject o, DependencyPropertyChangedEventArgs e)
        {
            var sender = o as ToastPrompt;

            if (sender == null || sender.ToastImage == null)
                return;

            sender.SetImageVisibility(e.NewValue as ImageSource);
        }

        private void SetImageVisibility(ImageSource source)
        {
            ToastImage.Visibility = (source == null) ? Visibility.Collapsed : Visibility.Visible;
        }

        private void SetTextOrientation(TextWrapping value)
        {
            if (value == TextWrapping.Wrap)
            {
                TextOrientation = Orientation.Vertical;
            }
        } 
        #endregion
    }
}