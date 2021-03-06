﻿using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;

namespace KimiStudio.Controls
{
    public class TabSwitch : ItemsControl
    {
        #region Propertys

        public static readonly DependencyProperty SelectedWidthProperty =
            DependencyProperty.Register("SelectedWidth", typeof (double), typeof (TabSwitch),
                                        new PropertyMetadata(65.0, SelectedWidthPropertyChanged));

        public double SelectedWidth
        {
            get { return (double)GetValue(SelectedWidthProperty); }
            set { SetValue(SelectedWidthProperty, value); }
        }

        public static readonly DependencyProperty SelectedItemProperty =
            DependencyProperty.Register("SelectedItem", typeof(TabSwitchItem), typeof(TabSwitch),
                                        new PropertyMetadata(null));

        public TabSwitchItem SelectedItem
        {
            get { return (TabSwitchItem)GetValue(SelectedItemProperty); }
            private set { SetValue(SelectedItemProperty, value); }
        }

        public static readonly DependencyProperty SelectedValueProperty =
            DependencyProperty.Register("SelectedValue", typeof(object), typeof(TabSwitch),
                                        new PropertyMetadata(null));

        public object SelectedValue
        {
            get { return GetValue(SelectedValueProperty); }
            private set { SetValue(SelectedValueProperty, value); }
        }

        public static readonly DependencyProperty SelectedIndexProperty =
            DependencyProperty.Register("SelectedIndex", typeof(int), typeof(TabSwitch),
                                        new PropertyMetadata(0, SelectedIndexPropertyChanged));

        public int SelectedIndex
        {
            get { return (int)GetValue(SelectedIndexProperty); }
            set { SetValue(SelectedIndexProperty, value); }
        }

        public static readonly DependencyProperty SelectedBackgroundProperty =
            DependencyProperty.Register("SelectedBackground", typeof(Brush), typeof(TabSwitch),
                                        new PropertyMetadata(new SolidColorBrush(Colors.Red),
                                                             SelectedBackgroundPropertyChanged));

        public Brush SelectedBackground
        {
            get { return (Brush)GetValue(SelectedBackgroundProperty); }
            set { SetValue(SelectedBackgroundProperty, value); }
        }

        private static void SelectedBackgroundPropertyChanged(DependencyObject o, DependencyPropertyChangedEventArgs e)
        {
            var sender = o as TabSwitch;
            if (sender == null || e.NewValue == e.OldValue) return;
            sender.SetSelectedBackground(e.NewValue as Brush);
        }

        private static void SelectedIndexPropertyChanged(DependencyObject o, DependencyPropertyChangedEventArgs e)
        {
            var sender = o as TabSwitch;
            if (sender == null || e.NewValue == e.OldValue) return;

            sender.SetSelectedIndex((int)e.NewValue);
        }

        private static void SelectedWidthPropertyChanged(DependencyObject o, DependencyPropertyChangedEventArgs e)
        {
            var sender = o as TabSwitch;
            if (sender == null || e.NewValue == e.OldValue) return;

            sender.SetSelectedWidth((double)e.NewValue);
        }


        #endregion



        private Rectangle rectangle;
        public event RoutedEventHandler Selected;

        private void OnSelected(RoutedEventArgs e)
        {
            RoutedEventHandler handler = Selected;
            if (handler != null) handler(this, e);
        }


        private void ChangeSelected(int index)
        {
            if (index < 0 || index > Items.Count) return;
            ;
            SelectedIndex = index;
            var switchItem = Items[index] as TabSwitchItem;
            if (switchItem != null)
            {
                SelectedItem = switchItem;
                SelectedValue = switchItem.Content;
            }
            else
            {
                SelectedValue = Items[index];
            }
            OnSelected(new RoutedEventArgs());
        }

        public TabSwitch()
        {
            DefaultStyleKey = typeof(TabSwitch);
        }

        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();
            rectangle = (Rectangle)GetTemplateChild("rectangle");
            SetSelectedBackground(SelectedBackground);
            SetSelectedWidth(SelectedWidth);
            SetSelectedIndex(SelectedIndex);
        }

        private void SetSelectedWidth(double itemWidth)
        {
            if (rectangle == null) return;
            rectangle.Width = itemWidth;
        }

        private void SetSelectedBackground(Brush brush)
        {
            if (rectangle == null) return;
            rectangle.Fill = brush;
        }

        private void SetSelectedIndex(int index)
        {
            if (rectangle == null) return;
            if (index < 0 || index > Items.Count) return;

            double x = SelectedWidth*index;
            var compositeTransform = (CompositeTransform) rectangle.RenderTransform;
            if (Math.Abs(compositeTransform.TranslateX - x) > 1)
                compositeTransform.TranslateX = SelectedWidth*index;
            // ShowAnimation(SelectedWidth * index);
        }

        protected override void OnTap(GestureEventArgs e)
        {
            Point point = e.GetPosition(this);

            var x = (int) (point.X/SelectedWidth)*SelectedWidth;
            int index = (int) (x/SelectedWidth);
            ShowAnimation(x);
            ChangeSelected(index);
            base.OnTap(e);
        }

        private void ShowAnimation(double x)
        {
            var animation = new DoubleAnimation
                                {
                                    Duration = new Duration(new TimeSpan(0, 0, 0, 0, 500)),
                                    To = x,
                                    EasingFunction =
                                        new ElasticEase { EasingMode = EasingMode.EaseOut, Oscillations = 1, Springiness = 5 }
                                };
            Storyboard.SetTarget(animation, rectangle);
            Storyboard.SetTargetProperty(animation, new PropertyPath("(UIElement.RenderTransform).(CompositeTransform.TranslateX)"));
            var storyboard = new Storyboard();
            storyboard.Children.Add(animation);
            storyboard.Begin();
        }
    }

}
