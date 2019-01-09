using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Vyger.Web.Pages
{
    [AllowAnonymous]
    public class PrivacyModel : PageModel
    {
        public void OnGet()
        {
        }
    }
}