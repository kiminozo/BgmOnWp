using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Interactivity;

namespace KimiStudio.Controls.Behaviors
{
    public sealed class ScrollViewerScrollCommandBehavior : Behavior<ListBox>
    {

        public static readonly DependencyProperty ScrollTopCommandProperty =
            DependencyProperty.Register("ScrollTopCommand", typeof(ICommand), typeof(ScrollViewerScrollCommandBehavior), new PropertyMetadata(default(ICommand)));

        public ICommand ScrollTopCommand
        {
            get { return (ICommand)GetValue(ScrollTopCommandProperty); }
            set { SetValue(ScrollTopCommandProperty, value); }
        }

        public static readonly DependencyProperty ScrollFootCommandProperty =
        DependencyProperty.Register("ScrollFootCommand", typeof(ICommand), typeof(ScrollViewerScrollCommandBehavior), new PropertyMetadata(default(ICommand)));

        public ICommand ScrollFootCommand
        {
            get { return (ICommand)GetValue(ScrollFootCommandProperty); }
            set { SetValue(ScrollFootCommandProperty, value); }
        }

        private ScrollViewer scrollViewer;

        protected override void OnAttached()
        {
            base.OnAttached();
            AssociatedObject.Loaded += AssociatedObject_Loaded;
        }



        protected override void OnDetaching()
        {
            base.OnDetaching();
            if (scrollViewer == null) return;
            scrollViewer.ManipulationCompleted -= ScrollViewer_ManipulationCompleted;
            scrollViewer = null;
        }

        void AssociatedObject_Loaded(object sender, RoutedEventArgs e)
        {
             scrollViewer = AssociatedObject.GetFirstLogicalChildByType<ScrollViewer>(true);
            if (scrollViewer == null) return;
            scrollViewer.ManipulationCompleted += ScrollViewer_ManipulationCompleted;
        }

        private void ScrollViewer_ManipulationCompleted(object sender, ManipulationCompletedEventArgs e)
        {
            Debug.WriteLine("VerticalOffset={0}", scrollViewer.VerticalOffset);
            Debug.WriteLine("ScrollableHeight={0}", scrollViewer.ScrollableHeight);

            if (scrollViewer.ScrollableHeight <= 0) return;

            if (scrollViewer.VerticalOffset < 1.0)
            {
                var command = ScrollTopCommand;
                if (command != null && command.CanExecute(null))
                {
                    command.Execute(null);
                }
            }

            if ((scrollViewer.ScrollableHeight - scrollViewer.VerticalOffset) < 1.0)
            {

                var command = ScrollFootCommand;
                if (command != null && command.CanExecute(null))
                {
                    command.Execute(null);
                }
            }
        }

    }
}