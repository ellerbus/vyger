using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Vyger.Web.Pages.Account
{
    public class LogoutModel : PageModel
    {
        public IActionResult OnGet()
        {
            Task task = HttpContext.SignOutAsync();

            task.Wait();

            return Redirect("~/Account/Login?loggedout=true");
        }
    }
}