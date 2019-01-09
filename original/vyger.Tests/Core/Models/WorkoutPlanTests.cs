using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using FluentAssertions;
using vyger.Core.Models;

namespace vyger.Tests.Core.Models
{
    [TestClass]
    public class WorkoutPlanTests
    {
        #region Methods

        [TestMethod]
        public void WorkoutPlan_OverlayWith_ShouldNot_IncludePrimaryKey()
        {
            var a = Design.One<WorkoutPlan>().Build();
            var b = Design.One<WorkoutPlan>().Build();

            a.OverlayWith(b);

            a.Status.Should().Be(b.Status);

            a.RoutineId.Should().NotBe(b.RoutineId);
            a.CreatedAt.Should().NotBe(b.CreatedAt);
        }

        #endregion

        #region Property Tests

        #endregion
    }
}
