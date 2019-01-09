using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using Augment;
using vyger.Core;
using vyger.Core.Models;
using vyger.Core.Services;
using vyger.ViewModels;

namespace vyger.Controllers
{
    [RoutePrefix("Workouts/Routines/{id}/Exercises"), MvcAuthorizeRoles(Constants.Roles.ActiveMember)]
    public partial class WorkoutRoutineExercisesController : BaseController
    {
        #region Members

        private IWorkoutRoutineService _service;

        #endregion

        #region Constructors

        public WorkoutRoutineExercisesController(IWorkoutRoutineService service)
        {
            _service = service;
        }

        #endregion

        #region List Methods

        [HttpGet, Route("Index")]
        public virtual ActionResult Index(string id, int week)
        {
            WorkoutRoutineExerciseIndexViewModel vm = new WorkoutRoutineExerciseIndexViewModel();

            vm.Routine = _service.GetWorkoutRoutines().GetByPrimaryKey(id);

            vm.Week = week;

            return View(vm);
        }

        [HttpPost, Route("Index"), ValidateAntiForgeryToken]
        public virtual ActionResult Index(string id, int week, WorkoutRoutine post)
        {
            if (ModelState.IsValid)
            {
                WorkoutRoutine routine = _service.GetWorkoutRoutines().GetByPrimaryKey(id);

                IList<WorkoutRoutineExercise> exercises = routine
                    .RoutineExercises
                    .Filter(week, 0, null)
                    .ToList();

                foreach (WorkoutRoutineExercise posted in post.RoutineExercises)
                {
                    foreach (WorkoutRoutineExercise exercise in routine.RoutineExercises)
                    {
                        if (posted.ExerciseId == exercise.ExerciseId)
                        {
                            if (posted.WeekId == exercise.WeekId && posted.DayId == exercise.DayId)
                            {
                                exercise.OverlayWith(posted);
                            }

                            //  make the sequences match for this exercise/day

                            if (posted.DayId == exercise.DayId)
                            {
                                exercise.SequenceNumber = posted.SequenceNumber;
                            }
                        }
                    }
                }

                _service.SaveWorkoutRoutines();

                AddFlashMessage(FlashMessageType.Success, $"Exercises Saved for Week {week}");

                return RedirectToAction("Index", new { id, week });
            }

            WorkoutRoutineExerciseIndexViewModel vm = new WorkoutRoutineExerciseIndexViewModel();

            vm.Routine = _service.GetWorkoutRoutines().GetByPrimaryKey(id);

            vm.Week = week;

            return View(vm);
        }

        #endregion

        #region Create Methods

        [HttpGet, Route("Create")]
        public virtual ActionResult Create(string id, int day)
        {
            WorkoutRoutineExerciseCreateViewModel vm = CreateViewModel(id, day);

            return View(vm);
        }

        [HttpPost, Route("Create"), ValidateAntiForgeryToken]
        public virtual ActionResult Create(string id, int day, WorkoutRoutineExerciseCreateViewModel post)
        {
            WorkoutRoutineExerciseCreateViewModel vm = CreateViewModel(id, day);

            if (ModelState.IsValid)
            {
                vm.Routine.RoutineExercises.Add(day, post.RoutineExercise.ExerciseId, post.RoutineExercise.WorkoutRoutine);

                _service.SaveWorkoutRoutines();

                AddFlashMessage(FlashMessageType.Success, $"Successfully added to Day {day}");

                return RedirectToAction("Index", new { id, week = 1 });
            }

            return View(vm);
        }

        private WorkoutRoutineExerciseCreateViewModel CreateViewModel(string id, int day)
        {
            WorkoutRoutineExerciseCreateViewModel vm = new WorkoutRoutineExerciseCreateViewModel()
            {
                Routine = _service.GetWorkoutRoutines().GetByPrimaryKey(id),
                RoutineExercise = new WorkoutRoutineExercise()
                {
                    WeekId = 1,
                    DayId = day,
                    WorkoutRoutine = "[L]80%x5, [L]90%x5, 1RM85%x5"
                }
            };

            vm.Exercises = vm.Routine.AllExercises.ToList();

            return vm;
        }

        #endregion

        #region Delete Methods

        [HttpGet, Route("Delete")]
        public virtual ActionResult Delete(string id, int day, string exercise)
        {
            WorkoutRoutineExerciseDeleteViewModel vm = DeleteViewModel(id, day, exercise);

            return View(vm);
        }

        [HttpPost, Route("Delete"), ValidateAntiForgeryToken]
        public virtual ActionResult Delete(string id, int day, string exercise, WorkoutRoutineExercise post)
        {
            WorkoutRoutineExerciseDeleteViewModel vm = DeleteViewModel(id, 0, "");

            vm.Routine.RoutineExercises.DeleteWorkoutRoutineExercise(day, exercise);

            _service.SaveWorkoutRoutines();

            AddFlashMessage(FlashMessageType.Success, $"Exercise successfully removed from Day {day}");

            return RedirectToAction("Index", new { id, week = 1 });
        }

        private WorkoutRoutineExerciseDeleteViewModel DeleteViewModel(string id, int day, string exercise)
        {
            WorkoutRoutineExerciseDeleteViewModel vm = new WorkoutRoutineExerciseDeleteViewModel()
            {
                Routine = _service.GetWorkoutRoutines().GetByPrimaryKey(id)
            };

            if (day > 0 || exercise.IsNotEmpty())
            {
                vm.RoutineExercise = vm.Routine.RoutineExercises.Filter(1, day, exercise).FirstOrDefault();
            }

            return vm;
        }

        #endregion

        #region Copy Methods

        [HttpGet, Route("Copy")]
        public virtual ActionResult Copy(string id, int week, int day, string exercise)
        {
            WorkoutRoutine routine = _service.GetWorkoutRoutines().GetByPrimaryKey(id);

            string workoutRoutine = routine
                .RoutineExercises
                .Filter(week, day, exercise)
                .Select(x => x.WorkoutRoutine)
                .First();

            IEnumerable<WorkoutRoutineExercise> routineExercises = routine
                .RoutineExercises
                .Filter(0, day, exercise);

            foreach (WorkoutRoutineExercise rex in routineExercises)
            {
                if (rex.WeekId > week)
                {
                    rex.WorkoutRoutine = workoutRoutine;
                }
            }

            _service.SaveWorkoutRoutines();

            AddFlashMessage(FlashMessageType.Success, "Copied workout routine forward successfully");

            return RedirectToAction("Index", new { id, week });
        }

        #endregion
    }
}

