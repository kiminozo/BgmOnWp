using System;
using KimiStudio.Bangumi.Api.Models;

namespace KimiStudio.BgmOnWp.Models
{
    public class SubjectSummaryModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string CnName { get; set; }
        public string Doing { get; set; }
        public Uri UriSource { get; set; }

        public static SubjectSummaryModel FromBagumiData(SubjectSummary subject,Func<ImageData,Uri> func)
        {
            return new SubjectSummaryModel
            {
                Id = subject.Id,
                Name = subject.Name,
                CnName = subject.NameCn,
                Doing = subject.Collection != null ? string.Format("{0}»À‘⁄ø¥", subject.Collection.Doing) : null,
                UriSource = subject.Images != null ? func(subject.Images) : null
            };
        }


      
    }
}