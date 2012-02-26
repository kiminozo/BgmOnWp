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
using KimiStudio.BgmOnWp.ViewModels;

namespace KimiStudio.BgmOnWp.Storages
{
    public class RecentlyWatchedViewModelStorage : StorageHandler<MainPageViewModel>
    {

        public override void Configure()
        {
            Id(x => x.GetType());
            Property(x => x.Items)
                .InPhoneState()
                .RestoreAfterViewReady();
        }
    }
}
