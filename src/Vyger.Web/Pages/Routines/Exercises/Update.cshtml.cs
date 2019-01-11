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
    public class UpdateModel : PageModel
    {
        #region Members

        private IRoutineService _routines;

        #endregion

        #region Constructors

        public UpdateModel(IRoutineService routines)
        {
            _routines = routines;
        }

        #endregion

        #region Methods

        public IActionResult OnGet(string id, string exercise, int day = 1)
        {
            LoadRoutine(id, day);

            if (Routine == null)
            {
                this.FlashWarning($"Could not find requested Routine");

                return Redirect("~/Routines/Index");
            }

            Exercises = Routine.Exercises
                .FilterForUpdating(day, exercise)
                .OrderBy(x => x.Week)
                .ToList();

            Exercise = Exercises.First();

            return new PageResult();
        }

        public IActionResult OnPost(string id, string exercise, int day = 1)
        {
            LoadRoutine(id, day);

            foreach (RoutineExercise item in Exercises)
            {
                RoutineExercise cache = Routine.Exercises.FilterForUpdate(item);

                cache.WorkoutPattern = item.WorkoutPattern;
            }

            _routines.UpdateRoutine(Routine);

            this.FlashSuccess($"Saved Routine Exercise");

            return Redirect($"~/Routines/Exercises/Index/{id}?day={SelectedDay}");
        }

        private void LoadRoutine(string id, int day)
        {
            SelectedDay = day;

            Routine = _routines.GetRoutine(id);
        }

        #endregion

        #region Properties

        [BindProperty]
        public Routine Routine { get; set; }

        [BindProperty]
        public RoutineExercise Exercise { get; set; }

        [BindProperty]
        public IList<RoutineExercise> Exercises { get; set; }

        [BindProperty]
        public int SelectedDay { get; set; }

        #endregion
    }
}