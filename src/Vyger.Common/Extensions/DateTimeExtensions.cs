using System;

namespace Vyger.Common
{
    public static class DateTimeExtensions
    {
        public static string ToYMD(this DateTime date)
        {
            return date.ToString("yyyy-MM-dd");
        }
    }
}