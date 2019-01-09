using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Security.Principal;
using System.Text;
using System.Web.Hosting;
using Augment;

namespace vyger.Core
{
    public interface ISecurityActor : IPrincipal
    {
        string[] GetRoles();

        string Email { get; }

        bool IsAuthenticated { get; }

        string AccessToken { get; }

        string ProfileFolder { get; }
    }

    class SecurityIdentity : IIdentity
    {
        public string Name { get; set; }

        public string AuthenticationType { get; set; }

        public bool IsAuthenticated { get; set; }
    }

    public class SecurityActor : ISecurityActor
    {
        #region Constructors

        public SecurityActor(string email, bool? isAuthenticated, string accessToken)
        {
            Email = email.AssertNotNull().ToLower();

            IsAuthenticated = isAuthenticated.GetValueOrDefault();

            AccessToken = accessToken;

            Identity = new SecurityIdentity()
            {
                Name = email,
                AuthenticationType = "vyger",
                IsAuthenticated = isAuthenticated.GetValueOrDefault()
            };
        }

        #endregion

        #region Methods

        public string[] GetRoles()
        {
            List<string> roles = new List<string>() { };

            if (Email.IsNotEmpty())
            {
                roles.Add(Constants.Roles.ActiveMember);
                roles.Add("Token:" + AccessToken);
            }

            return roles.ToArray();
        }

        public bool IsInRole(string role)
        {
            return GetRoles().Any(x => x == role);
        }

        #endregion

        #region Properties

        /// <summary>
        /// 
        /// </summary>
        public IIdentity Identity { get; private set; }

        /// <summary>
        /// Login EMail
        /// </summary>
        public string Email
        {
            get
            {
                return _email.AssertNotNull().ToLower();
            }
            private set { _email = value; }
        }
        private string _email;

        /// <summary>
        /// Whether or not we're authenticated
        /// </summary>
        public bool IsAuthenticated { get; private set; }

        /// <summary>
        /// Current Google Access Token
        /// </summary>
        public string AccessToken { get; private set; }

        /// <summary>
        /// Folder of profile data (local pathing)
        /// </summary>
        public string ProfileFolder
        {
            get
            {
                StringBuilder hash = new StringBuilder();

                if (IsAuthenticated && Email.IsNotEmpty())
                {
                    using (MD5CryptoServiceProvider md5 = new MD5CryptoServiceProvider())
                    {
                        byte[] bytes = md5.ComputeHash(new UTF8Encoding().GetBytes(Email));

                        for (int i = 0; i < bytes.Length; i++)
                        {
                            hash.Append(bytes[i].ToString("x2"));
                        }
                    }
                }

                return HostingEnvironment.MapPath($"~/App_Data/") + hash.ToString();
            }
        }

        #endregion
    }
}