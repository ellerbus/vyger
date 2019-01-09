using System.Linq;
using System.Web.Mvc;
using Augment;

namespace vyger
{
    public class MvcAuthorizeRolesAttribute : AuthorizeAttribute
    {
        public MvcAuthorizeRolesAttribute(params string[] roles) : base()
        {
            Roles = roles.Join(",");
        }
    }
}