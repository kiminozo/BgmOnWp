using System;
using KimiStudio.Bangumi.Api.Models;

namespace KimiStudio.BgmOnWp.Models
{
    public class SubjectSummaryModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string CnName { get; set; }
        public Uri UriSource { get; set; }

        public static SubjectSummaryModel FromBagumiData(SubjectSummary subject)
        {
            return new SubjectSummaryModel
                       {
                           Id = subject.Id,
                           Name = subject.Name,
                           CnName = subject.NameCn,
                           UriSource = subject.Images != null ? subject.Images.Large : null
                       };
        }

        public static SubjectSummaryModel FromBagumiDataSmall(SubjectSummary subject)
        {
            return new SubjectSummaryModel
            {
                Id = subject.Id,
                Name = subject.Name,
                CnName = subject.NameCn,
                UriSource = subject.Images != null ? subject.Images.Small : null
            };
        }
    }
}