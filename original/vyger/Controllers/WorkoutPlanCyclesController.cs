using System.Collections.Generic;
using System.Linq;
using System.Security;
using System.Web.Mvc;
using vyger.Core;
using vyger.Core.Models;
using vyger.Core.Services;
using vyger.ViewModels;

namespace vyger.Controllers
{
    [RoutePrefix("Workouts/Routines/{id}/Plans/{plan}/Cycles"), MvcAuthorizeRoles(Constants.Roles.ActiveMember)]
    public partial class WorkoutPlanCyclesController : BaseController
    {
        #region Members

        private IPlanGeneratorService _generator;
        private IWorkoutRoutineService _service;

        #endregion

        #region Constructors

        public WorkoutPlanCyclesController(
            IPlanGeneratorService generator,
            IWorkoutRoutineService service)
        {
            _generator = generator;
            _service = service;
        }

        #endregion

        #region List Methods

        [HttpGet, Route("Index")]
        public virtual ActionResult Index(string id, int plan)
        {
            WorkoutRoutine routine = _service.GetWorkoutRoutines().GetByPrimaryKey(id);

            WorkoutPlan vm = routine.Plans.GetByPrimaryKey(plan);

            return View(vm);
        }

        #endregion

        #region Create Methods

        [HttpGet, Route("Create")]
        public virtual ActionResult Create(string id, int plan)
        {
            WorkoutRoutine routine = _service.GetWorkoutRoutines().GetByPrimaryKey(id);

            WorkoutPlan vm = routine.Plans.GetByPrimaryKey(plan);

            return View(vm);
        }

        [HttpPost, Route("Create"), ValidateAntiForgeryToken]
        public virtual ActionResult CreateCycle(string id, int plan)
        {
            WorkoutRoutine routine = _service.GetWorkoutRoutines().GetByPrimaryKey(id);

            WorkoutPlan vm = routine.Plans.GetByPrimaryKey(plan);

            _generator.CreateWorkoutCycle(vm);

            _service.SaveWorkoutRoutines();

            AddFlashMessage(FlashMessageType.Success, "Workout Cycle Created");

            return RedirectToAction("Edit", new { id, plan, cycle = vm.CurrentCycleId });
        }

        #endregion

        #region Edit Methods

        [HttpGet, Route("Edit")]
        public virtual ActionResult Edit(string id, int plan, int cycle)
        {
            WorkoutRoutine routine = _service.GetWorkoutRoutines().GetByPrimaryKey(id);

            WorkoutPlanCycleEditViewModel vm = new WorkoutPlanCycleEditViewModel()
            {
                Plan = routine.Plans.GetByPrimaryKey(plan),
                CycleId = cycle
            };

            if (vm.Plan.CurrentCycleId != cycle)
            {
                return RedirectToAction("Display", new { id, plan, cycle });
            }

            vm.Cycle = vm.Plan.PlanCycles.Filter(cycle).ToList();

            return View(vm);
        }

        [HttpPost, Route("Edit"), ValidateAntiForgeryToken]
        public virtual ActionResult Edit(string id, int plan, int cycle, WorkoutPlan post)
        {
            WorkoutRoutine routine = _service.GetWorkoutRoutines().GetByPrimaryKey(id);

            WorkoutPlanCycleEditViewModel vm = new WorkoutPlanCycleEditViewModel()
            {
                Plan = routine.Plans.GetByPrimaryKey(plan),
                CycleId = cycle
            };

            post.Routine = routine;

            if (ModelState.IsValid)
            {
                foreach (WorkoutPlanCycle posted in post.PlanCycles)
                {
                    WorkoutPlanCycle item = vm.Plan.PlanCycles.Fetch(posted.CycleId, posted.ExerciseId);

                    item.OverlayWith(posted);
                }

                _generator.GenerateCurrentCycle(vm.Plan);

                _service.SaveWorkoutRoutines();

                AddFlashMessage(FlashMessageType.Success, "Cycle Setup Saved");

                return RedirectToAction("Index", "WorkoutPlanLogs", new { id, plan, cycle });
            }

            if (vm.Plan.CurrentCycleId != cycle)
            {
                return RedirectToAction("Display", new { id, plan, cycle });
            }

            vm.Cycle = post.PlanCycles.ToList();

            return View(vm);
        }

        #endregion

        #region Display Methods

        [HttpGet, Route("Display")]
        public virtual ActionResult Display(string id, int plan, int cycle)
        {
            WorkoutRoutine routine = _service.GetWorkoutRoutines().GetByPrimaryKey(id);

            WorkoutPlanCycleEditViewModel vm = new WorkoutPlanCycleEditViewModel()
            {
                Plan = routine.Plans.GetByPrimaryKey(plan),
                CycleId = cycle
            };

            vm.Cycle = vm.Plan.PlanCycles.Filter(cycle).ToList();

            return View(vm);
        }

        #endregion
    }
}