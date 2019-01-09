using Augment.Caching;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace Vyger.Tests.Common.Services
{
    [TestClass]
    public class BaseServiceTests<TService>
        where TService : class
    {
        #region Helpers/Test Initializers

        protected Moxy Moxy;
        protected TService SubjectUnderTest;

        [TestInitialize]
        public virtual void TestInitialize()
        {
            Moxy = new Moxy();

            Mock<ICacheProvider> mockCache = Moxy.GetMock<ICacheProvider>();

            Moxy.SetInstance<ICacheManager>(new CacheManager(mockCache.Object));

            SubjectUnderTest = Moxy.CreateInstance<TService>();
        }

        protected void SetupCacheMiss<T>() where T : class
        {
            Moxy.GetMock<ICacheProvider>()
                .Setup(x => x.Get(Any.String))
                .Returns(null as T);
        }

        protected void SetupCacheHit<T>(T cachedItem) where T : class
        {
            Moxy.GetMock<ICacheProvider>()
                .Setup(x => x.Get(Any.String))
                .Returns(cachedItem);
        }

        #endregion
    }
}