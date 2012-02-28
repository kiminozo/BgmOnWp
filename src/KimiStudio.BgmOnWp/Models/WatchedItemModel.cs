using System;
using KimiStudio.Bagumi.Api.Models;

namespace KimiStudio.BgmOnWp.Models
{
    public class WatchedItemModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public Uri UriSource { get; set; }

        public static WatchedItemModel FromBagumiData(BagumiData data)
        {
            return new WatchedItemModel
                {
                    Id = data.Subject.Id,
                    Name = data.Name,
                    UriSource = data.Subject.Images.Large
                };
        }
    }
}