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
        public Uri UriSource { get; set; }

        public SubjectSummaryViewModel SubjectSummary { get; set; }

        public SubjectViewModel(IProgressService progressService)
        {
            this.progressService = progressService;
            SubjectSummary = new SubjectSummaryViewModel();
        }

        protected override void OnViewLoaded(object view)
        {
            base.OnViewLoaded(view);

            SubjectSummary.UriSource = UriSource;

            progressService.Show("加载中\u2026");
            var command = new GetSubjectCommand(Id);
            command.Execute(Handle);
        }

        private void Handle(SubjectMessage message)
        {
            if (message.Cancelled) return;
            DisplayName = message.Subject.Name;
            SubjectSummary.SetSubject(message.Subject);
            progressService.Hide();
        }
    }
}
