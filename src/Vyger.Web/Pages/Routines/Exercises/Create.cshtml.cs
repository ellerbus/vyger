using System.Collections.Generic;
using System.Linq;
using Augment;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Vyger.Common.Models;
using Vyger.Common.Services;

namespace Vyger.Web.Pages.RoutineExercises
{
    [Authorize]
    public class CreateModel : PageModel
    {
        #region Members

        private IExerciseService _exercises;
        private IRoutineService _routines;

        #endregion

        #region Constructors

        public CreateModel(
            IExerciseService exercises,
            IRoutineService routines)
        {
            _exercises = exercises;
            _routines = routines;
        }

        #endregion

        #region Methods

        public IActionResult OnGet(string id, int day = 1)
        {
            LoadRoutine(id, day);

            if (Routine == null)
            {
                this.FlashWarning($"Could not find requested Routine");

                return Redirect("~/Routines/Index");
            }

            LoadExercises();

            Exercise = new RoutineExercise()
            {
                Day = SelectedDay,
                Sets = Routine.Sets.ToArray()
            };

            return new PageResult();
        }

        public IActionResult OnPost(string id, int day = 1)
        {
            LoadRoutine(id, day);

            if (Exercise.Id.IsNullOrEmpty())
            {
                ModelState.AddModelError("Id", "Select an Exercise");

                this.FlashDanger($"Validation errors were found");

                LoadExercises();

                return new PageResult();
            }

            Exercise primary = _exercises.GetExercise(Exercise.Id);

            for (int week = 1; week <= Routine.Weeks; week++)
            {
                RoutineExercise exercise = new RoutineExercise(week, day, primary);

                exercise.Sets = Exercise.Sets.ToArray();

                exercise.Sequence = 99999;

                Routine.Exercises.Add(exercise);
            }

            _routines.UpdateRoutine(Routine);

            this.FlashSuccess($"Added Routine Exercise");

            return Redirect($"~/Routines/Exercises/Index/{id}?day={SelectedDay}");
        }

        private void LoadRoutine(string id, int day)
        {
            SelectedDay = day;

            Routine = _routines.GetRoutine(id);
        }

        private void LoadExercises()
        {
            IEnumerable<string> list = Routine.Exercises.Select(x => x.Id).Distinct();

            HashSet<string> hash = new HashSet<string>(list);

            Exercises = _exercises
                .GetExerciseCollection()
                .Where(x => !hash.Contains(x.Id))
                .OrderBy(x => x.DisplayName)
                .ToList();
        }

        #endregion

        #region Properties

        public Routine Routine { get; set; }

        [BindProperty]
        public RoutineExercise Exercise { get; set; }

        [BindProperty]
        public IList<Exercise> Exercises { get; set; }

        [BindProperty]
        public int SelectedDay { get; set; }

        #endregion
    }
}