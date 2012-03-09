using System;
using System.Collections.Generic;
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

namespace KimiStudio.BgmOnWp.ViewModels
{
    public class SearchViewModel:Screen
    {
        private string keyword;
        private IEnumerable<SubjectSummaryModel> results;

        public IEnumerable<SubjectSummaryModel> Results
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


        private readonly INavigationService navigation;
        private readonly IProgressService progressService;
        private readonly IPromptManager promptManager;

        public SearchViewModel(INavigationService navigation, IProgressService progressService, IPromptManager promptManager)
        {
            this.navigation = navigation;
            this.progressService = progressService;
            this.promptManager = promptManager;
        }

        private bool inSearch;

        public void Search()
        {
            if(string.IsNullOrEmpty(Keyword))return;
            if (inSearch)return;

            inSearch = true;
            progressService.Show("搜索中\u2026");
            var task = CommandTaskFactory.Create(new SearchSubjectCommand(Keyword));
            task.Result(result =>
                            {
                                Results = result.Results > 0
                                              ? result.List.Select(SubjectSummaryModel.FromBagumiData)
                                              : Enumerable.Empty<SubjectSummaryModel>();
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
                .WithParam(x => x.DisplayName,subject.Name)
                .WithParam(x => x.UriSource, subject.UriSource)
                .Navigate();
        }
    }

    //public class SearchResultModel
    //{
    //    public string Name { get; set; }
    //    public string CnName { get; set; }
    //    public int Id { get; set; }
    //    public Uri Image { get; set; }

    //    public static SearchResultModel FromSearchResult(BagumiSubject searchResult)
    //    {
    //        return new SearchResultModel
    //                   {
    //                       CnName = searchResult.List
    //                   }
    //    }
    //}
}
