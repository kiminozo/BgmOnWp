using System;
using System.IO;
using System.IO.IsolatedStorage;
using System.Linq;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using KimiStudio.Bangumi.Api.Models;
using KimiStudio.BgmOnWp.Storages;
using KimiStudio.Controls;
using Microsoft.Expression.Interactivity.Core;
using Microsoft.Phone.Shell;

namespace KimiStudio.BgmOnWp.Models
{
    public class SubjectSummaryModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string CnName { get; set; }
        public string Doing { get; set; }
        public Uri Image { get; set; }
        public Uri SmallImage { get; set; }
        public Uri LargeImage { get; set; }
        public Uri RemoteUrl { get; set; }

        public static SubjectSummaryModel FromBagumiData(SubjectSummary subject)
        {
            return new SubjectSummaryModel
            {
                Id = subject.Id,
                Name = subject.Name,
                CnName = subject.NameCn,
                Doing = subject.Collection != null ? string.Format("{0}»À‘⁄ø¥", subject.Collection.Doing) : null,
                Image = subject.Images != null ? subject.Images.Common : null,
                SmallImage = subject.Images != null ? subject.Images.Small : null,
                LargeImage = subject.Images != null ? subject.Images.Large : null,
                RemoteUrl = subject.Url,
            };
        }

        public SubjectSummaryModel()
        {
            Pin = new ActionCommand(OnPin);
        }



        public ICommand Pin { get; private set; }

        public void OnPin()
        {
           this.PinTile();
        }

    }
}