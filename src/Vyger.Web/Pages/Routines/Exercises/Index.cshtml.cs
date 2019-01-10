using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Vyger.Common.Models;
using Vyger.Common.Services;

namespace Vyger.Web.Pages.RoutineExercises
{
    [Authorize]
    public class IndexModel : PageModel
    {
        #region Members

        private IRoutineService _routines;

        #endregion

        #region Constructors

        public IndexModel(IRoutineService routines)
        {
            _routines = routines;
        }

        #endregion

        #region Methods

        public IActionResult OnGet(string id, int week = 1, int day = 1)
        {
            LoadRoutine(id, week, day);

            if (Routine == null)
            {
                this.FlashWarning($"Could not find requested Routine");

                return Redirect("~/Routines/Index");
            }

            Exercises = Routine.Exercises.Filter(week, day).ToList();

            return new PageResult();
        }

        public IActionResult OnPostSortExercises(string id, int week = 1, int day = 1)
        {
            LoadRoutine(id, week, day);

            SortExercises();

            Exercises = Routine.Exercises.Filter(week, day).ToList();

            return new PageResult();
        }

        private void SortExercises()
        {
            string[] ids = Request.Form["ids"].ToString().Split(',');

            for (int i = 0; i < ids.Length; i++)
            {
                string id = ids[i];

                IEnumerable<RoutineExercise> exercises = Routine.Exercises.FilterForUpdating(SelectedDay, id);

                foreach (RoutineExercise exercise in exercises)
                {
                    exercise.Sequence = (i + 1) * 100;
                }
            }

            _routines.UpdateRoutine(Routine);

            this.FlashInfo("Exercise Sequences Saved Successfully");
        }

        private void LoadRoutine(string id, int week, int day)
        {
            SelectedWeek = week;
            SelectedDay = day;

            Routine = _routines.GetRoutine(id);
        }

        #endregion

        #region Properties

        [BindProperty]
        public Routine Routine { get; set; }

        [BindProperty]
        public IList<RoutineExercise> Exercises { get; set; }

        [BindProperty]
        public int SelectedWeek { get; set; }

        [BindProperty]
        public int SelectedDay { get; set; }

        #endregion
    }
}