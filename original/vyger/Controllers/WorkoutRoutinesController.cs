using System.Web.Mvc;
using vyger.Core;
using vyger.Core.Models;
using vyger.Core.Services;
using vyger.ViewModels;

namespace vyger.Controllers
{
    [RoutePrefix("Workouts/Routines"), MvcAuthorizeRoles(Constants.Roles.ActiveMember)]
    public partial class WorkoutRoutinesController : BaseController
    {
        #region Members

        private IWorkoutRoutineService _service;

        #endregion

        #region Constructors

        public WorkoutRoutinesController(IWorkoutRoutineService service)
        {
            _service = service;
        }

        #endregion

        #region List Methods

        [HttpGet, Route("Index")]
        public virtual ActionResult Index()
        {
            WorkoutRoutineCollection routines = _service.GetWorkoutRoutines();

            return View(routines);
        }

        #endregion

        #region Create Methods

        [HttpGet, Route("Create")]
        public virtual ActionResult Create()
        {
            WorkoutRoutineViewModel vm = new WorkoutRoutineViewModel();

            vm.Id = Constants.SequenceId(_service.GetWorkoutRoutines().Count);

            return View(vm);
        }

        [HttpPost, Route("Create"), ValidateAntiForgeryToken]
        public virtual ActionResult Create(WorkoutRoutine post)
        {
            if (ModelState.IsValid)
            {
                WorkoutRoutineCollection routines = _service.GetWorkoutRoutines();

                routines.Add(post);

                _service.SaveWorkoutRoutines();

                AddFlashMessage(FlashMessageType.Success, "Workout Routine created successfully");

                return RedirectToAction("Index", "WorkoutRoutineExercises", new { id = post.Id, week = 1 });
            }

            WorkoutRoutineViewModel vm = new WorkoutRoutineViewModel(post);

            return View(vm);
        }

        #endregion

        #region Edit Methods

        [HttpGet, Route("Edit/{id}")]
        public virtual ActionResult Edit(string id)
        {
            WorkoutRoutine routine = _service.GetWorkoutRoutines().GetByPrimaryKey(id);

            if (routine == null)
            {
                return RedirectToAction("Index");
            }

            WorkoutRoutineViewModel vm = new WorkoutRoutineViewModel(routine);

            return View(vm);
        }

        [HttpPost, Route("Edit/{id}"), ValidateAntiForgeryToken]
        public virtual ActionResult Edit(string id, WorkoutRoutine post)
        {
            if (ModelState.IsValid)
            {
                WorkoutRoutine routine = _service.GetWorkoutRoutines().GetByPrimaryKey(id);

                routine.OverlayWith(post);

                _service.SaveWorkoutRoutines();

                AddFlashMessage(FlashMessageType.Success, "Workout Routine saved successfully");

                return RedirectToAction("Index", "WorkoutRoutineExercises", new { id = post.Id, week = 1 });
            }

            WorkoutRoutineViewModel vm = new WorkoutRoutineViewModel(post);

            return View(vm);
        }

        #endregion
    }
}

