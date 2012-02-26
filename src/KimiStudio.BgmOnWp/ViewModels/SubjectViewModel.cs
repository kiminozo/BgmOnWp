using System;
using Caliburn.Micro;
using KimiStudio.BgmOnWp.Api;
using KimiStudio.BgmOnWp.ModelMessages;
using KimiStudio.BgmOnWp.Models;
using KimiStudio.BgmOnWp.Toolkit;

namespace KimiStudio.BgmOnWp.ViewModels
{
    public class SubjectViewModel : Screen
    {
        private readonly IProgressService progressService;

        public int Id { get; set; }

        #region Property
        private string name;
        private string cnName;
        private Uri uriSource;
        private string summary;

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

        public string Summary
        {
            get { return summary; }
            set
            {
                summary = value;
                NotifyOfPropertyChange(() => Summary);
            }
        }

        #endregion


        public SubjectViewModel(IProgressService progressService)
        {
            this.progressService = progressService;
        }

        protected override void OnViewLoaded(object view)
        {
            base.OnViewLoaded(view);

            UriSource = UriSource;

            progressService.Show("加载中\u2026");
            var command = new GetSubjectCommand(Id);
            command.Execute(Handle);
        }

        private void Handle(SubjectMessage message)
        {
            progressService.Hide();

            if (message.Cancelled) return;
            DisplayName = message.Subject.Name;
            Name = message.Subject.Name;
            CnName = message.Subject.NameCn;
            UriSource = message.Subject.Images.Large;
            Summary = message.Subject.Summary;
        }
    }
}
