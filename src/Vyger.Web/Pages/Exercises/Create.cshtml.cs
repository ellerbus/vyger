using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Vyger.Common.Models;
using Vyger.Common.Services;

namespace Vyger.Web.Pages.Exercises
{
    [Authorize]
    public class CreateModel : PageModel
    {
        #region Members

        private IExerciseService _exercises;

        #endregion

        #region Constructors

        public CreateModel(IExerciseService exercises)
        {
            _exercises = exercises;
        }

        #endregion

        #region Methods

        public IActionResult OnGet()
        {
            Exercise = new Exercise();

            return new PageResult();
        }

        public IActionResult OnPost()
        {
            if (ModelState.IsValid)
            {
                _exercises.CreateExercise(Exercise);

                this.FlashSuccess($"Created Exercise");

                return Redirect("~/Exercises/Index");
            }

            this.FlashDanger($"Validation errors were found");

            return new PageResult();
        }

        #endregion

        #region Properties

        [BindProperty]
        public Exercise Exercise { get; set; }

        #endregion
    }
}