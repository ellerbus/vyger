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

namespace Vyger.Web.Pages.CycleExercises
{
    [Authorize]
    public class IndexModel : PageModel
    {
        #region Members

        private ILogExerciseService _logs;
        private ICycleService _cycles;

        #endregion

        #region Constructors

        public IndexModel(
            ILogExerciseService logs,
            ICycleService cycles)
        {
            _logs = logs;
            _cycles = cycles;
        }

        #endregion

        #region Methods

        public IActionResult OnGet(string id, int week = 1, int day = 1)
        {
            LoadCycle(id, week, day);

            if (Cycle == null)
            {
                this.FlashWarning($"Could not find requested Cycle");

                return Redirect("~/Cycles/Index");
            }

            LoadExercises();

            return new PageResult();
        }

        public IActionResult OnPost(string id, int week = 1, int day = 1)
        {
            LoadCycle(id, week, day);

            LoadExercises();

            LogExerciseCollection all = _logs.GetLogExerciseCollection();

            IList<LogExercise> logs = all
                .FilterForUpdating(SelectedDate)
                .ToList();

            foreach (CycleExercise exercise in Exercises)
            {
                LogExercise log = logs.FirstOrDefault(x => x.Id.IsSameAs(exercise.Id));

                if (log == null)
                {
                    log = new LogExercise(SelectedDate, exercise);

                    all.Add(log);
                }
            }

            _logs.SaveLogExercises();

            Cycle.LastLogged = $"{SelectedWeek}:{SelectedDay}";

            _cycles.UpdateCycle(Cycle);

            return Redirect("~/Logs/Exercises/Index?date=" + SelectedDate.ToYMD());
        }

        private void LoadCycle(string id, int week, int day)
        {
            SelectedWeek = week;
            SelectedDay = day;

            Cycle = _cycles.GetCycle(id);
        }

        private void LoadExercises()
        {
            Exercises = Cycle.Exercises
                .Filter(SelectedWeek, SelectedDay)
                .ToList();
        }

        #endregion

        #region Properties

        public Cycle Cycle { get; set; }

        public IList<CycleExercise> Exercises { get; set; }

        public bool CanLogWorkout
        {
            get
            {
                if (SelectedWeek > Cycle.LoggedWeek)
                {
                    return true;
                }
                else
                {
                    if (SelectedDay > Cycle.LoggedDay)
                    {
                        return true;
                    }
                }

                return false;
            }
        }

        public int SelectedWeek { get; set; }

        public int SelectedDay { get; set; }

        public IEnumerable<DateTime> AvailableDates
        {
            get
            {
                DateTime dt = DateTime.Now.Date;

                yield return dt.AddDays(-1);
                yield return dt.AddDays(-2);
                yield return dt.AddDays(-3);
                yield return dt.AddDays(-4);
                yield return dt.AddDays(-5);
                yield return dt.AddDays(-6);
                yield return dt.AddDays(-7);
                yield return dt.AddDays(-8);
                yield return dt.AddDays(-9);
            }
        }

        [BindProperty]
        public DateTime SelectedDate { get; set; } = DateTime.Now.Date;

        #endregion
    }
}