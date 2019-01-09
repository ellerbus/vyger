namespace Vyger.Common.Configuration
{
    public class IdentityConfiguration : BaseConfiguration
    {
        #region Constructors

        public IdentityConfiguration() : base("Identity")
        {
            GoogleClientId = GetAppSetting("GoogleClientId");
            GoogleSecret = GetAppSetting("GoogleSecret");
        }

        #endregion

        #region Properties

        /// <summary>
        /// Name of the application
        /// </summary>
        public string GoogleClientId { get; }

        /// <summary>
        /// Name of the application
        /// </summary>
        public string GoogleSecret { get; }

        #endregion
    }
}