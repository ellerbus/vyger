using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Vyger.Common.Models;
using Vyger.Common.Services;

namespace Vyger.Web.Pages.Cycles
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

        public void OnGet()
        {
            Cycles = _cycles.GetCycleCollection()
                .OrderByDescending(x => x.Sequence)
                .Take(5);
        }

        #endregion

        #region Properties

        [BindProperty]
        public IEnumerable<Cycle> Cycles { get; set; }

        #endregion
    }
}