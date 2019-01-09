using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using vyger.Core.Models;

namespace vyger.Tests.Core.Models
{
    [TestClass]
    public class WorkoutPlanCycleTests
    {
        #region Methods

        [TestMethod]
        public void WorkoutPlanCycle_OverlayWith_ShouldNot_IncludePrimaryKey()
        {
            var a = Design.One<WorkoutPlanCycle>().Build();
            var b = Design.One<WorkoutPlanCycle>().Build();

            a.OverlayWith(b);

            a.Status.Should().Be(b.Status);

            a.CycleId.Should().NotBe(b.CycleId);
            a.CreatedAt.Should().NotBe(b.CreatedAt);
        }

        #endregion
    }
}
