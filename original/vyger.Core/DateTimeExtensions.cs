using System;
using Augment;

namespace vyger.Core
{
    public static class DateTimeExtensions
    {
        public static DateTime? EnsureUtc(this DateTime? dt)
        {
            if (dt == null)
            {
                return null;
            }

            return dt.Value.EnsureUtc();
        }

        public static DateTime GetNextMonday(this DateTime date)
        {
            return GetNextWeekday(date, DayOfWeek.Monday);
        }

        private static DateTime GetNextWeekday(this DateTime date, DayOfWeek day)
        {
            // The (... + 7) % 7 ensures we end up with a value in the range [0, 6]
            int daysToAdd = ((int)day - (int)date.DayOfWeek + 7) % 7;

            return date.Date.AddDays(daysToAdd);
        }
    }
}
