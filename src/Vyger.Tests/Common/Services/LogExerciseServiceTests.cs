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
    public class LogExerciseServiceTests : BaseServiceTests<LogExerciseService>
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
        public void LogExerciseService_GetLogExercises_Should_PullFromStorage()
        {
            //  arrange
            var exercises = CreateA.LogExercise.CreateCollection();

            Moxy.GetMock<IGoogleStorageService>()
                .Setup(x => x.GetContents<IList<LogExercise>>(LogExerciseService.StorageName))
                .Returns(exercises);

            Moxy.GetMock<IIdentity>()
                .Setup(x => x.Name)
                .Returns("X");

            //  act
            var actual = SubjectUnderTest.GetLogExerciseCollection();

            //  assert

            Moxy.VerifyAll();
        }

        [TestMethod]
        public void LogExerciseService_GetLogExercise_Should_PullFromStorage()
        {
            //  arrange
            var exercises = CreateA.LogExercise.CreateCollection();

            var exercise = exercises.Last();

            Moxy.GetMock<IGoogleStorageService>()
                .Setup(x => x.GetContents<IList<LogExercise>>(LogExerciseService.StorageName))
                .Returns(exercises);

            Moxy.GetMock<IIdentity>()
                .Setup(x => x.Name)
                .Returns("X");

            //  act
            var actual = SubjectUnderTest.GetLogExercise(exercise.Id);

            //  assert
            actual.Should().BeSameAs(exercise);

            Moxy.VerifyAll();
        }

        [TestMethod]
        public void LogExerciseService_GetLogExercise_Should_ReturnNull()
        {
            //  arrange
            var exercises = CreateA.LogExercise.CreateCollection(0);

            var exercise = CreateA.LogExercise.Create();

            Moxy.GetMock<IGoogleStorageService>()
                .Setup(x => x.GetContents<IList<LogExercise>>(LogExerciseService.StorageName))
                .Returns(exercises);

            Moxy.GetMock<IIdentity>()
                .Setup(x => x.Name)
                .Returns("X");

            //  act
            var actual = SubjectUnderTest.GetLogExercise(exercise.Id);

            //  assert
            actual.Should().BeNull();

            Moxy.VerifyAll();
        }

        #endregion

        #region Save Tests

        [TestMethod]
        public void LogExerciseService_SaveLogExercises_Should_PushToStorage()
        {
            //  arrange
            var exercises = CreateA.LogExercise.CreateCollection();

            SetupCacheHit(exercises);

            Moxy.GetMock<IGoogleStorageService>()
                .Setup(x => x.StoreContents(LogExerciseService.StorageName, exercises));

            Moxy.GetMock<IIdentity>()
                .Setup(x => x.Name)
                .Returns("X");

            //  act
            SubjectUnderTest.SaveLogExercises();

            //  assert

            Moxy.VerifyAll();
        }

        [TestMethod]
        public void LogExerciseService_UpdateLogExercise_Should_PushToStorage()
        {
            //  arrange
            var exercises = CreateA.LogExercise.CreateCollection(1);

            var exercise = exercises.Last();

            SetupCacheHit(exercises);

            Moxy.GetMock<IGoogleStorageService>()
                .Setup(x => x.StoreContents(LogExerciseService.StorageName, exercises));

            Moxy.GetMock<IIdentity>()
                .Setup(x => x.Name)
                .Returns("X");

            //  act
            SubjectUnderTest.UpdateLogExercise(exercise);

            //  assert
            exercises.Count.Should().Be(1);

            Moxy.VerifyAll();
        }

        [TestMethod]
        public void LogExerciseService_CreateLogExercise_Should_AddToStorage()
        {
            //  arrange
            var exercises = CreateA.LogExercise.CreateCollection(0);

            var exercise = CreateA.LogExercise.Create();

            SetupCacheHit(exercises);

            Moxy.GetMock<IGoogleStorageService>()
                .Setup(x => x.StoreContents(LogExerciseService.StorageName, exercises));

            Moxy.GetMock<IIdentity>()
                .Setup(x => x.Name)
                .Returns("X");

            //  act
            SubjectUnderTest.UpdateLogExercise(exercise);

            //  assert
            exercises.Count.Should().Be(1);

            Moxy.VerifyAll();
        }

        #endregion
    }
}