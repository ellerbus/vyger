using System.Collections.Generic;
using System.Security;
using System.Web.Mvc;

namespace vyger.Controllers
{
    public abstract partial class BaseController : Controller
    {
        #region Methods

        public void AddFlashMessage(FlashMessageType type, string message)
        {
            object messages = null;

            if (!TempData.TryGetValue("FlashMessage", out messages))
            {
                messages = new FlashMessageCollection();

                TempData["FlashMessage"] = messages;
            }

            (messages as FlashMessageCollection).Add(new FlashMessage(type, message));
        }

        protected override void OnException(ExceptionContext filterContext)
        {
            if (filterContext.Exception is SecurityException)
            {
                AddFlashMessage(FlashMessageType.Danger, filterContext.Exception.Message);

                filterContext.ExceptionHandled = true;
                filterContext.Result = RedirectToAction("Index");
            }

            if (filterContext.Exception is KeyNotFoundException)
            {
                AddFlashMessage(FlashMessageType.Danger, filterContext.Exception.Message);

                filterContext.ExceptionHandled = true;
                filterContext.Result = RedirectToAction("Index");
            }

            base.OnException(filterContext);
        }

        #endregion
    }
}