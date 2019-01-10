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
    public class CreateModel : PageModel
    {
        #region Members

        private IExerciseService _exercises;
        private ILogExerciseService _logs;

        #endregion

        #region Constructors

        public CreateModel(
            IExerciseService exercises,
            ILogExerciseService logs)
        {
            _exercises = exercises;
            _logs = logs;
        }

        #endregion

        #region Methods

        public IActionResult OnGet(string date)
        {
            LoadSelectedDate(date);

            LoadExercises();

            LogExercise = new LogExercise();

            return new PageResult();
        }

        public IActionResult OnGetLastWorkout(string date, string id)
        {
            DateTime dt = DateTime.Parse(date);

            LogExercise log = _logs.GetLogExerciseCollection()
                .Where(x => x.Id.IsSameAs(id) && x.Date < dt)
                .OrderByDescending(x => x.Date)
                .FirstOrDefault();

            return new JsonResult(log?.WorkoutPattern);
        }

        public IActionResult OnPost(string date)
        {
            LoadSelectedDate(date);

            if (ModelState.IsValid)
            {
                Exercise exercise = _exercises.GetExercise(LogExercise.Id);

                LogExercise log = _logs.GetLogExercise(SelectedDate, LogExercise.Id);

                if (log == null)
                {
                    log = new LogExercise(SelectedDate, exercise);
                }

                log.WorkoutPattern = LogExercise.WorkoutPattern;

                _logs.CreateLogExercise(log);

                this.FlashSuccess($"Created Log Exercise");

                return Redirect("~/Logs/Exercises/Index?date=" + SelectedDate.ToYMD());
            }

            LoadExercises();

            this.FlashDanger($"Validation errors were found");

            return new PageResult();
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

        private void LoadExercises()
        {
            IEnumerable<string> list = _logs.GetLogExerciseCollection()
                .FilterForUpdating(SelectedDate)
                .Select(x => x.Id)
                .Distinct();

            HashSet<string> hash = new HashSet<string>(list);

            Exercises = _exercises
                .GetExerciseCollection()
                .Where(x => !hash.Contains(x.Id))
                .OrderBy(x => x.DisplayName)
                .ToList();
        }

        #endregion

        #region Properties

        public DateTime SelectedDate { get; set; }

        [BindProperty]
        public LogExercise LogExercise { get; set; }

        [BindProperty]
        public IList<Exercise> Exercises { get; set; }

        #endregion
    }
}