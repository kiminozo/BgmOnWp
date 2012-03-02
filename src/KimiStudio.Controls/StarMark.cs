using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace KimiStudio.Controls
{
    public class StarMark : Control
    {
        private ItemsControl stars;
        private const int StarSize = 34;
        private ObservableCollection<StarItem> starItems;

        #region DependencyPropertys
        public static readonly DependencyProperty MarkedFillProperty =
           DependencyProperty.Register("MarkedFill", typeof(Brush), typeof(StarMark),
           new PropertyMetadata(new SolidColorBrush(Colors.Red)));

        public static readonly DependencyProperty UnMarkedFillProperty =
            DependencyProperty.Register("UnMarkedFill", typeof(Brush), typeof(StarMark),
            new PropertyMetadata(new SolidColorBrush(Colors.DarkGray)));

        public static readonly DependencyProperty MaxStarsProperty =
            DependencyProperty.Register("MaxStars", typeof(int), typeof(StarMark),
            new PropertyMetadata(10));

        public static readonly DependencyProperty MarkedProperty =
            DependencyProperty.Register("Marked", typeof(int), typeof(StarMark),
            new PropertyMetadata(0, OnMarkedPropertyChanged));

        public int Marked
        {
            get { return (int)GetValue(MarkedProperty); }
            set { SetValue(MarkedProperty, value); }
        }

        public int MaxStars
        {
            get { return (int)GetValue(MaxStarsProperty); }
            set { SetValue(MaxStarsProperty, value); }
        }

        public Brush UnMarkedFill
        {
            get { return (Brush)GetValue(UnMarkedFillProperty); }
            set { SetValue(UnMarkedFillProperty, value); }
        }

        public Brush MarkedFill
        {
            get { return (Brush)GetValue(MarkedFillProperty); }
            set { SetValue(MarkedFillProperty, value); }
        }

        private static void OnMarkedPropertyChanged(DependencyObject o, DependencyPropertyChangedEventArgs args)
        {
            var starMark = o as StarMark;
            if (starMark == null || args.NewValue == args.OldValue) return;

            starMark.SetMarked((int)args.NewValue);
        }

        #endregion

        public StarMark()
        {
            this.DefaultStyleKey = typeof(StarMark);
        }

        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();
            stars = (ItemsControl)GetTemplateChild("stars");


            starItems = new ObservableCollection<StarItem>();
            for (int i = 0, length = MaxStars; i < length; i++)
            {
                starItems.Add(new StarItem { Fill = UnMarkedFill });
            }
            stars.ItemsSource = starItems;
            SetMarked(Marked);
        }

        private void SetMarked(int value)
        {
            if (stars == null) return; ;
            FillStars(value - 1);
        }

        protected override void OnManipulationStarted(ManipulationStartedEventArgs e)
        {
            e.Handled = true;
            UpdateStar(e.ManipulationOrigin);
            base.OnManipulationStarted(e);
        }

        protected override void OnManipulationDelta(ManipulationDeltaEventArgs e)
        {
            e.Handled = true;
            UpdateStar(e.ManipulationOrigin);
            base.OnManipulationDelta(e);
        }

        protected override void OnManipulationCompleted(ManipulationCompletedEventArgs e)
        {
            e.Handled = true;
            var index = UpdateStar(e.ManipulationOrigin);
            if (VerifyValue(index)) { Marked = index + 1; }
            base.OnManipulationCompleted(e);
        }

        private int UpdateStar(Point point)
        {
            int x = (int)point.X;
            int index = x / StarSize;
            FillStars(index);
            return index;
        }

        //protected override void OnTap(GestureEventArgs e)
        //{
        //    int x = (int)e.GetPosition(stars).X;
        //    int index = x / StarSize;
        //    FillStars(index);

        //    if (VerifyValue(index)) { Marked = index + 1; }
        //    base.OnTap(e);
        //}


        //protected override void OnMouseLeftButtonUp(MouseButtonEventArgs e)
        //{
        //    int x = (int)e.GetPosition(stars).X;
        //    int index = x / StarSize;
        //    FillStars(index);

        //    if (VerifyValue(index)) { Marked = index + 1; }
        //    base.OnMouseLeftButtonUp(e);
        //}

        //protected override void OnMouseMove(MouseEventArgs e)
        //{
        //    int x = (int)e.GetPosition(stars).X;
        //    int index = x / starSize;
        //    FillStar(index,Colors.Yellow);
        //    base.OnMouseMove(e);
        //}
        private bool VerifyValue(int index)
        {
            if (index < 0 || index >= stars.Items.Count) return false;
            return true;
        }


        private void FillStars(int index)
        {
            if (!VerifyValue(index)) return;

            //var starlist = stars.Items.Cast<Shape>().ToArray();
            for (int i = 0, length = starItems.Count; i < length; i++)
            {
                var star = starItems[i];
                if (i > index)
                {
                    star.Fill = UnMarkedFill;
                }
                else
                {
                    star.Fill = MarkedFill;
                }
            }

        }




        public class StarItem : INotifyPropertyChanged
        {
            private Brush fill;

            public event PropertyChangedEventHandler PropertyChanged;

            public Brush Fill
            {
                get { return fill; }
                set
                {
                    if (fill == value) return;
                    fill = value;
                    OnPropertyChanged("Fill");
                }
            }

            private void OnPropertyChanged(string propertyName)
            {
                var handler = PropertyChanged;
                if (handler == null) return;
                handler(this, new PropertyChangedEventArgs(propertyName));
            }
        }
    }
}
