using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace KimiStudio.Bagumi.Api.Models
{
    public class Calendar
    {
        public List<BagumiSubject> Items { get; set; }
        public WeekDay WeekDay { get; set; }
    }

    public class WeekDay
    {
        public int Id { get; set; }
        public string Cn { get; set; }
        public string En { get; set; }
        public int Jp { get; set; }
    }
}
