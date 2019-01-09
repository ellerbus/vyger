using System.Linq;
using System.Web.Mvc;
using vyger.Core;
using vyger.Core.Models;
using vyger.Core.Services;
using vyger.ViewModels;

namespace vyger.Controllers
{
    [RoutePrefix("Workouts/Routines/{id}/Plans/{plan}/Logs"), MvcAuthorizeRoles(Constants.Roles.ActiveMember)]
    public partial class WorkoutPlanLogsController : BaseController
    {
        #region Members

        private IWorkoutRoutineService _service;

        #endregion

        #region Constructors

        public WorkoutPlanLogsController(IWorkoutRoutineService service)
        {
            _service = service;
        }

        #endregion

        #region List Methods

        [HttpGet, Route("Index")]
        public virtual ActionResult Index(string id, int plan, int cycle)
        {
            WorkoutRoutine routine = _service.GetWorkoutRoutines().GetByPrimaryKey(id);

            WorkoutPlanLogIndexViewModel vm = new WorkoutPlanLogIndexViewModel()
            {
                Plan = routine.Plans.GetByPrimaryKey(plan),
                CycleId = cycle
            };

            return View(vm);
        }

        #endregion
    }
}