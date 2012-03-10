using System;
using System.Collections.Generic;

namespace KimiStudio.Bangumi.Api.Models
{
    public class Calendar
    {
        public List<SubjectSummary> Items { get; set; }
        public WeekDay WeekDay { get; set; }

    }

    public class WeekDay
    {
        public int Id { get; set; }
        public string Cn { get; set; }
        public string En { get; set; }
        public int Jp { get; set; }

        public static int WeekDayIdOfToday
        {
            get
            {
                return GetWeekDayId(DateTime.Today);
            }
        }

        public static int GetWeekDayId(DateTime date)
        {
            int dayOfWeek = (int)date.DayOfWeek;
            return dayOfWeek == 0 ? 7 : dayOfWeek;
        }
    }
}
