using System.IO;
using System.Web.Hosting;

namespace vyger.Core.Repositories
{
    public class FileSystemRepository
    {
        #region Members

        private ISecurityActor _actor;

        #endregion

        #region Constructor

        public FileSystemRepository(ISecurityActor actor)
        {
            _actor = actor;
        }

        #endregion

        #region Loaders/Savers

        #endregion
    }
}
