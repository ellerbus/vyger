using System.Configuration;
using Augment;

namespace Vyger.Common.Configuration
{
    public class BaseConfiguration
    {
        #region Members

        private string _prefix;

        #endregion

        #region Constructors

        protected BaseConfiguration(string prefix)
        {
            _prefix = prefix;

            Environment = GetAppSetting("Environment");
        }

        #endregion

        #region Methods

        protected string GetAppSetting(string appSetting)
        {
            if (appSetting.IsSameAs("environment"))
            {
                return ConfigurationManager.AppSettings["Environment"];
            }

            if (_prefix.IsNotEmpty())
            {
                appSetting = _prefix + "." + appSetting;
            }

            string value = ConfigurationManager.AppSettings[$"{appSetting}.{Environment}"].AssertNotNull();

            //  not environment specific setting
            if (value.IsNullOrEmpty())
            {
                //  do we have an non-environment specific value to use
                value = ConfigurationManager.AppSettings[appSetting];
            }

            if (value.IsNotEmpty())
            {
                value = ReplaceMacros(value);
            }

            return value;
        }

        protected string GetConnectionString(string name)
        {
            ConnectionStringSettings setting = ConfigurationManager.ConnectionStrings[$"{name}.{Environment}"];

            return ReplaceMacros(setting.ConnectionString);
        }

        protected string ReplaceMacros(string value)
        {
            //  replace all macros
            foreach (string key in ConfigurationManager.AppSettings.AllKeys)
            {
                string macro = "{" + key + "}";

                string subsitute = ConfigurationManager.AppSettings[key];

                value = value.Replace(macro, subsitute);
            }

            return value;
        }

        #endregion

        #region Properties

        /// <summary>
        /// The environment name for this configuration object
        /// </summary>
        public string Environment { get; }

        #endregion
    }
}