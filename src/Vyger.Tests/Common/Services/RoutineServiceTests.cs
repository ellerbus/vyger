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
    public class RoutineServiceTests : BaseServiceTests<RoutineService>
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
        public void RoutineService_GetRoutines_Should_PullFromStorage()
        {
            //  arrange
            var routines = CreateA.Routine.CreateCollection();

            Moxy.GetMock<IGoogleStorageService>()
                .Setup(x => x.GetContents<IList<Routine>>(RoutineService.StorageName))
                .Returns(routines);

            Moxy.GetMock<IIdentity>()
                .Setup(x => x.Name)
                .Returns("X");

            //  act
            var actual = SubjectUnderTest.GetRoutineCollection();

            //  assert

            Moxy.VerifyAll();
        }

        [TestMethod]
        public void RoutineService_GetRoutine_Should_PullFromStorage()
        {
            //  arrange
            var routines = CreateA.Routine.CreateCollection();

            var routine = routines.Last();

            Moxy.GetMock<IGoogleStorageService>()
                .Setup(x => x.GetContents<IList<Routine>>(RoutineService.StorageName))
                .Returns(routines);

            Moxy.GetMock<IIdentity>()
                .Setup(x => x.Name)
                .Returns("X");

            //  act
            var actual = SubjectUnderTest.GetRoutine(routine.Id);

            //  assert
            actual.Should().BeSameAs(routine);

            Moxy.VerifyAll();
        }

        [TestMethod]
        public void RoutineService_GetRoutine_Should_ReturnNull()
        {
            //  arrange
            var routines = CreateA.Routine.CreateCollection(0);

            var routine = CreateA.Routine.Create();

            Moxy.GetMock<IGoogleStorageService>()
                .Setup(x => x.GetContents<IList<Routine>>(RoutineService.StorageName))
                .Returns(routines);

            Moxy.GetMock<IIdentity>()
                .Setup(x => x.Name)
                .Returns("X");

            //  act
            var actual = SubjectUnderTest.GetRoutine(routine.Id);

            //  assert
            actual.Should().BeNull();

            Moxy.VerifyAll();
        }

        #endregion

        #region Save Tests

        [TestMethod]
        public void RoutineService_SaveRoutines_Should_PushToStorage()
        {
            //  arrange
            var routines = CreateA.Routine.CreateCollection();

            SetupCacheHit(routines);

            Moxy.GetMock<IGoogleStorageService>()
                .Setup(x => x.StoreContents(RoutineService.StorageName, routines));

            Moxy.GetMock<IIdentity>()
                .Setup(x => x.Name)
                .Returns("X");

            //  act
            SubjectUnderTest.SaveRoutines();

            //  assert

            Moxy.VerifyAll();
        }

        [TestMethod]
        public void RoutineService_UpdateRoutine_Should_PushToStorage()
        {
            //  arrange
            var routines = CreateA.Routine.CreateCollection(1);

            var routine = routines.Last();

            SetupCacheHit(routines);

            Moxy.GetMock<IGoogleStorageService>()
                .Setup(x => x.StoreContents(RoutineService.StorageName, routines));

            Moxy.GetMock<IIdentity>()
                .Setup(x => x.Name)
                .Returns("X");

            //  act
            SubjectUnderTest.UpdateRoutine(routine);

            //  assert
            routines.Count.Should().Be(1);

            Moxy.VerifyAll();
        }

        [TestMethod]
        public void RoutineService_CreateRoutine_Should_AddToStorage()
        {
            //  arrange
            var routines = CreateA.Routine.CreateCollection(0);

            var routine = CreateA.Routine.Create();

            SetupCacheHit(routines);

            Moxy.GetMock<IGoogleStorageService>()
                .Setup(x => x.StoreContents(RoutineService.StorageName, routines));

            Moxy.GetMock<IIdentity>()
                .Setup(x => x.Name)
                .Returns("X");

            //  act
            SubjectUnderTest.UpdateRoutine(routine);

            //  assert
            routines.Count.Should().Be(1);

            Moxy.VerifyAll();
        }

        #endregion
    }
}