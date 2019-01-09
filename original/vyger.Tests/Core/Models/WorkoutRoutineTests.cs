using System;
using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using vyger.Core;
using vyger.Core.Models;

namespace vyger.Tests.Core.Models
{
    [TestClass]
    public class WorkoutRoutineTests
    {
        #region Methods

        [TestMethod]
        public void WorkoutRoutine_OverlayWith_ShouldNot_IncludePrimaryKey()
        {
            var a = Design.One<WorkoutRoutine>().Build();
            var b = Design.One<WorkoutRoutine>().Build();

            a.OverlayWith(b);

            a.Name.Should().Be(b.Name);
            a.Weeks.Should().Be(b.Weeks);
            a.Days.Should().Be(b.Days);

            a.Id.Should().NotBe(b.Id);
        }

        #endregion

        #region Collection Tests

        [TestMethod]
        public void WorkoutRoutineCollection_GetPrimaryKey_ShouldBe_TheId()
        {
            //  arrange
            var routine = Design.One<WorkoutRoutine>().Build();

            var exercises = new ExerciseCollection();

            var routines = new WorkoutRoutineCollection(exercises, new[] { routine });

            //  act
            var actual = routines.GetByPrimaryKey(routine.Id);

            //  assert
            actual.Should().BeSameAs(routine);
        }

        [TestMethod]
        public void WorkoutRoutineCollection_UpdateReferences_Should_SetAllExercises()
        {
            //  arrange
            var routine = Design.One<WorkoutRoutine>().Build();

            var exercises = new ExerciseCollection();

            var routines = new WorkoutRoutineCollection(exercises, new[] { routine });

            //  act
            var actual = routines.GetByPrimaryKey(routine.Id);

            //  assert
            actual.Should().BeSameAs(routine);

            actual.AllExercises.Should().BeSameAs(exercises);
        }

        #endregion
    }
}
