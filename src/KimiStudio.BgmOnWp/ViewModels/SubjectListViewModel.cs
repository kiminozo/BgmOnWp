using System;
using System.Collections.Generic;
using System.Linq;
using Caliburn.Micro;
using KimiStudio.Bangumi.Api.Models;
using KimiStudio.BgmOnWp.Models;

namespace KimiStudio.BgmOnWp.ViewModels
{
    public class SubjectListViewModel : Screen
    {
        private IEnumerable<SubjectSummaryModel> watchingItems;

        public IEnumerable<SubjectSummaryModel> WatchingItems
        {
            get { return watchingItems; }
            set
            {
                watchingItems = value;
                NotifyOfPropertyChange(() => WatchingItems);
            }
        }

        public Func<SubjectSummary, bool> Filter { get; set; }

        public void UpdateWatchingItems(IEnumerable<SubjectSummary> itemModels)
        {
            var query = Filter == null ? itemModels : itemModels.Where(Filter);
            WatchingItems = query.Select(SubjectSummaryModel.FromBagumiData);
        }

    }
}