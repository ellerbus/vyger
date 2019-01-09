using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Vyger.Common.Models;
using Vyger.Common.Services;

namespace Vyger.Web.Pages.Routines
{
    [Authorize]
    public class IndexModel : PageModel
    {
        #region Members

        private IRoutineService _routines;

        #endregion

        #region Constructors

        public IndexModel(IRoutineService routines)
        {
            _routines = routines;
        }

        #endregion

        #region Methods

        public void OnGet()
        {
            Routines = _routines.GetRoutineCollection();
        }

        #endregion

        #region Properties

        [BindProperty]
        public RoutineCollection Routines { get; set; }

        #endregion
    }
}