using System.Collections.Generic;
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
using KimiStudio.BgmOnWp.Models;
using Microsoft.Phone.Tasks;

namespace KimiStudio.BgmOnWp.ViewModels
{
    public class AboutViewModel : Screen
    {
        private static readonly IList<SoftStaffModel> StaffList = new List<SoftStaffModel> { SoftStaffModel.Kiminozo, SoftStaffModel.Sai };

        public IList<SoftStaffModel> Staff
        {
            get { return StaffList; }
        }

        public void TapItem(SoftStaffModel staffModel)
        {
            if (staffModel.Url == null) return;

            var task = new WebBrowserTask { Uri = staffModel.Url };
            task.Show();
        }
    }
}
