using System;
using System.Net;
using System.Reflection;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace KimiStudio.Controls
{
    public class Thumbnail : Control
    {
        #region DependencyPropertys

        public static readonly DependencyProperty TextProperty =
            DependencyProperty.Register("Text", typeof(string), typeof(Thumbnail),
                                        new PropertyMetadata(TextPropertyChanged));

        public static readonly DependencyProperty ImageSourceProperty =
            DependencyProperty.Register("UriSource", typeof(Uri), typeof(Thumbnail),
                                        new PropertyMetadata(ImageSourcePropertyChanged));
        
        private static void TextPropertyChanged(DependencyObject o, DependencyPropertyChangedEventArgs e)
        {
            var sender = o as Thumbnail;
            if (sender == null || e.NewValue == e.OldValue) return;
            sender.SetText(e.NewValue as string);
        }

        private static void ImageSourcePropertyChanged(DependencyObject o, DependencyPropertyChangedEventArgs e)
        {
            var sender = o as Thumbnail;
            if (sender == null || e.NewValue == e.OldValue) return;
            sender.SetImageBrush(e.NewValue as Uri);
        }
        #endregion

        private TextBlock imageName;
        private ImageBrush imageBrush;
        private Rectangle imageNameBackground;

        public Thumbnail()
        {
            DefaultStyleKey = typeof(Thumbnail);
        }

        public string Text
        {
            get { return (string)GetValue(TextProperty); }
            set { SetValue(TextProperty, value); }
        }

        public Uri UriSource
        {
            get { return (Uri)GetValue(ImageSourceProperty); }
            set { SetValue(ImageSourceProperty, value); }
        }

        public override void OnApplyTemplate()
        {
            imageName = (TextBlock)GetTemplateChild("imageName");
            imageBrush = (ImageBrush)GetTemplateChild("imageBrush");
            imageNameBackground = (Rectangle)GetTemplateChild("imageNameBackground");
            SetText(Text);
            SetImageBrush(UriSource);
            base.OnApplyTemplate();
        }

        private void SetText(string text)
        {
            if (imageName == null) return;
            SetIsShowText(!string.IsNullOrEmpty(text));
            imageName.Text = text;
        }

        private void SetImageBrush(Uri uriSource)
        {
            if (imageBrush == null) return;

            if (uriSource == null)
            {
                imageBrush.ImageSource = null;
                return;
            }
            if (uriSource.IsAbsoluteUri)
            {
                imageBrush.ImageSource = new StorageCachedImage(uriSource);
            }
            else
            {
                imageBrush.ImageSource = new BitmapImage(uriSource);
            }
        }

        private void SetIsShowText(bool isShow)
        {
            if (imageName == null || imageNameBackground == null) return;
            if (isShow)
            {
                imageName.Visibility = Visibility.Visible;
                imageNameBackground.Visibility = Visibility.Visible;
            }
            else
            {
                imageName.Visibility = Visibility.Collapsed;
                imageNameBackground.Visibility = Visibility.Collapsed;
            }
        }
    }
}
