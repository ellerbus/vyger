using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using vyger.Core;
using vyger.Core.Models;
using vyger.Core.Services;

namespace vyger.Controllers
{
    [RoutePrefix("Workouts/Routines"), MvcAuthorizeRoles(Constants.Roles.ActiveMember)]
    public partial class WorkoutPlansController : BaseController
    {
        #region Members

        private IPlanGeneratorService _generator;
        private IWorkoutRoutineService _service;

        #endregion

        #region Constructors

        public WorkoutPlansController(
            IPlanGeneratorService generator,
            IWorkoutRoutineService service)
        {
            _generator = generator;
            _service = service;
        }

        #endregion

        #region List Methods

        [HttpGet, Route("All/Plans")]
        public virtual ActionResult All()
        {
            List<WorkoutPlan> plans = _service
                .GetWorkoutRoutines()
                .SelectMany(x => x.Plans)
                .OrderByDescending(x => x.CreatedAt)
                .ToList();

            if (plans.Count == 0)
            {
                AddFlashMessage(FlashMessageType.Info, "No Workout Plans currently available, use a Routine to create one");

                return RedirectToAction("Index", "Routines");
            }

            return View(plans);
        }

        [HttpGet, Route("{id}/Plans/Index")]
        public virtual ActionResult Index(string id)
        {
            WorkoutRoutine routine = _service.GetWorkoutRoutines().GetByPrimaryKey(id);

            if (routine.Plans.Count == 0)
            {
                AddFlashMessage(FlashMessageType.Info, "No Workout Plans currently available for " + routine.Name);

                return RedirectToAction("Create", new { id });
            }

            return View(routine);
        }

        #endregion

        #region Create Methods

        [HttpGet, Route("{id}/Plans/Create")]
        public virtual ActionResult Create(string id)
        {
            WorkoutRoutine routine = _service.GetWorkoutRoutines().GetByPrimaryKey(id);

            return View(routine);
        }

        [HttpPost, Route("{id}/Plans/Create"), ValidateAntiForgeryToken]
        public virtual ActionResult CreatePlan(string id)
        {
            WorkoutRoutine routine = _service.GetWorkoutRoutines().GetByPrimaryKey(id);

            WorkoutPlan plan = _generator.CreateWorkoutPlan(routine);

            routine.Plans.Add(plan);

            _service.SaveWorkoutRoutines();

            return RedirectToAction("Index", new { id, plan = plan.Id });
        }

        #endregion
    }
}