using System;
using System.Collections.Generic;
using KimiStudio.Bangumi.Api.Models;

namespace KimiStudio.BgmOnWp.Models
{
    public class StaffModel
    {
        public Uri StaffImage { get; set; }
        public string StaffName { get; set; }
        public string Job { get; set; }
        public Uri RemoteUrl { get; set; }

        public static StaffModel FromStaffItem(StaffItem staffItem)
        {
            return new StaffModel
                       {
                           Job = GetJob(staffItem.Jobs),
                           RemoteUrl = staffItem.Url,
                           StaffImage = ImageUri(staffItem.Images),
                           StaffName = staffItem.Name
                       };
        }

        private static string GetJob(IList<string> jobs)
        {
            if (jobs == null || jobs.Count < 1) return string.Empty;
            return jobs[0];
        }

        private static Uri ImageUri(ImageData image)
        {
            return image != null ? image.Grid : new Uri("/Images/info_only_m.png", UriKind.Relative);
        }
    }
}