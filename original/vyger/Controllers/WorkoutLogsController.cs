using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using vyger.Core;
using vyger.Core.Models;
using vyger.Core.Services;
using vyger.ViewModels;

namespace vyger.Controllers
{
    [RoutePrefix("Workouts/Logs"), MvcAuthorizeRoles(Constants.Roles.ActiveMember)]
    public partial class WorkoutLogsController : BaseController
    {
        #region Members

        private IWorkoutLogService _service;
        private IExerciseService _exercises;
        private IWorkoutRoutineService _routines;

        #endregion

        #region Constructors

        public WorkoutLogsController(
            IWorkoutLogService service,
            IExerciseService exercises,
            IWorkoutRoutineService routines)
        {
            _service = service;
            _exercises = exercises;
            _routines = routines;
        }

        #endregion

        #region List Methods

        [HttpGet, Route("Index")]
        public virtual ActionResult Index()
        {
            return View();
        }

        [HttpGet, Route("Details")]
        public virtual ActionResult Details(DateTime date)
        {
            WorkoutLogDetailViewModel vm = new WorkoutLogDetailViewModel();

            vm.LogDate = date;
            vm.Logs = _service.GetWorkoutLogs().Filter(vm.LogDate).ToList();

            if (vm.Logs.Count == 0)
            {
                AddFlashMessage(FlashMessageType.Info, "Add an Exercise to the Workout Log");

                return RedirectToAction("Create", new { date = date.ToYMD() });
            }

            return View(vm);
        }

        [HttpPost, Route("Details"), ValidateAntiForgeryToken]
        public virtual ActionResult Details(WorkoutLogDetailViewModel post, DateTime date)
        {
            if (ModelState.IsValid)
            {
                return SaveWorkoutDetails(post, date);
            }

            ExerciseCollection exercises = _exercises.GetExercises();

            foreach (var log in post.Logs)
            {
                log.Exercise = exercises.GetByPrimaryKey(log.ExerciseId);
            }

            WorkoutLogDetailViewModel vm = new WorkoutLogDetailViewModel();

            vm.LogDate = date;
            vm.Logs = post.Logs;

            return View(vm);
        }

        private ActionResult SaveWorkoutDetails(WorkoutLogDetailViewModel post, DateTime date)
        {
            WorkoutLogCollection allLogs = _service.GetWorkoutLogs();

            IDictionary<string, WorkoutLog> logs = allLogs.Filter(date).ToDictionary(x => x.ExerciseId, x => x);

            //  add or update logs
            foreach (WorkoutLog log in post.Logs)
            {
                WorkoutLog target = null;

                if (logs.TryGetValue(log.ExerciseId, out target))
                {
                    target.OverlayWith(log);

                    logs.Remove(log.ExerciseId);
                }
                else
                {
                    //  not found
                    log.LogDate = date;

                    target = log;

                    allLogs.Add(target);
                }
            }

            //  do we have any to remove
            if (logs.Count > 0)
            {
                foreach (WorkoutLog log in logs.Values)
                {
                    allLogs.Remove(log);
                }
            }

            _service.SaveWorkoutLogs();

            AddFlashMessage(FlashMessageType.Success, $"Workout Log saved for {date:d}");

            return RedirectToAction("Details", new { date = date.ToYMD() });
        }

        #endregion

        #region Create by Exercise Methods

        [HttpGet, Route("Create")]
        public virtual ActionResult Create(DateTime date)
        {
            WorkoutLogCreateViewModel vm = new WorkoutLogCreateViewModel();

            vm.Exercises = _exercises.GetExercises();
            vm.LogDate = date;

            return View(vm);
        }

