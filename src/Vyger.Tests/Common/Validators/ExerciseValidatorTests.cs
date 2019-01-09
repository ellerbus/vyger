using FluentValidation.TestHelper;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Vyger.Common.Models;
using Vyger.Common.Services;
using Vyger.Common.Validators;

namespace Vyger.Tests.Common.Validators
{
    [TestClass]
    public class ExerciseValidatorTests : BaseValidatorTests<ExerciseValidator>
    {
        #region Members

        [TestInitialize]
        public override void TestInitialize()
        {
            base.TestInitialize();
        }

        #endregion

        #region Validation Rule Tests

        [TestMethod]
        public void ExerciseValidator_RuleFor_Name()
        {
            var exercises = CreateA.Exercise.CreateCollection();

            Moxy.GetMock<IExerciseService>()
                .Setup(x => x.GetExerciseCollection())
                .Returns(exercises);

            var item = new Exercise();

            item.Name = default(string);
            SubjectUnderTest.ShouldNotHaveValidationErrorFor(v => v.Name, item);

            item.Name = "";
            SubjectUnderTest.ShouldHaveValidationErrorFor(v => v.Name, item);

            item.Name = new string('0', 150);
            SubjectUnderTest.ShouldNotHaveValidationErrorFor(v => v.Name, item);

            item.Name = new string('0', 150 + 1);
            SubjectUnderTest.ShouldHaveValidationErrorFor(v => v.Name, item);
        }

        #endregion
    }
}