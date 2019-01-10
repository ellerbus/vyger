using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Vyger.Common.Models;
using Vyger.Common.Services;

namespace Vyger.Web.Pages.CycleExercises
{
    [Authorize]
    public class IndexModel : PageModel
    {
        #region Members

        private ICycleService _cycles;

        #endregion

        #region Constructors

        public IndexModel(ICycleService cycles)
        {
            _cycles = cycles;
        }

        #endregion

        #region Methods

        public IActionResult OnGet(string id, int week = 1, int day = 1)
        {
            LoadCycle(id, week, day);

            if (Cycle == null)
            {
                this.FlashWarning($"Could not find requested Cycle");

                return Redirect("~/Cycles/Index");
            }

            Exercises = Cycle.Exercises.Filter(week, day).ToList();

            return new PageResult();
        }

        private void LoadCycle(string id, int week, int day)
        {
            SelectedWeek = week;
            SelectedDay = day;

            Cycle = _cycles.GetCycle(id);
        }

        #endregion

        #region Properties

        [BindProperty]
        public Cycle Cycle { get; set; }

        [BindProperty]
        public IList<CycleExercise> Exercises { get; set; }

        [BindProperty]
        public int SelectedWeek { get; set; }

        [BindProperty]
        public int SelectedDay { get; set; }

        #endregion
    }
}