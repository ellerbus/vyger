﻿using System.Web.Mvc;
using vyger.Common;

namespace vyger.Controllers
{
    [RoutePrefix("Home"), MvcAuthorizeRoles(Constants.Roles.ActiveMember)]
    public partial class HomeController : BaseController
    {
        [HttpGet, Route("Index"), Route("~/")]
        public virtual ActionResult Index()
        {
            return View();
        }
    }
}