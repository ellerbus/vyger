using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using FluentAssertions;
using vyger.Core.Models;

namespace vyger.Tests.Core.Models
{
    [TestClass]
    public class WorkoutPlanLogTests
    {
        #region Methods

        [TestMethod]
        public void WorkoutPlanLog_OverlayWith_ShouldNot_IncludePrimaryKey()
        {
            var a = Design.One<WorkoutPlanLog>().Build();
            var b = Design.One<WorkoutPlanLog>().Build();

            a.OverlayWith(b);

            a.SequenceNumber.Should().Be(b.SequenceNumber);
            a.WorkoutPlan.Should().Be(b.WorkoutPlan);
            a.Status.Should().Be(b.Status);

            a.WeekId.Should().NotBe(b.WeekId);
            a.DayId.Should().NotBe(b.DayId);
            a.ExerciseId.Should().NotBe(b.ExerciseId);
        }

        #endregion
    }
}
