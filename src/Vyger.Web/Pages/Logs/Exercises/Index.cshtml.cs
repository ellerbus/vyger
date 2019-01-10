using System;
using System.Collections.Generic;
using System.Linq;
using Augment;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Vyger.Common;
using Vyger.Common.Models;
using Vyger.Common.Services;

namespace Vyger.Web.Pages.LogExercises
{
    [Authorize]
    public class IndexModel : PageModel
    {
        #region Members

        private ILogExerciseService _logs;

        #endregion

        #region Constructors

        public IndexModel(ILogExerciseService exercises)
        {
            _logs = exercises;
        }

        #endregion

        #region Methods

        public IActionResult OnGet(string date)
        {
            LoadSelectedDate(date);

            LogExercises = _logs.GetLogExerciseCollection()
                .FilterForUpdating(SelectedDate)
                .ToList();

            if (LogExercises.Count == 0)
            {
                return Redirect("~/Logs/Exercises/Create?date=" + SelectedDate.ToYMD());
            }

            return new PageResult();
        }

        public IActionResult OnPost(string date)
        {
            LoadSelectedDate(date);

            LogExercises = UpdateLogExercises().ToList();

            return new PageResult();
        }

        private IEnumerable<LogExercise> UpdateLogExercises()
        {
            LogExerciseCollection all = _logs.GetLogExerciseCollection();

            IList<LogExercise> logs = all
                .FilterForUpdating(SelectedDate)
                .ToList();

            for (int i = 0; i < LogExercises.Count; i++)
            {
                LogExercise input = LogExercises[i];

                LogExercise log = logs.First(x => x.Id.IsSameAs(input.Id));

                log.WorkoutPattern = input.WorkoutPattern;

                log.Sequence = (i + 1) * 100;

                logs.Remove(log);

                yield return log;
            }

            //  that which is left has been deleted via the UI
            foreach (LogExercise log in logs)
            {
                all.Remove(log);
            }

            _logs.SaveLogExercises();

            this.FlashInfo("Log Exercises Saved Successfully");
        }

        private void LoadSelectedDate(string date)
        {
            if (date.IsNullOrEmpty())
            {
                SelectedDate = DateTime.Now.Date;
            }
            else
            {
                SelectedDate = DateTime.Parse(date);
            }
        }

        #endregion

        #region Properties

        [BindProperty]
        public IList<LogExercise> LogExercises { get; set; }

        public DateTime SelectedDate { get; set; }

        #endregion
    }
}