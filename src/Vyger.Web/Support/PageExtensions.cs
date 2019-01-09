using System;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Vyger.Web
{
    public static class PageExtensions
    {
        public static Uri GetAbsoluteUri(this PageModel page)
        {
            HttpRequest req = page.HttpContext.Request;

            return new Uri(req.Scheme + "://" + req.Host.Value);
        }

        public static string GetGoogleRedirect(this PageModel page)
        {
            Uri uri = new Uri(page.GetAbsoluteUri(), page.Url.Page("/Account/Google"));

            return uri.ToString();
        }

        public static SelectList CreateYesNoSelectList(this PageModel page)
        {
            SelectListItem[] items = new[]
            {
                new SelectListItem(){ Value = "True", Text = "Yes" },
                new SelectListItem(){ Value = "False", Text = "No" },
            };

            return new SelectList(items, "Value", "Text");
        }

        public static void FlashSuccess(this PageModel page, string message)
        {
            Flash(page, "success", message);
        }

        public static void FlashInfo(this PageModel page, string message)
        {
            Flash(page, "info", message);
        }

        public static void FlashWarning(this PageModel page, string message)
        {
            Flash(page, "warning", message);
        }

        public static void FlashDanger(this PageModel page, string message)
        {
            Flash(page, "danger", message);
        }

        private static void Flash(PageModel page, string type, string message)
        {
            FlashMessageCollection messages = page.TempData.Get<FlashMessageCollection>();

            if (messages == null)
            {
                messages = new FlashMessageCollection();
            }

            messages.Add(new FlashMessage(type, message));

            page.TempData.Put(messages);
        }
    }
}