using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Vyger.Common.Models;
using Vyger.Common.Services;

namespace Vyger.Tests.Common.Services
{
    [TestClass]
    public class ExerciseServiceTests : BaseServiceTests<ExerciseService>
    {
        #region Helpers/Test Initializers

        [TestInitialize]
        public override void TestInitialize()
        {
            base.TestInitialize();
        }

        #endregion

        #region Get Tests

        [TestMethod]
        public void ExerciseService_GetExercises_Should_PullFromStorage()
        {
            //  arrange
            var exercises = CreateA.Exercise.CreateCollection();

            Moxy.GetMock<IGoogleStorageService>()
                .Setup(x => x.GetContents<IList<Exercise>>(ExerciseService.StorageName))
                .Returns(exercises);

            Moxy.GetMock<IIdentity>()
                .Setup(x => x.Name)
                .Returns("X");

            //  act
            var actual = SubjectUnderTest.GetExerciseCollection();

            //  assert

            Moxy.VerifyAll();
        }

        [TestMethod]
        public void ExerciseService_GetExercise_Should_PullFromStorage()
        {
            //  arrange
            var exercises = CreateA.Exercise.CreateCollection();

            var exercise = exercises.Last();

            Moxy.GetMock<IGoogleStorageService>()
                .Setup(x => x.GetContents<IList<Exercise>>(ExerciseService.StorageName))
                .Returns(exercises);

            Moxy.GetMock<IIdentity>()
                .Setup(x => x.Name)
                .Returns("X");

            //  act
            var actual = SubjectUnderTest.GetExercise(exercise.Id);

            //  assert
            actual.Should().BeSameAs(exercise);

            Moxy.VerifyAll();
        }

        [TestMethod]
        public void ExerciseService_GetExercise_Should_ReturnNull()
        {
            //  arrange
            var exercises = CreateA.Exercise.CreateCollection(0);

            var exercise = CreateA.Exercise.Create();

            Moxy.GetMock<IGoogleStorageService>()
                .Setup(x => x.GetContents<IList<Exercise>>(ExerciseService.StorageName))
                .Returns(exercises);

            Moxy.GetMock<IIdentity>()
                .Setup(x => x.Name)
                .Returns("X");

            //  act
            var actual = SubjectUnderTest.GetExercise(exercise.Id);

            //  assert
            actual.Should().BeNull();

            Moxy.VerifyAll();
        }

        #endregion

        #region Save Tests

        [TestMethod]
        public void ExerciseService_SaveExercises_Should_PushToStorage()
        {
            //  arrange
            var exercises = CreateA.Exercise.CreateCollection();

            SetupCacheHit(exercises);

            Moxy.GetMock<IGoogleStorageService>()
                .Setup(x => x.StoreContents(ExerciseService.StorageName, exercises));

            Moxy.GetMock<IIdentity>()
                .Setup(x => x.Name)
                .Returns("X");

            //  act
            SubjectUnderTest.SaveExercises();

            //  assert

            Moxy.VerifyAll();
        }

        [TestMethod]
        public void ExerciseService_UpdateExercise_Should_PushToStorage()
        {
            //  arrange
            var exercises = CreateA.Exercise.CreateCollection(1);

            var exercise = exercises.Last();

            SetupCacheHit(exercises);

            Moxy.GetMock<IGoogleStorageService>()
                .Setup(x => x.StoreContents(ExerciseService.StorageName, exercises));

            Moxy.GetMock<IIdentity>()
                .Setup(x => x.Name)
                .Returns("X");

            //  act
            SubjectUnderTest.UpdateExercise(exercise);

            //  assert
            exercises.Count.Should().Be(1);

            Moxy.VerifyAll();
        }

        [TestMethod]
        public void ExerciseService_CreateExercise_Should_AddToStorage()
        {
            //  arrange
            var exercises = CreateA.Exercise.CreateCollection(0);

            var exercise = CreateA.Exercise.Create();

            SetupCacheHit(exercises);

            Moxy.GetMock<IGoogleStorageService>()
                .Setup(x => x.StoreContents(ExerciseService.StorageName, exercises));

            Moxy.GetMock<IIdentity>()
                .Setup(x => x.Name)
                .Returns("X");

            //  act
            SubjectUnderTest.UpdateExercise(exercise);

            //  assert
            exercises.Count.Should().Be(1);

            Moxy.VerifyAll();
        }

        #endregion
    }
}