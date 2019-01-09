using System.Collections.Generic;
using Augment;
using Microsoft.AspNetCore.Mvc.Rendering;
using Vyger.Common.Configuration;

namespace Vyger.Web
{
    public static class WebConstants
    {
        public static ApplicationConfiguration Application { get; } = new VygerConfiguration().Application;

        public static IEnumerable<SelectListItem> GetWeekSelectListItems(int week)
        {
            for (int i = 0; i < 9; i++)
            {
                yield return new SelectListItem()
                {
                    Value = (i + 1).ToString(),
                    Text = "{0} Week{1} per Cycle".FormatArgs(i + 1, i > 1 ? "s" : ""),
                    Selected = (i + 1) == week
                };
            }
        }

        public static IEnumerable<SelectListItem> GetDaySelectListItems(int day)
        {
            for (int i = 0; i < 7; i++)
            {
                yield return new SelectListItem()
                {
                    Value = (i + 1).ToString(),
                    Text = "{0} Day{1} per Week".FormatArgs(i + 1, i > 1 ? "s" : ""),
                    Selected = (i + 1) == day
                };
            }
        }
    }
}