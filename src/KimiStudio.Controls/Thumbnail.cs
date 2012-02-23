using System;
using System.Net;
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
        public readonly static DependencyProperty TextProperty =
            DependencyProperty.Register("Text", typeof(string), typeof(Thumbnail), new PropertyMetadata(TextPropertyChanged));

        public readonly static DependencyProperty ImageSourceProperty =
            DependencyProperty.Register("UriSource", typeof(Uri), typeof(Thumbnail), new PropertyMetadata(ImageSourcePropertyChanged));


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
            base.OnApplyTemplate();
            imageName = GetTemplateChild("imageName") as TextBlock;
            imageBrush = GetTemplateChild("imageBrush") as ImageBrush;
            SetText(Text);
            SetImageBrush(UriSource);
        }

        private void SetText(string text)
        {
            if (imageName == null) return; ;
            imageName.Text = text;
        }

        private void SetImageBrush(Uri uriSource)
        {
            if (imageBrush == null) return;

            var disposer = imageBrush.ImageSource as IDisposable;
            if(disposer != null)disposer.Dispose();
            
            imageBrush.ImageSource = new BitmapImage(uriSource);

        }
    }
}
