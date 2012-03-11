using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;

namespace KimiStudio.BgmOnWp.Views
{
    public partial class MainPage
    {
        private readonly Color backColor = Color.FromArgb(0, 0, 0, 0);

        // 构造函数
        public MainPage()
        {
            InitializeComponent();
            //InitializePanoramaIndex();
        }

        private void InitializePanoramaIndex()
        {

            int start = 2;
            if (start <= 0 || start >= panorama.Items.Count) return;
            var items = panorama.Items;
            for (int i = 0; i < start; i++)
            {
                var tmp = items[0];
                items.RemoveAt(0);
                items.Add(tmp);
            }
        }

        private void OnAppBarStateChanged(object sender, ApplicationBarStateChangedEventArgs e)
        {
            ApplicationBar.BackgroundColor = e.IsMenuVisible ? backColor : Colors.Transparent;
        }
    }
}