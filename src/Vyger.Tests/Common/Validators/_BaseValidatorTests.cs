using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Vyger.Tests.Common.Validators
{
    [TestClass]
    public class BaseValidatorTests<TValidator>
        where TValidator : class
    {
        #region Helpers/Test Initializers

        protected Moxy Moxy;
        protected TValidator SubjectUnderTest;

        [TestInitialize]
        public virtual void TestInitialize()
        {
            Moxy = new Moxy();

            SubjectUnderTest = Moxy.CreateInstance<TValidator>();
        }

        #endregion
    }
}