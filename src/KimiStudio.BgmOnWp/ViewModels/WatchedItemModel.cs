using System;
using KimiStudio.BgmOnWp.Models;

namespace KimiStudio.BgmOnWp.ViewModels
{
    public class WatchedItemModel
    {
        public string Id { get; set; }
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