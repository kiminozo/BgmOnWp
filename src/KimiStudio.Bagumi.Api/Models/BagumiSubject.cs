using System;
using Newtonsoft.Json;

namespace KimiStudio.Bangumi.Api.Models
{
    public class BagumiSubject
    {
        [JsonProperty("air_date")]
        public string AirDate { get; set; }

        [JsonProperty("air_weekday")]
        public int AirWeekday { get; set; }

        public BagumiSubjectTap Collection { get; set; }
        public int Eps { get; set; }
        public int Id { get; set; }
        public ImageData Images { get; set; }
        public string Name { get; set; }

        [JsonProperty("name_cn")]
        public string NameCn { get; set; }

        public string Subject { get; set; }
        public int Type { get; set; }
        public Uri Url { get; set; }
    }
}