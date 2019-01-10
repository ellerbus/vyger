using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Vyger.Common.Models;
using Vyger.Common.Services;

namespace Vyger.Web.Pages.Exercises
{
    [Authorize]
    public class IndexModel : PageModel
    {
        #region Members

        private IExerciseService _exercises;

        #endregion

        #region Constructors

        public IndexModel(IExerciseService exercises)
        {
            _exercises = exercises;
        }

        #endregion

        #region Methods

        public void OnGet(ExerciseGroups group = ExerciseGroups.None, ExerciseCategories category = ExerciseCategories.None)
        {
            SelectedGroup = group;
            SelectedCategory = category;

            Exercises = _exercises
                .GetExerciseCollection()
                .Filter(group, category)
                .OrderBy(x => x.DisplayName);
        }

        #endregion

        #region Properties

        [BindProperty]
        public IEnumerable<Exercise> Exercises { get; set; }

        [BindProperty]
        public ExerciseGroups SelectedGroup { get; set; }

        [BindProperty]
        public ExerciseCategories SelectedCategory { get; set; }

        #endregion
    }
}