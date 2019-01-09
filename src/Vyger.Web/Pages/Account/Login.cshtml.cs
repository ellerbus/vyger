using System.Net;
using Vyger.Common.Models;
using Vyger.Common.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Vyger.Web.Pages.Account
{
    public class LoginModel : PageModel
    {
        #region Members

        private IGoogleAuthenticationService _google;

        #endregion

        #region Constructors

        public LoginModel(IGoogleAuthenticationService google)
        {
            _google = google;
        }

        #endregion

        #region Methods

        public void OnGet(string returnUrl = "", bool loggedout = false)
        {
            LoggedOut = loggedout;

            Member = new Member();

            TempData["ReturnUrl"] = returnUrl ?? "/";
        }

        public void OnPost()
        {
            this.FlashWarning("Only Google Login is supported");
        }

        #endregion

        #region Properties

        public string GoogleLoginUrl
        {
            get
            {
                string url = this.GetGoogleRedirect();

                string encodedUrl = WebUtility.UrlEncode(url);

                return _google.LoginUrl(encodedUrl);
            }
        }

        public bool LoggedOut { get; set; }

        [BindProperty]
        public Member Member { get; set; }

        #endregion
    }
}