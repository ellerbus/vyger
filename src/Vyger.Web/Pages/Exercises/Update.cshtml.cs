using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Vyger.Common;
using Vyger.Common.Models;
using Vyger.Common.Services;

namespace Vyger.Web.Pages.Exercises
{
    [Authorize]
    public class UpdateModel : PageModel
    {
        #region Exercises

        private IExerciseService _exercises;

        #endregion

        #region Constructors

        public UpdateModel(IExerciseService exercises)
        {
            _exercises = exercises;
        }

        #endregion

        #region Methods

        public IActionResult OnGet(string id)
        {
            Exercise = _exercises.GetExercise(id);

            if (Exercise == null)
            {
                this.FlashWarning($"Could not find requested Exercise");

                return Redirect("~/Exercises/Index");
            }

            return new PageResult();
        }

        public IActionResult OnPost(string id)
        {
            if (ModelState.IsValid)
            {
                string[] inputs = new[]
                {
                    nameof(Exercise.Name)
                };

                Exercise cache = _exercises.GetExercise(id);

                Exercise.OverlayFrom(cache, inputs);

                _exercises.UpdateExercise(Exercise);

                this.FlashSuccess($"Saved Exercise");

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