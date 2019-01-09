using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using vyger.Core.Models;

namespace vyger.Tests.Core.Models
{
    [TestClass]
    public class ExerciseTests
    {
        #region Methods

        [TestMethod]
        public void Exercise_OverlayWith_ShouldNot_IncludePrimaryKey()
        {
            var a = Design.One<Exercise>().Build();
            var b = Design.One<Exercise>().Build();

            a.OverlayWith(b);

            a.Name.Should().Be(b.Name);

            a.Category.Should().NotBe(b.Category);
            a.Group.Should().NotBe(b.Group);
            a.Id.Should().NotBe(b.Id);
        }

        #endregion

        #region Collection Tests

        [TestMethod]
        public void ExerciseCollection_GetPrimaryKey_ShouldBe_TheId()
        {
            //  arrange
            var exercise = Design.One<Exercise>().Build();

            var exercises = new ExerciseCollection(new[] { exercise });

            //  act
            var actual = exercises.GetByPrimaryKey(exercise.Id);

            //  assert
            actual.Should().BeSameAs(exercise);
        }

        #endregion
    }
}
