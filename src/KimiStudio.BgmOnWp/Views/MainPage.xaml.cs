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
        }

        private void OnAppBarStateChanged(object sender, ApplicationBarStateChangedEventArgs e)
        {
            ApplicationBar.BackgroundColor = e.IsMenuVisible ? backColor : Colors.Transparent;
        }
    }
}