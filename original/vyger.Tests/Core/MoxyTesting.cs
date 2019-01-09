using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace vyger.Tests.Core
{
    public class MoxyTesting<T> where T : class
    {
        protected Moxy Moxy;
        protected T SubjectUnderTest;

        [TestInitialize]
        public virtual void TestInitialize()
        {
            Moxy = new Moxy();

            SubjectUnderTest = Moxy.CreateInstance<T>();
        }
    }
}
