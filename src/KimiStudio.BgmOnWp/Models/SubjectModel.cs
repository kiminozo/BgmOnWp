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
using KimiStudio.Bangumi.Api.Models;
using KimiStudio.BgmOnWp.Storages;

namespace KimiStudio.BgmOnWp.Models
{
    public class SubjectModel : PropertyChangedBase
    {
        private string name;
        private string cnName;
        private string doing;
        private bool isUnPined = true;
        private Uri imageSource;
        private Uri largeImageSource;
        private string summary;

        public int Id { get; set; }

        public string Name
        {
            get { return name; }
            set
            {
                name = value;
                NotifyOfPropertyChange(() => Name);
            }
        }

        public string CnName
        {
            get { return cnName; }
            set
            {
                cnName = value;
                NotifyOfPropertyChange(() => CnName);
            }
        }

        public Uri ImageSource
        {
            get { return imageSource; }
            set
            {
                imageSource = value;
                NotifyOfPropertyChange(() => ImageSource);
            }
        }

        
        public Uri LargeImageSource
        {
            get { return largeImageSource; }
            set
            {
                largeImageSource = value;
                NotifyOfPropertyChange(() => LargeImageSource);
            }
        }

        public string Summary
        {
            get { return summary; }
            set
            {
                summary = value;
                NotifyOfPropertyChange(() => Summary);
            }
        }

        public string Doing
        {
            get { return doing; }
            set
            {
                doing = value;
                NotifyOfPropertyChange(() => Doing);
            }
        }

        public bool IsUnPined
        {
            get { return isUnPined; }
            set
            {
                isUnPined = value;
                NotifyOfPropertyChange(() => IsUnPined);
            }
        }

        public static SubjectModel FromSubject(Subject subject)
        {
            var result = new SubjectModel
                       {
                           Id = subject.Id,
                           Name = subject.Name,
                           CnName = subject.NameCn,
                           ImageSource = subject.Images != null ? subject.Images.Common : null,
                           LargeImageSource = subject.Images != null ? subject.Images.Large : null,
                           Summary = subject.Summary,
                           Doing = subject.Collection != null ? string.Format("{0}人在看", subject.Collection.Doing) : null,
                       };
            result.IsUnPined = !result.IsPined();
            return result;
        }
    }
}
