using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using KimiStudio.Bangumi.Api.Commands;
using KimiStudio.Bangumi.Api.Models;
using KimiStudio.BgmOnWp.Models;
using Caliburn.Micro;
using KimiStudio.BgmOnWp.Toolkit;
using Microsoft.Expression.Interactivity.Core;

namespace KimiStudio.BgmOnWp.ViewModels
{
    public class SearchViewModel : Screen
    {
        private string keyword;
        private string oldKeyword;
        private int resultCount;
        private ObservableCollection<SubjectSummaryModel> results = new BindableCollection<SubjectSummaryModel>();

        public ObservableCollection<SubjectSummaryModel> Results
        {
            get { return results; }
            set
            {
                results = value;
                NotifyOfPropertyChange(() => Results);
            }
        }

        public string Keyword
        {
            get { return keyword; }
            set
            {
                keyword = value;
                NotifyOfPropertyChange(() => Keyword);
            }
        }

        public ICommand SearchMoreResult { get; private set; }


        private readonly INavigationService navigation;
        private readonly IProgressService progressService;
        private readonly IPromptManager promptManager;

        public SearchViewModel(INavigationService navigation, IProgressService progressService, IPromptManager promptManager)
        {
            this.navigation = navigation;
            this.progressService = progressService;
            this.promptManager = promptManager;

            SearchMoreResult = new ActionCommand(MoreResult);
        }

        protected override void OnDeactivate(bool close)
        {
            base.OnDeactivate(close);
            progressService.Hide();
        }

        private bool inSearch;

        private void MoreResult()
        {
            if (inSearch) return;

            if (string.IsNullOrEmpty(Keyword) || Keyword != oldKeyword) return;
            if (Results == null || Results.Count == resultCount) return;

            DoSearch(Results.Count);
        }

        public void Search()
        {
            if (string.IsNullOrEmpty(Keyword)) return;
            if (inSearch) return;
            oldKeyword = Keyword;
            Results.Clear();
            DoSearch();
        }

        public void DoSearch(int start = 0)
        {
            inSearch = true;
            progressService.Show("搜索中\u2026");
            var task = CommandTaskFactory.Create(new SearchSubjectCommand(oldKeyword, start: start));
            task.Result(result =>
                            {
                                if(result.Results > 0 && result.List != null && result.List.Count > 0)
                                {
                                    result.List
                                        .Select(SubjectSummaryModel.FromBagumiData)
                                        .Apply(x => Results.Add(x));
                                    resultCount = result.Results;
                                }
                                else
                                {
                                    resultCount = 0;    
                                }
                                progressService.Hide();
                                inSearch = false;
                            });
            task.Exception(err =>
                               {
                                   progressService.Hide();
                                   if (err is ApiException)
                                   {
                                       promptManager.ToastWarn("没有搜索到结果");
                                   }
                                   else
                                   {
                                       promptManager.ToastError(err);
                                   }
                                   inSearch = false;
                               });
            task.Start();
        }

        public void TapItem(SubjectSummaryModel subject)
        {
            navigation.UriFor<SubjectViewModel>()
                .WithParam(x => x.Id, subject.Id)
                .WithParam(x => x.DisplayName, subject.Name)
                .WithParam(x => x.UriSource, subject.Image)
                .Navigate();
        }
    }
}
