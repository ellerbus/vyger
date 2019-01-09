using System.Security.Claims;
using System.Threading.Tasks;
using Augment;
using Vyger.Common.Services;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Vyger.Web.Pages.Account
{
    public class GoogleModel : PageModel
    {
        #region Members

        private IGoogleAuthenticationService _google;

        #endregion

        #region Constructors

        public GoogleModel(IGoogleAuthenticationService google)
        {
            _google = google;
        }

        #endregion

        #region Methods

        public IActionResult OnGet(string code, string state)
        {
            ClaimsPrincipal principal = _google.Authenticate(GoogleRedirect, code);

            Task task = HttpContext.SignInAsync(principal);

            string url = TempData["ReturnUrl"] as string;

            if (url.IsNullOrEmpty())
            {
                url = "/";
            }

            task.Wait();

            return Redirect(url);
        }

        #endregion

        #region Properties

        public string GoogleRedirect
        {
            get
            {
                return this.GetGoogleRedirect();
            }
        }

        #endregion
    }
}