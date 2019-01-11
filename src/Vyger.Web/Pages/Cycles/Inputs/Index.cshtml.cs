using System;
using System.Collections.Generic;
using System.Linq;
using Augment;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Vyger.Common.Models;
using Vyger.Common.Services;

namespace Vyger.Web.Pages.CycleInputs
{
    [Authorize]
    public class IndexModel : PageModel
    {
        #region Members

        private ILogExerciseService _logs;
        private ICycleService _cycles;

        #endregion

        #region Constructors

        public IndexModel(
            ILogExerciseService logs,
            ICycleService cycles)
        {
            _logs = logs;
            _cycles = cycles;
        }

        #endregion

        #region Methods

        public IActionResult OnGet(string id)
        {
            LoadCycle(id);

            if (Cycle == null)
            {
                this.FlashWarning($"Could not find requested Cycle");

                return Redirect("~/Cycles/Index");
            }

            if (Cycle.LastLogged.IsNotEmpty())
            {
                return Redirect("~/Cycles/Exercises/Index/" + Cycle.Id);
            }

            LoadInputs();

            return new PageResult();
        }

        public IActionResult OnPost(string id)
        {
            LoadCycle(id);

            UpdateInputs();

            return Redirect("~/Cycles/Exercises/Index/" + Cycle.Id);
        }

        private void UpdateInputs()
        {
            for (int i = 0; i < Inputs.Count; i++)
            {
                CycleInput x = Inputs[i];

                CycleInput input = Cycle.Inputs.GetByPrimaryKey(x.Id);

                input.Weight = x.Weight;
                input.Reps = x.Reps;
                input.Pullback = x.Pullback;
            }

            _cycles.UpdateCycle(Cycle);

            this.FlashInfo("Cycle Inputs Saved Successfully");
        }

        private void LoadCycle(string id)
        {
            Cycle = _cycles.GetCycle(id);
        }

        private void LoadInputs()
        {
            Inputs = Cycle.Inputs
                .Where(x => x.RequiresInput)
                .OrderBy(x => x.DisplayName)
                .ToList();

            var all = _logs.GetLogExerciseCollection();

            //  do we need to lookup stuff
            foreach (var input in Inputs.Where(x => x.OneRepMax == 0))
            {
                var log = all.FilterMax(input.Id);

                //  find the max
                double orm = log.OneRepMax;

                WorkoutSet set = log.Sets
                    .Select(x => new WorkoutSet(x))
                    .First(x => x.OneRepMax == orm);

                input.Weight = (int)set.Weight;
                input.Reps = set.Reps;

                TimeSpan time = DateTime.Now.Date - log.Date;

                if (time.TotalDays > 90)
                {
                    input.Pullback = 10;
                }
            }
        }

        public IEnumerable<SelectListItem> GetRepsSelectList(int reps)
        {
            return WebConstants.GetRepsSelectListItems(reps);
        }

        public IEnumerable<SelectListItem> GetPullbackSelectList(int pullback)
        {
            return WebConstants.GetPullbackSelectListItems(pullback);
        }

        #endregion

        #region Properties

        [BindProperty]
        public Cycle Cycle { get; set; }

        [BindProperty]
        public IList<CycleInput> Inputs { get; set; }

        #endregion
    }
}