using System.Web.Mvc;
using System.Web.Routing;

namespace vyger
{
    public static class MvcConfig
    {
        public static void Register()
        {
            ViewEngines.Engines.Clear();

            ViewEngines.Engines.Add(new CsViewEngine());

            RouteTable.Routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            RouteTable.Routes.MapMvcAttributeRoutes();
        }
    }

    class CsViewEngine : RazorViewEngine
    {
        public CsViewEngine()
        {
            AreaViewLocationFormats = new string[] { "~/Areas/{2}/Views/{1}/{0}.cshtml", "~/Areas/{2}/Views/Shared/{0}.cshtml" };
            AreaMasterLocationFormats = new string[] { "~/Areas/{2}/Views/{1}/{0}.cshtml", "~/Areas/{2}/Views/Shared/{0}.cshtml" };
            AreaPartialViewLocationFormats = new string[] { "~/Areas/{2}/Views/{1}/{0}.cshtml", "~/Areas/{2}/Views/Shared/{0}.cshtml" };
            ViewLocationFormats = new string[] { "~/Views/{1}/{0}.cshtml", "~/Views/Shared/{0}.cshtml" };
            MasterLocationFormats = new string[] { "~/Views/{1}/{0}.cshtml", "~/Views/Shared/{0}.cshtml" };
            PartialViewLocationFormats = new string[] { "~/Views/{1}/{0}.cshtml", "~/Views/Shared/{0}.cshtml" };
            FileExtensions = new string[] { "cshtml" };
        }
    }
}
