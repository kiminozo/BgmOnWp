using System;
using KimiStudio.Bagumi.Api.Models;

namespace KimiStudio.BgmOnWp.Models
{
    public class WatchedItemModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public Uri UriSource { get; set; }

        public static WatchedItemModel FromBagumiData(BagumiSubject subject)
        {
            return new WatchedItemModel
                {
                    Id = subject.Id,
                    Name = subject.Name,
                    UriSource = subject.Images.Large
                };
        }
    }
}