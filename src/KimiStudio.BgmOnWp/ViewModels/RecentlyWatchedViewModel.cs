using System;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using Caliburn.Micro;

namespace KimiStudio.BgmOnWp.ViewModels
{
    public sealed class RecentlyWatchedViewModel : Screen
    {
        public IObservableCollection<WatchedModel> Items { get; private set; }

        public RecentlyWatchedViewModel()
        {
            Items = new BindableCollection<WatchedModel>();
            for (int i = 0; i < 8; i++)
            {
                Items.Add(new WatchedModel { Name = "日常", UriSource = new Uri(@"http://lain.bgm.tv/pic/cover/c/fb/94/23640_T3pB2.jpg",UriKind.Absolute) });    
            }
        }
    }

    public class WatchedModel
    {
        public string Name { get; set; }
        public Uri UriSource { get; set; }
    }
}
