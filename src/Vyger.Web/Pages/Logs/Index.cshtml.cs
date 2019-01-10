using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Vyger.Common;
using Vyger.Common.Models;
using Vyger.Common.Services;

namespace Vyger.Web.Pages.Logs
{
    [Authorize]
    public class IndexModel : PageModel
    {
        #region Members

        private ILogExerciseService _logs;

        #endregion

        #region Constructors

        public IndexModel(ILogExerciseService Logs)
        {
            _logs = Logs;
        }

        #endregion

        #region Methods

        public void OnGet()
        {
            //Logs = _logs.GetRoutineCollection().OrderBy(x => x.Name);
        }

        public IActionResult OnGetEvents(DateTime start, DateTime end)
        {
            //Logs = _logs.GetRoutineCollection().OrderBy(x => x.Name);

            var events = _logs.GetLogExerciseCollection()
                .FilterDateRange(start, end)
                .Select(x => x.Date.ToYMD())
                .Distinct()
                .Select(x => new { start = x, title = "Workout Details", url = Url.Page("/Logs/Exercises/Index", new { date = x }) });

            return new JsonResult(events);
        }

        #endregion

        #region Properties

        [BindProperty]
        public IEnumerable<LogExercise> Logs { get; set; }

        #endregion
    }
}