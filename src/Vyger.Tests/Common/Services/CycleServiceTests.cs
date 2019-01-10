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
    public class CycleServiceTests : BaseServiceTests<CycleService>
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
        public void CycleService_GetCycles_Should_PullFromStorage()
        {
            //  arrange
            var cycles = CreateA.Cycle.CreateCollection();

            Moxy.GetMock<IGoogleStorageService>()
                .Setup(x => x.GetContents<IList<Cycle>>(CycleService.StorageName))
                .Returns(cycles);

            Moxy.GetMock<IIdentity>()
                .Setup(x => x.Name)
                .Returns("X");

            //  act
            var actual = SubjectUnderTest.GetCycleCollection();

            //  assert

            Moxy.VerifyAll();
        }

        [TestMethod]
        public void CycleService_GetCycle_Should_PullFromStorage()
        {
            //  arrange
            var cycles = CreateA.Cycle.CreateCollection();

            var cycle = cycles.Last();

            Moxy.GetMock<IGoogleStorageService>()
                .Setup(x => x.GetContents<IList<Cycle>>(CycleService.StorageName))
                .Returns(cycles);

            Moxy.GetMock<IIdentity>()
                .Setup(x => x.Name)
                .Returns("X");

            //  act
            var actual = SubjectUnderTest.GetCycle(cycle.Id);

            //  assert
            actual.Should().BeSameAs(cycle);

            Moxy.VerifyAll();
        }

        [TestMethod]
        public void CycleService_GetCycle_Should_ReturnNull()
        {
            //  arrange
            var cycles = CreateA.Cycle.CreateCollection(0);

            var cycle = CreateA.Cycle.Create();

            Moxy.GetMock<IGoogleStorageService>()
                .Setup(x => x.GetContents<IList<Cycle>>(CycleService.StorageName))
                .Returns(cycles);

            Moxy.GetMock<IIdentity>()
                .Setup(x => x.Name)
                .Returns("X");

            //  act
            var actual = SubjectUnderTest.GetCycle(cycle.Id);

            //  assert
            actual.Should().BeNull();

            Moxy.VerifyAll();
        }

        #endregion

        #region Save Tests

        [TestMethod]
        public void CycleService_SaveCycles_Should_PushToStorage()
        {
            //  arrange
            var cycles = CreateA.Cycle.CreateCollection();

            SetupCacheHit(cycles);

            Moxy.GetMock<IGoogleStorageService>()
                .Setup(x => x.StoreContents(CycleService.StorageName, cycles));

            Moxy.GetMock<IIdentity>()
                .Setup(x => x.Name)
                .Returns("X");

            //  act
            SubjectUnderTest.SaveCycles();

            //  assert

            Moxy.VerifyAll();
        }

        [TestMethod]
        public void CycleService_UpdateCycle_Should_PushToStorage()
        {
            //  arrange
            var cycles = CreateA.Cycle.CreateCollection(1);

            var cycle = cycles.Last();

            SetupCacheHit(cycles);

            Moxy.GetMock<IGoogleStorageService>()
                .Setup(x => x.StoreContents(CycleService.StorageName, cycles));

            Moxy.GetMock<IIdentity>()
                .Setup(x => x.Name)
                .Returns("X");

            //  act
            SubjectUnderTest.UpdateCycle(cycle);

            //  assert
            cycles.Count.Should().Be(1);

            Moxy.VerifyAll();
        }

        [TestMethod]
        public void CycleService_CreateCycle_Should_AddToStorage()
        {
            //  arrange
            var cycles = CreateA.Cycle.CreateCollection(0);

            var cycle = CreateA.Cycle.Create();

            SetupCacheHit(cycles);

            Moxy.GetMock<IGoogleStorageService>()
                .Setup(x => x.StoreContents(CycleService.StorageName, cycles));

            Moxy.GetMock<IIdentity>()
                .Setup(x => x.Name)
                .Returns("X");

            //  act
            SubjectUnderTest.UpdateCycle(cycle);

            //  assert
            cycles.Count.Should().Be(1);

            Moxy.VerifyAll();
        }

        #endregion
    }
}