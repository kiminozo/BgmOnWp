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
    public sealed class WatchingsViewModel : Conductor<IScreen>.Collection.OneActive
    {
        private readonly INavigationService navigation;

        public WatchingsViewModel(INavigationService navigation)
        {
            this.navigation = navigation;
            Items.Add(new WatchingsItemViewModel { DisplayName = "全部" });
            Items.Add(new WatchingsItemViewModel { DisplayName = "动画" });
            Items.Add(new WatchingsItemViewModel { DisplayName = "三次元" });
            ActivateItem(Items[0]);
        }

        protected override void OnInitialize()
        {
          
        }

        public void OnTap(WatchedItemModel item)
        {
            navigation.UriFor<SubjectViewModel>()
                .WithParam(x => x.Id, item.Id)
                .Navigate();
        }
    }
}
