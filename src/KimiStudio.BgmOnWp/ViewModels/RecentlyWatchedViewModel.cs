using System.Collections.Generic;
using System.Linq;
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
using KimiStudio.BgmOnWp.Api;
using KimiStudio.BgmOnWp.Models;

namespace KimiStudio.BgmOnWp.ViewModels
{
    public sealed class RecentlyWatchedViewModel : Screen
    {
        private readonly INavigationService navigation;
        private IEnumerable<WatchedItemModel> items;

        public RecentlyWatchedViewModel(INavigationService navigation)
        {
            this.navigation = navigation;
            items = Enumerable.Empty<WatchedItemModel>();
        }

        public IEnumerable<WatchedItemModel> Items
        {
            get { return items; }
            set
            {
                items = value;
                NotifyOfPropertyChange(() => Items);
            }
        }

        public void OnTap(WatchedItemModel item)
        {
            navigation.UriFor<SubjectViewModel>()
                .WithParam(x => x.Id,item.Id)
                .Navigate();
        }

        protected override void OnViewLoaded(object view)
        {
            base.OnViewLoaded(view);
            base.OnViewLoaded(view);
            var loginCommand = new LoginCommand("kiminozo", "haruka", LoginCallBack);
            loginCommand.Execute();
        }

        private void LoginCallBack()
        {
            var getWatchedCommand = new GetWatchedCommand(WatchedCallBack);
            getWatchedCommand.Execute();
        }

        private void WatchedCallBack(IList<BagumiData> list)
        {
            Items = list.OrderByDescending(p => p.LastTouch)
                .Take(8)
                .Select(p => new WatchedItemModel
                                 {
                                     Id = p.Subject.Id,
                                     Name = p.Name,
                                     UriSource = p.Subject.Images.Large
                                 });
        }
    }
}
