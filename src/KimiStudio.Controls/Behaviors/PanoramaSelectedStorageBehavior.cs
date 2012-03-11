using System;
using System.IO.IsolatedStorage;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Interactivity;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using Microsoft.Phone.Controls;

namespace KimiStudio.Controls.Behaviors
{
    public sealed class PanoramaSelectedStorageBehavior : Behavior<Panorama>
    {

        public static readonly DependencyProperty KeyProperty =
            DependencyProperty.Register("Key", typeof(string), typeof(PanoramaSelectedStorageBehavior), new PropertyMetadata(default(string)));

        public string Key
        {
            get { return (string)GetValue(KeyProperty); }
            set { SetValue(KeyProperty, value); }
        }

        protected override void OnAttached()
        {
            base.OnAttached();
            AssociatedObject.Loaded += AssociatedObject_Loaded;
            AssociatedObject.SelectionChanged += AssociatedObject_SelectionChanged;
        }

        protected override void OnDetaching()
        {
            base.OnDetaching();
            AssociatedObject.Loaded -= AssociatedObject_Loaded;
            AssociatedObject.SelectionChanged -= AssociatedObject_SelectionChanged;
        }

        private void AssociatedObject_Loaded(object sender, RoutedEventArgs e)
        {
            if(!IsolatedStorageSettings.ApplicationSettings.Contains(Key))return;

            var start = Convert.ToInt32(IsolatedStorageSettings.ApplicationSettings[Key]);
            AssociatedObject.DefaultItem = AssociatedObject.Items[start];
        }

        private void AssociatedObject_SelectionChanged(object sender, RoutedEventArgs e)
        {
            IsolatedStorageSettings.ApplicationSettings[Key] = AssociatedObject.SelectedIndex;
        }


    }
}
