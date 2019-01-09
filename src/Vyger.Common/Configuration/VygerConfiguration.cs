namespace Vyger.Common.Configuration
{
    public interface IVygerConfiguration
    {
        string Environment { get; }

        ApplicationConfiguration Application { get; }

        IdentityConfiguration Identity { get; }
    }

    public class VygerConfiguration : BaseConfiguration, IVygerConfiguration
    {
        #region Constructors

        public VygerConfiguration() : base("")
        {
        }

        #endregion

        #region Properties

        /// <summary>
        /// Configuration specific to the application
        /// </summary>
        public ApplicationConfiguration Application { get; } = new ApplicationConfiguration();

        /// <summary>
        /// Configuration specific to the application
        /// </summary>
        public IdentityConfiguration Identity { get; } = new IdentityConfiguration();

        #endregion
    }
}