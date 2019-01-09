using System;
using System.IO;
using System.Web;
using System.Web.Hosting;
using System.Web.Mvc;
using System.Web.Security;
using Augment;
using vyger.Core;
using vyger.Core.Services;

namespace vyger.Controllers
{
    [RoutePrefix("Members")]
    public partial class MembersController : BaseController
    {
        #region Members

        private IGoogleService _google;

        #endregion

        #region Constructors

        public MembersController(IGoogleService google)
        {
            _google = google;
        }

        #endregion

        #region Google Authentication

        [HttpGet, Route("Login"), AllowAnonymous]
        public ActionResult GoogleLogin()
        {
            string redirectUrl = GetLoginRedirectUrl();

            string scope = GetScope();

            string googleLoginUrl = _google.LoginUrl(redirectUrl, scope);

            return Redirect(googleLoginUrl);
        }

        [HttpGet, Route("Google"), AllowAnonymous]
        public ActionResult GoogleAuthenticate(string code, string state)
        {
            string authenticatedUrl = GetLoginRedirectUrl();

            string scope = GetScope();

            ISecurityActor sa = _google.Authenticate(authenticatedUrl, code);

            FormsAuthenticationTicket ticket = Authenticated(sa);

            string url = TempData["ReturnUrl"] as string;

            if (url.IsNullOrEmpty())
            {
                url = FormsAuthentication.GetRedirectUrl(ticket.Name, false);
            }

            return Redirect(url);
        }

        private string GetLoginRedirectUrl()
        {
            return Url.Action("GoogleAuthenticate", null, null, Request.Url.Scheme);
        }

        private string GetScope()
        {
            string[] scopes = _google.GetScopesRequired();

            return Url.Encode(scopes.Join(" "));
        }

        private FormsAuthenticationTicket Authenticated(ISecurityActor sa)
        {
            Session["SECURITY_ACTOR"] = sa;

            _google.DownloadContents(sa);

            FormsAuthenticationTicket ticket = sa.ToAuthenticationTicket();

            string encryptedTicket = FormsAuthentication.Encrypt(ticket);

            HttpCookie cookie = new HttpCookie(FormsAuthentication.FormsCookieName, encryptedTicket);

            Response.Cookies.Add(cookie);

            return ticket;
        }

        #endregion

        #region Methods

        [HttpGet, Route("Logout"), AllowAnonymous]
        public ActionResult Logout()
        {
            FormsAuthentication.SignOut();

            return RedirectToAction("Index");
        }

        [HttpGet, Route("AccessDenied"), AllowAnonymous]
        public ActionResult AccessDenied()
        {
            TempData["ReturnUrl"] = Request.QueryString["ReturnUrl"];

            return View();
        }

        [HttpGet, Route("Index"), MvcAuthorizeRoles(Constants.Roles.Administrator)]
        public ActionResult Index()
        {
            return View();
        }

        #endregion
    }
}