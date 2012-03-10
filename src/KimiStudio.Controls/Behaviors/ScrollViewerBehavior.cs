using System;
using System.Diagnostics;
using System.Globalization;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;

namespace KimiStudio.Controls
{

    //ÐÞ¸Ä×Ô:http://www.cnblogs.com/jerryh/archive/2012/01/02/2310490.html

    public class ScrollViewerBehavior
    {
        public static DependencyProperty ScrollEndCommandProperty
            = DependencyProperty.RegisterAttached(
                "ScrollEndCommand", typeof (ICommand),
                typeof (ScrollViewerBehavior),
                new PropertyMetadata(null, OnCommandChanged));

        public static ICommand GetScrollEndCommand(DependencyObject obj)
        {
            return (ICommand) obj.GetValue(ScrollEndCommandProperty);
        }

        public static void SetScrollEndCommand(DependencyObject obj, ICommand value)
        {
            obj.SetValue(ScrollEndCommandProperty, value);
        }

        public static DependencyProperty ScrollTopCommandProperty
            = DependencyProperty.RegisterAttached(
                "ScrollTopCommand", typeof (ICommand),
                typeof (ScrollViewerBehavior),
                new PropertyMetadata(null, OnCommandChanged));

        public static ICommand GetScrollTopCommand(DependencyObject obj)
        {
            return (ICommand) obj.GetValue(ScrollTopCommandProperty);
        }

        public static void SetScrollTopCommand(DependencyObject obj, ICommand value)
        {
            obj.SetValue(ScrollTopCommandProperty, value);
        }


        public static void OnCommandChanged(
            DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var element = (FrameworkElement) d;
            if (element == null || !(element is ListBox)) return;
            element.Loaded -= element_Loaded;
            element.Loaded += element_Loaded;
        }

        private static void element_Loaded(object sender, RoutedEventArgs e)
        {
            var element = (FrameworkElement) sender;
            element.Loaded -= element_Loaded;

            var scrollViewer = element.GetFirstLogicalChildByType<ScrollViewer>(true);
            if (scrollViewer == null) return;
            var listener = new Listener();

            listener.Changed += (o, args) =>
                                    {


                                        Debug.WriteLine("VerticalOffset={0}", scrollViewer.VerticalOffset);
                                        Debug.WriteLine("ScrollableHeight={0}", scrollViewer.ScrollableHeight);

                                        if (scrollViewer.ScrollableHeight <= 0) return;
                                        if ((scrollViewer.ScrollableHeight - scrollViewer.VerticalOffset) < 1.0)
                                        {
                                          
                                            var endCommand = GetScrollEndCommand((FrameworkElement)sender);
                                            if (endCommand != null && endCommand.CanExecute(null))
                                            {
                                                endCommand.Execute(null);
                                            }
                                        }

                                        Debug.WriteLine(
                                            scrollViewer.VerticalOffset.ToString(CultureInfo.InvariantCulture));
                                        if (scrollViewer.VerticalOffset < 1.0)
                                        {
                                            var topCommand = GetScrollTopCommand((FrameworkElement)sender);
                                            if (topCommand != null && topCommand.CanExecute(null))
                                            {
                                                topCommand.Execute(null);
                                            }
                                        }
                                    };

            var binding = new Binding("VerticalOffset") {Source = scrollViewer};
            listener.Attach(scrollViewer, binding);
        }

    }


    internal class Listener
    {
        public readonly DependencyProperty Property;
        private ScrollViewer target;

        public Listener()
        {
            Property = DependencyProperty.RegisterAttached(
                "ListenValue",
                typeof(object),
                typeof(Listener),
                new PropertyMetadata(null, HandleValueChanged));
        }

        public event EventHandler<ValueChangedEventArgs> Changed;

        private void HandleValueChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e)
        {

            OnChanged(new ValueChangedEventArgs(e));
        }

        protected void OnChanged(ValueChangedEventArgs e)
        {

            if (Changed != null)
            {
                Changed(target, e);
            }
        }

        public void Attach(ScrollViewer element, Binding binding)
        {
            if (target != null)
            {
                throw new Exception(
                    "Cannot attach an already attached listener");
            }

            target = element;
            if (target.GetBindingExpression(Property) == null)
            {
                target.SetBinding(Property, binding);
            }

        }

        public void Detach()
        {
            target.ClearValue(Property);
            target = null;
        }

    }

    internal class ValueChangedEventArgs : EventArgs
    {
        public ValueChangedEventArgs(DependencyPropertyChangedEventArgs e)
        {
            EventArgs = e;
        }

        public DependencyPropertyChangedEventArgs EventArgs { get; private set; }
    }
}