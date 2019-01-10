using System.Collections.Generic;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Vyger.Common.Models;
using Vyger.Common.Services;

namespace Vyger.Web.Pages.Routines
{
    [Authorize]
    public class CreateModel : PageModel
    {
        #region Members

        private IRoutineService _routines;

        #endregion

        #region Constructors

        public CreateModel(IRoutineService routines)
        {
            _routines = routines;
        }

        #endregion

        #region Methods

        public IActionResult OnGet()
        {
            Routine = new Routine();

            return new PageResult();
        }

        public IActionResult OnPost()
        {
            if (ModelState.IsValid)
            {
                _routines.CreateRoutine(Routine);

                this.FlashSuccess($"Created Routine");

                return Redirect("~/Routines/Exercises/Index/" + Routine.Id);
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