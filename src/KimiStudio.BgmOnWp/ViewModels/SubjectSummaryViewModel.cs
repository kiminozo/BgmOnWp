using System;
using Caliburn.Micro;
using KimiStudio.BgmOnWp.Models;

namespace KimiStudio.BgmOnWp.ViewModels
{
    public class SubjectSummaryViewModel : Screen
    {
        #region Property
        private string name;
        private string cnName;
        private Uri uriSource;

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

        public Uri UriSource
        {
            get { return uriSource; }
            set
            {
                uriSource = value;
                NotifyOfPropertyChange(() => UriSource);
            }
        }
        #endregion

        public void SetSubject(Subject subject)
        {
            Name = subject.Name;
            CnName = subject.NameCn;
            UriSource = subject.Images.Large;
        }
    }
}