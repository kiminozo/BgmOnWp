using System;

namespace KimiStudio.BgmOnWp.Models
{
    public class BagumiSubject
    {
        public DateTime AirDate { get; set; }
        public int AirWeekday { get; set; }
        public BagumiSubjectTap Collection { get; set; }
        public int Eps { get; set; }
        public string Id { get; set; }
        public ImageData Images { get; set; }
        public string Name { get; set; }
        public string NameCn { get; set; }
        public string Subject { get; set; }
        public int Type { get; set; }
        public Uri Url { get; set; }
    }
}