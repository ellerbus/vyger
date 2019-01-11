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

        public IActionResult OnPost(string id, int week = 1, int day = 1)
        {
            LoadRoutine(id, week, day);

            Exercises = UpdateExercises().ToList();

            return new PageResult();
        }

        private IEnumerable<RoutineExercise> UpdateExercises()
        {
            IList<RoutineExercise> all = Routine.Exercises
                .Filter(SelectedWeek, SelectedDay)
                .ToList();

            for (int i = 0; i < Exercises.Count; i++)
            {
                RoutineExercise input = Exercises[i];

                RoutineExercise exercise = all.First(x => x.Id.IsSameAs(input.Id));

                exercise.WorkoutPattern = input.WorkoutPattern;

                exercise.Sequence = (i + 1) * 100;

                IEnumerable<RoutineExercise> sequences = Routine.Exercises.FilterForUpdating(SelectedDay, exercise.Id);

                foreach (RoutineExercise seq in sequences)
                {
                    seq.Sequence = exercise.Sequence;
                }

                yield return exercise;
            }

            _routines.UpdateRoutine(Routine);

            this.FlashInfo("Routine Exercises Saved Successfully");
        }

        //private void SortExercises()
        //{
        //    string[] ids = Request.Form["ids"].ToString().Split(',');

        //    for (int i = 0; i < ids.Length; i++)
        //    {
        //        string id = ids[i];

        //        IEnumerable<RoutineExercise> exercises = Routine.Exercises.FilterForUpdating(SelectedDay, id);

        //        foreach (RoutineExercise exercise in exercises)
        //        {
        //            exercise.Sequence = (i + 1) * 100;
        //        }
        //    }

        //    this.FlashInfo("Exercise Sequences Saved Successfully");
        //}

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