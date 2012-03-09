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
using KimiStudio.Bangumi.Api.Models;

namespace KimiStudio.BgmOnWp.Models
{
    public class BlogModel
    {
        public Uri Image { get; set; }
        public Uri Url { get; set; }
        public string Title { get; set; }
        public string Subtitle { get; set; }
        public string Summary { get; set; }

        public static BlogModel FromBlog(Blog blog)
        {
            return new BlogModel
                       {
                           Image = GetImageUri(blog.User.Avatar),
                           Title = blog.Title,
                           Subtitle = GetSubtitle(blog.User, blog.Dateline),
                           Summary = blog.Summary,
                           Url = blog.Url
                       };
        }

        private static string GetSubtitle(User user,DateTime dateline)
        {
            if (user == null) return null;
            string name = string.IsNullOrEmpty(user.NickName) ? user.UserName : user.NickName;
            return string.Format("by {0} {1}", name, dateline.ToString("yyyy-MM-dd HH:mm"));
        }

        private static readonly Uri DefaultUri = new Uri(@"http://lain.bgm.tv/pic/user/l/icon.jpg");
        private static Uri GetImageUri(Avatar avatar)
        {
            if (avatar != null && avatar.Large != null
                && avatar.Large != DefaultUri)
            {
                return avatar.Large;
            }
            return new Uri("/Images/usericon.png", UriKind.Relative);
        }
    }
}
