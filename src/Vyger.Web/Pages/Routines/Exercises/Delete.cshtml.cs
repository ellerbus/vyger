using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Vyger.Common.Models;
using Vyger.Common.Services;

namespace Vyger.Web.Pages.Routines.Exercises
{
    public class DeleteModel : PageModel
    {
        #region Members

        private IExerciseService _exercises;
        private IRoutineService _routines;

        #endregion

        #region Constructors

        public DeleteModel(
            IExerciseService exercises,
            IRoutineService routines)
        {
            _exercises = exercises;
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

            Exercise = _exercises.GetExercise(exercise);

            if (Exercise == null)
            {
                this.FlashWarning($"Could not find requested Exercise");

                return Redirect("~/Routines/Exercises/Index/" + id);
            }

            return new PageResult();
        }

        public IActionResult OnPost(string id, string exercise, int day = 1)
        {
            LoadRoutine(id, day);

            List<RoutineExercise> remove = Routine.Exercises.FilterForUpdating(day, exercise).ToList();

            foreach (RoutineExercise item in remove)
            {
                Routine.Exercises.Remove(item);
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
        public Exercise Exercise { get; set; }

        [BindProperty]
        public int SelectedDay { get; set; }

        #endregion
    }
}