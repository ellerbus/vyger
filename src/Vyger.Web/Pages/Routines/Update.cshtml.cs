using System.Collections.Generic;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Vyger.Common;
using Vyger.Common.Models;
using Vyger.Common.Services;

namespace Vyger.Web.Pages.Routines
{
    [Authorize]
    public class UpdateModel : PageModel
    {
        #region Members

        private IRoutineService _routines;

        #endregion

        #region Constructors

        public UpdateModel(IRoutineService routines)
        {
            _routines = routines;
        }

        #endregion

        #region Methods

        public IActionResult OnGet(string id)
        {
            Routine = _routines.GetRoutine(id);

            if (Routine == null)
            {
                this.FlashWarning($"Could not find requested Routine");

                return Redirect("~/Routines/Index");
            }

            return new PageResult();
        }

        public IActionResult OnPost(string id)
        {
            if (ModelState.IsValid)
            {
                string[] inputs = new[]
                {
                    nameof(Routine.Name),
                    nameof(Routine.Weeks),
                    nameof(Routine.Days),
                    nameof(Routine.Sets),
                    //nameof(Routine.WorkoutPattern),
                };

                Routine cache = _routines.GetRoutine(id);

                Routine.OverlayFrom(cache, inputs);

                _routines.UpdateRoutine(Routine);

                this.FlashSuccess($"Saved Routine");

                return Redirect($"~/Routines/Exercises/Index/{Routine.Id}");
            }

            this.FlashDanger($"Validation errors were found");

            return new PageResult();
        }

        #endregion

        #region Properties

        [BindProperty]
        public Routine Routine { get; set; }

        [BindProperty]
        public IEnumerable<SelectListItem> WeekSelectList
        {
            get
            {
                return WebConstants.GetWeekSelectListItems(Routine.Weeks);
            }
        }

        [BindProperty]
        public IEnumerable<SelectListItem> DaySelectList
        {
            get
            {
                return WebConstants.GetDaySelectListItems(Routine.Days);
            }
        }

        #endregion
    }
}