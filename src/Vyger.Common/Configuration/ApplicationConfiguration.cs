namespace Vyger.Common.Configuration
{
    public class ApplicationConfiguration : BaseConfiguration
    {
        #region Constructors

        public ApplicationConfiguration() : base("Application")
        {
            Name = GetAppSetting("Name");
        }

        #endregion

        #region Properties

        /// <summary>
        /// Name of the application
        /// </summary>
        public string Name { get; }

        #endregion
    }
}