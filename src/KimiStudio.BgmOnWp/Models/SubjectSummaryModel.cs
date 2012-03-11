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
        public Uri Image { get; set; }
        public Uri SmallImage { get; set; }
        public Uri RemoteUrl { get; set; }

        public static SubjectSummaryModel FromBagumiData(SubjectSummary subject)
        {
            return new SubjectSummaryModel
            {
                Id = subject.Id,
                Name = subject.Name,
                CnName = subject.NameCn,
                Doing = subject.Collection != null ? string.Format("{0}»À‘⁄ø¥", subject.Collection.Doing) : null,
                Image = subject.Images != null ? subject.Images.Common : null,
                SmallImage = subject.Images != null ? subject.Images.Small : null,
                RemoteUrl = subject.Url,
            };
        }


      
    }
}