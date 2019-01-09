using System.Collections.Generic;
using System.Security.Claims;
using Augment;
using Vyger.Common.Models;

namespace Vyger.Common.Services
{
    #region Interface

    public interface IIdentityService
    {
        ClaimsPrincipal CreateClaimsPrincipal(IIdentityProfile profile, string token);
    }

    #endregion

    public partial class IdentityService : IIdentityService
    {
        #region Methods

        public ClaimsPrincipal CreateClaimsPrincipal(IIdentityProfile profile, string token)
        {
            Member member = new Member()
            {
                Email = profile.Email,
                Name = profile.Name ?? profile.Email.GetLeftOf("@")
            };

            List<Claim> claims = new List<Claim>()
            {
                new Claim(ClaimTypes.Name, member.Email),
                new Claim(ClaimTypes.Email, member.Email),
                new Claim(Constants.GoogleTokenClaim, token)
            };

            ClaimsIdentity identity = new ClaimsIdentity(claims, "login");

            ClaimsPrincipal principal = new ClaimsPrincipal(identity);

            return principal;
        }

        #endregion
    }
}