using Microsoft.VisualStudio.TestTools.UnitTesting;
using FluentAssertions;
using vyger.Core.Models;

namespace vyger.Tests.Core.Models
{
    [TestClass]
    public class WorkoutLogTests
    {
        #region Methods

        [TestMethod]
        public void WorkoutLog_OverlayWith_ShouldNot_IncludePrimaryKey()
        {
            var a = Design.One<WorkoutLog>().Build();
            var b = Design.One<WorkoutLog>().Build();

            a.OverlayWith(b);

            a.PlanId.Should().Be(b.PlanId);
            a.CycleId.Should().Be(b.CycleId);
            a.WeekId.Should().Be(b.WeekId);
            a.DayId.Should().Be(b.DayId);
            a.SequenceNumber.Should().Be(b.SequenceNumber);
            a.Workout.Should().Be(b.Workout);

            a.LogDate.Should().NotBe(b.LogDate);
            a.ExerciseId.Should().NotBe(b.ExerciseId);
        }

        #endregion
    }
}
