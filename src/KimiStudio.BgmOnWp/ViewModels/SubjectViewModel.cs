﻿using System;
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
using KimiStudio.BgmOnWp.Toolkit;
using KimiStudio.Controls;

namespace KimiStudio.BgmOnWp.ViewModels
{
    public class SubjectViewModel : Screen
    {
        private readonly IProgressService progressService;

        public int Id { get; set; }

        public SubjectSummaryViewModel SubjectSummary { get; set; }

        public SubjectViewModel(IProgressService progressService)
        {
            this.progressService = progressService;
            SubjectSummary = new SubjectSummaryViewModel();
        }

        protected override void OnViewLoaded(object view)
        {
            base.OnViewLoaded(view);
            progressService.Show("加载中\u2026");
            var command = new GetSubjectCommand(Id, SubjectCallBack);
            command.Execute();
        }

        private void SubjectCallBack(Subject subject)
        {
            DisplayName = subject.Name;
            SubjectSummary.SetSubject(subject);
            progressService.Hide();
        }
    }

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
