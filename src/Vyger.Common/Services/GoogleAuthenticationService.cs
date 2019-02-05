using System.Collections.Specialized;
using System.Linq;
using System.Net;
using System.Security.Claims;
using System.Text;
using Augment;
using Newtonsoft.Json;
using Vyger.Common.Configuration;
using Vyger.Common.Models;

namespace Vyger.Common.Services
{
    #region Interface

    public interface IGoogleAuthenticationService
    {
        /// <summary>
        /// Gets the login URL for google
        /// </summary>
        /// <param name="redirectUrl"></param>
        /// <param name="scope"></param>
        /// <returns></returns>
        string LoginUrl(string redirectUrl);

        /// <summary>
        /// Authenticates against Google and creates a SecurityActor
        /// </summary>
        /// <param name="redirectUrl"></param>
        /// <param name="code"></param>
        /// <returns></returns>
        ClaimsPrincipal Authenticate(string redirectUrl, string code);
    }

    #endregion

    public partial class GoogleAuthenticationService : IGoogleAuthenticationService
    {
        #region Members

        private IIdentityService _identity;
        private IVygerConfiguration _config;

        #endregion

        #region Constructors

        public GoogleAuthenticationService(
            IIdentityService identity,
            IVygerConfiguration config)
        {
            _identity = identity;
            _config = config;
        }

        #endregion

        #region Auth Methods

        public string LoginUrl(string redirectUrl)
        {
            string scope = GetScopesRequired().Join(" ");

            string clientId = _config.Identity.GoogleClientId;

            string url = $"https://accounts.google.com/o/oauth2/v2/auth?client_id={clientId}&response_type=code&scope={scope}&redirect_uri={redirectUrl}&state=abcdef";

            return url;
        }

        public ClaimsPrincipal Authenticate(string redirectUrl, string code)
        {
            GoogleToken token = GetGoogleToken(redirectUrl, code);

            GoogleProfile profile = GetGoogleProfile(token);

            return _identity.CreateClaimsPrincipal(profile, token.AccessToken);
        }

        private GoogleToken GetGoogleToken(string redirectUrl, string code)
        {
            using (WebClient web = new WebClient())
            {
                string url = $"https://www.googleapis.com/oauth2/v4/token";

                NameValueCollection data = new NameValueCollection();

                data["code"] = code;
                data["client_id"] = _config.Identity.GoogleClientId;
                data["client_secret"] = _config.Identity.GoogleSecret;
                data["redirect_uri"] = redirectUrl;
                data["grant_type"] = "authorization_code";

                byte[] results = web.UploadValues(url, data);

                string json = Encoding.UTF8.GetString(results);

                return JsonConvert.DeserializeObject<GoogleToken>(json);
            }
        }

        private GoogleProfile GetGoogleProfile(GoogleToken token)
        {
            using (WebClient web = new WebClient())
            {
                string url = $"https://www.googleapis.com/oauth2/v3/tokeninfo?id_token={token.IdToken}";

                string json = web.DownloadString(url);

                return JsonConvert.DeserializeObject<GoogleProfile>(json);
            }
        }

        private string[] GetScopesRequired()
        {
            return new[]
            {
                "email",
                "https://www.googleapis.com/auth/drive.appdata"
            };
        }

        #endregion
    }
}