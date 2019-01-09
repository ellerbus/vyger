using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using Augment;
using EnsureThat;
using vyger.Core;
using vyger.Core.Models;

namespace vyger
{
    public static class HttpRequestExtensions
    {
        #region Members

        private static readonly string[] exclusionPatterns =
                {
                    "ALL_.*",
                    "APPL_.*",
                    "CERT_.*",
                    "INSTANCE_.*",
                    ".*PASSWORD.*",
                    "PATH_TRANSLATED",
                    "SERVER_SOFTWARE",
                    "HTTP_COOKIE",
                    "HTTP_AUTHORIZATION",
                    "__VIEW.*",
                    "__EVENT.*"
                };

        private static readonly string regexExclusionPattern = "^((" + exclusionPatterns.Join(")|(") + "))$";

        private static readonly Regex regexExclusions = new Regex(regexExclusionPattern, RegexOptions.Compiled | RegexOptions.IgnoreCase);

        #endregion

        #region To Key Value Pairs

        public static IDictionary<string, string> ToKeyValuePairs(this HttpRequest req)
        {
            Dictionary<string, string> pairs = new Dictionary<string, string>();

            GetPath(pairs, req);

            GetNamedValueCollection("Url", pairs, req.QueryString);

            GetNamedValueCollection("Form", pairs, req.Form);

            GetNamedValueCollection("Cookie", pairs, req.Cookies);

            GetNamedValueCollection("Header", pairs, req.Headers);

            GetNamedValueCollection("Server", pairs, req.ServerVariables);

            return pairs;
        }

        private static void GetPath(Dictionary<string, string> pairs, HttpRequest req)
        {
            string host = req.Url.Host;

            if (req.ApplicationPath.IsNotEmpty() && req.ApplicationPath != "/")
            {
                host += req.ApplicationPath;
            }

            pairs.Add("Host", host);
            pairs.Add("Path", req.Path);
            pairs.Add("Raw-Url", req.RawUrl);
            pairs.Add("Http-Method", req.HttpMethod);
            pairs.Add("Content-Type", req.ContentType);
        }

        private static void GetNamedValueCollection(string prefix, Dictionary<string, string> pairs, HttpCookieCollection cookies)
        {
            if (cookies != null && cookies.AllKeys.Length > 0)
            {
                for (int i = 0; i < cookies.AllKeys.Length; i++)
                {
                    string key = cookies.AllKeys[i];

                    if (!regexExclusions.IsMatch(key))
                    {
                        pairs.Add($"{prefix} [{key}]", cookies[key].Value);
                    }
                }
            }
        }

        private static void GetNamedValueCollection(string prefix, Dictionary<string, string> pairs, NameValueCollection values)
        {
            if (values != null && values.AllKeys.Length > 0)
            {
                for (int i = 0; i < values.AllKeys.Length; i++)
                {
                    string key = values.AllKeys[i];

                    if (!regexExclusions.IsMatch(key))
                    {
                        string pairKey = $"{prefix} [{key}]";

                        if (pairs.ContainsKey(pairKey))
                        {
                            pairs[pairKey] += "\n" + values[key];
                        }
                        else
                        {
                            pairs.Add(pairKey, values[key]);
                        }
                    }
                }
            }
        }

        #endregion
    }

    public static class StringExtensions
    {
        #region Methods

        public static string ToYMD(this DateTime date)
        {
            return date.ToString("yyyy-MM-dd");
        }

        #endregion
    }

    public static class WebExtensions
    {
        public static FormsAuthenticationTicket ToAuthenticationTicket(this ISecurityActor sa)
        {
            Ensure.That(sa.Email, nameof(ISecurityActor.Email)).IsNotEmpty();

            FormsAuthenticationTicket ticket = new FormsAuthenticationTicket(
                version: 1,
                name: sa.Email,
                issueDate: DateTime.Now,
                expiration: DateTime.Now.AddMinutes(20),
                isPersistent: false,
                userData: sa.GetRoles().Join("|")
                );

            return ticket;
        }

        public static IEnumerable<SelectListItem> ToSelectList(this IEnumerable<Exercise> exercises)
        {
            IEnumerable<Exercise> ordered = exercises
                .OrderBy(x => x.Group)
                .ThenBy(x => x.Category)
                .ThenBy(x => x.Name);

            foreach (Exercise exercise in ordered)
            {
                yield return new SelectListItem()
                {
                    Value = exercise.Id.ToString(),
                    Text = exercise.DetailName
                };
            }
        }
    }

    public static class HtmlExtensions
    {
        private const string ScopeKey = "HtmlExtensions_BeginFieldScope_Keys_";

        public static IDisposable BeginFieldScope(this HtmlHelper html, object item, string propertyCollection)
        {
            string scopeName = propertyCollection;

            string scopeIndex = GetScopeIndex(html, item, scopeName);

            string input = "<input type='hidden' name='{0}.index' autocomplete='off' value='{1}' />";

            // autocomplete="off" is needed to work around a very annoying Chrome behaviour
            // whereby it reuses old values after the user clicks "Back", which causes the
            // xyz.index and xyz[...] values to get out of sync.
            html.ViewContext.Writer.WriteLine(input, scopeName, html.Encode(scopeIndex));

            return new HtmlFieldScope(html.ViewData.TemplateInfo, $"{scopeName}[{scopeIndex}]");
        }

        private static string GetScopeIndex(HtmlHelper html, object item, string scopeName)
        {
            Queue<string> reuseScopes = GetScopesToReuse(html.ViewContext.HttpContext, scopeName);

            if (reuseScopes.Count > 0)
            {
                return reuseScopes.Dequeue();
            }

            StringBuilder sb = new StringBuilder("I");

            foreach (var p in item.GetType().GetProperties())
            {
                if (Attribute.GetCustomAttributes(p, typeof(KeyAttribute)).Any())
                {
                    object value = p.GetValue(item);

                    sb.Append(".").Append(value);
                }
            }

            return sb.ToString();
        }

        private static Queue<string> GetScopesToReuse(HttpContextBase httpContext, string scopeName)
        {
            string key = ScopeKey + scopeName;

            Queue<string> queue = httpContext.Items[key] as Queue<string>;

            if (queue == null)
            {
                httpContext.Items[key] = queue = new Queue<string>();

                string previousScopes = httpContext.Request[scopeName + ".index"];

                if (previousScopes.IsNotEmpty())
                {
                    foreach (string previouslyUsedId in previousScopes.Split(','))
                    {
                        queue.Enqueue(previouslyUsedId);
                    }
                }
            }
            return queue;
        }

        private class HtmlFieldScope : IDisposable
        {
            public readonly TemplateInfo TemplateInfo;
            public readonly string PreviousHtmlFieldPrefix;

            public HtmlFieldScope(TemplateInfo templateInfo, string htmlFieldPrefix)
            {
                TemplateInfo = templateInfo;

                PreviousHtmlFieldPrefix = TemplateInfo.HtmlFieldPrefix;

                TemplateInfo.HtmlFieldPrefix = htmlFieldPrefix;
            }

            public void Dispose()
            {
                TemplateInfo.HtmlFieldPrefix = PreviousHtmlFieldPrefix;
            }
        }
    }
}