        [HttpPost, Route("Create"), ValidateAntiForgeryToken]
        public virtual ActionResult Create(WorkoutLog post, DateTime date)
        {
            if (ModelState.IsValid)
            {
                IDictionary<string, WorkoutLog> logs = _service
                    .GetWorkoutLogs()
                    .Filter(date)
                    .ToDictionary(x => x.ExerciseId, x => x);

                if (logs.ContainsKey(post.ExerciseId))
                {
                    AddFlashMessage(FlashMessageType.Warning, $"Exercise was already logged");
                }
                else
                {
                    post.LogDate = date;

                    post.SequenceNumber = logs.Count;

                    _service.GetWorkoutLogs().Add(post);

                    _service.SaveWorkoutLogs();

                    AddFlashMessage(FlashMessageType.Success, "Exercise logged successfully");
                }

                return RedirectToAction("Details", new { date = date.ToYMD() });
            }

            WorkoutLogCreateViewModel vm = new WorkoutLogCreateViewModel();

            vm.Exercises = _exercises.GetExercises();
            vm.LogDate = date;
            vm.ExerciseId = post.ExerciseId;
            vm.Workout = post.Workout;

            Exercise exercise = null;

            _exercises.GetExercises().TryGetByPrimaryKey(vm.ExerciseId, out exercise);

            vm.Exercise = exercise;

            return View(vm);
        }

        #endregion

        #region Create by Plan Methods

        [HttpGet, Route("Plans")]
        public virtual ActionResult Plans(string routine, int plan, int cycle, int week, int day, DateTime? date = null)
        {
            WorkoutLogDetailViewModel vm = new WorkoutLogDetailViewModel()
            {
                LogDate = date.GetValueOrDefault(DateTime.UtcNow.Date),
                CanChangeDate = true,
                RoutineId = routine,
                PlanId = plan,
                CycleId = cycle,
                WeekId = week,
                DayId = day
            };

            WorkoutRoutine r = _routines.GetWorkoutRoutines().GetByPrimaryKey(routine);

            WorkoutPlan p = r.Plans.GetByPrimaryKey(plan);

            vm.Logs = p.PlanLogs
                .Filter(cycle, week, day)
                .Select(x => x.ToWorkoutLog(vm.LogDate))
                .OrderBy(x => x.SequenceNumber)
                .ToList();

            return View("Details", vm);
        }

        [HttpPost, Route("Plans"), ValidateAntiForgeryToken]
        public virtual ActionResult Plans(WorkoutLogDetailViewModel post, string routine, int plan, int cycle, int week, int day, DateTime? date = null)
        {
            DateTime logDate = date.GetValueOrDefault(DateTime.UtcNow.Date);

            if (ModelState.IsValid)
            {
                WorkoutRoutine r = _routines.GetWorkoutRoutines().GetByPrimaryKey(routine);

                WorkoutPlan p = r.Plans.GetByPrimaryKey(plan);

                ActionResult results = SaveWorkoutDetails(post, logDate);

                foreach (var log in p.PlanLogs.Filter(cycle, week, day))
                {
                    log.Status = StatusTypes.Logged;
                }

                _routines.SaveWorkoutRoutines();

                return results;
            }

            ExerciseCollection exercises = _exercises.GetExercises();

            foreach (var log in post.Logs)
            {
                log.Exercise = exercises.GetByPrimaryKey(log.ExerciseId);
            }

            WorkoutLogDetailViewModel vm = new WorkoutLogDetailViewModel();

            vm.LogDate = logDate;
            vm.Logs = post.Logs;
            vm.RoutineId = routine;
            vm.PlanId = plan;
            vm.CycleId = cycle;
            vm.WeekId = week;
            vm.DayId = day;

            return View("Details", vm);
        }

        #endregion

        #region Json Query Methods

        [HttpGet, Route("Events")]
        public virtual ActionResult Events(DateTime start, DateTime end)
        {
            var events = _service.GetWorkoutLogs()
                .Filter(start, end)
                .Select(x => x.LogDate.ToYMD())
                .Distinct()
                .Select(x => new { start = x, title = "Workout Details", url = Url.Action("Details", new { date = x }) });

            return Json(events, JsonRequestBehavior.AllowGet);
        }

        #endregion
    }
}