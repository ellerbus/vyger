using System.Collections.Generic;
using System.Linq;
using Augment;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Vyger.Common.Models;
using Vyger.Common.Services;

namespace Vyger.Web.Pages.Cycles
{
    [Authorize]
    public class CreateModel : PageModel
    {
        #region Members

        private IRoutineService _routines;
        private ICycleService _cycles;

        #endregion

        #region Constructors

        public CreateModel(
            IRoutineService routines,
            ICycleService cycles)
        {
            _routines = routines;
            _cycles = cycles;
        }

        #endregion

        #region Methods

        public IActionResult OnGet()
        {
            LoadRoutines();

            return new PageResult();
        }

        public IActionResult OnPost()
        {
            if (SelectedRoutineId.IsNullOrEmpty())
            {
                LoadRoutines();

                this.FlashDanger($"A Routine is required");

                return new PageResult();
            }

            Routine routine = _routines.GetRoutine(SelectedRoutineId);

            if (routine == null)
            {
                LoadRoutines();

                this.FlashDanger($"Invalid Routine Selected");

                return new PageResult();
            }

            Cycle cycle = new Cycle(routine);

            _cycles.CreateCycle(cycle);

            this.FlashSuccess($"Created Cycle");

            return Redirect("~/Cycles/Inputs/Index/" + cycle.Id);
        }

        private void LoadRoutines()
        {
            Routines = _routines.GetRoutineCollection().OrderBy(x => x.Name);
        }

        #endregion

        #region Properties

        [BindProperty]
        public string SelectedRoutineId { get; set; }

        public IEnumerable<Routine> Routines { get; set; }

        #endregion
    }
}