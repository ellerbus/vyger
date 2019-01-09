using System.IO;
using System.Web.Hosting;

namespace vyger.Core.Repositories
{
    public abstract class BaseRepository
    {
        #region Constructor

        protected BaseRepository(ISecurityActor actor)
        {
            Actor = actor;
        }

        #endregion

        #region Properties

        protected ISecurityActor Actor { get; private set; }

        protected abstract string FileName { get; }

        #endregion

        #region Loaders/Savers

        protected T ReadData<T>()
        {
            if (Actor.IsAuthenticated)
            {
                string fullpath = GetFullPath<T>();

                if (File.Exists(fullpath))
                {
                    string xml = File.ReadAllText(fullpath);

                    return Serializers.FromXml<T>(xml);
                }
            }

            return default(T);
        }

        protected void SaveData<T>(T value)
        {
            string xml = Serializers.ToXml(value);

            string fullpath = GetFullPath<T>();

            File.WriteAllText(fullpath, xml);
        }

        private string GetFullPath<T>()
        {
            return Path.Combine(Actor.ProfileFolder, typeof(T).Name + ".xml");
        }

        #endregion
    }
}