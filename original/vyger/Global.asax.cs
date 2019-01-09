using System;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Security.Principal;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using Augment;
using FluentValidation;
using FluentValidation.Mvc;
using SimpleInjector;
using SimpleInjector.Integration.Web;
using SimpleInjector.Integration.Web.Mvc;
using vyger.Core;
using vyger.Core.Services;

namespace vyger
{
    public class Global : HttpApplication
    {
        #region Members

        public class SimpleInjectorValidatorFactory : ValidatorFactoryBase
        {
            private readonly Container _container;

            public SimpleInjectorValidatorFactory(Container container)
                => _container = container;

            public override IValidator CreateInstance(Type validatorType)
            {
                try
                {
                    object validator = _container.GetInstance(validatorType);

                    return (IValidator)validator;
                }
                catch (ActivationException)
                {
                    // FluentValidation will handle null properly
                    return null;
                }
            }
        }

        private static readonly Container _container = new Injector(new WebRequestLifestyle());

        #endregion

        #region Events

        protected void Application_Start(object sender, EventArgs e)
        {
            MvcConfig.Register();

            // This is an extension method from the integration package.
            _container.RegisterMvcControllers(Assembly.GetExecutingAssembly());

            FluentValidationModelValidatorProvider.Configure(x => x.ValidatorFactory = new SimpleInjectorValidatorFactory(_container));

            _container.Verify();

            DependencyResolver.SetResolver(new SimpleInjectorDependencyResolver(_container));
        }

        protected void Application_Error(object sender, EventArgs e)
        {
            Exception ex = Server.GetLastError();

            ExceptionLogging.LogException(ex);
        }

        protected void Application_BeginRequest(object sender, EventArgs e)
        {
        }

        protected void Application_AuthenticateRequest(object sender, EventArgs eventArgs)
        {
            if (HttpContext.Current != null && HttpContext.Current.User != null)
            {
                if (HttpContext.Current.User.Identity != null)
                {
                    IIdentity id = HttpContext.Current.User.Identity;

                    if (id.IsAuthenticated)
                    {
                        HttpCookie cookie = HttpContext.Current.Request.Cookies[FormsAuthentication.FormsCookieName];

                        FormsAuthenticationTicket decodedTicket = FormsAuthentication.Decrypt(cookie.Value);

                        string[] roles = decodedTicket.UserData.Split(new[] { "|" }, StringSplitOptions.RemoveEmptyEntries);

                        string token = roles.FirstOrDefault(x => x.StartsWith("Token:"));

                        if (token.IsNotEmpty())
                        {
                            token = token.GetRightOf("Token:");
                        }

                        SecurityActor sa = new SecurityActor(id.Name, id.IsAuthenticated, token);

                        HttpContext.Current.User = sa;
                    }
                }
            }
        }

        protected void Session_OnEnd(object sender, EventArgs e)
        {
            ISecurityActor sa = Session["SECURITY_ACTOR"] as ISecurityActor;

            if (sa != null)
            {
                GoogleService google = new GoogleService();

                DirectoryInfo dir = new DirectoryInfo(sa.ProfileFolder);

                FileInfo[] files = dir.GetFiles("*.xml");

                foreach (FileInfo file in files)
                {
                    string name = file.Name;

                    string xml = File.ReadAllText(file.FullName);

                    try
                    {
                        google.UploadContents(sa, name, xml);
                    }
                    catch (Exception ex)
                    {
                        //  nothing for now
                    }
                }
            }
        }

        #endregion
    }
}