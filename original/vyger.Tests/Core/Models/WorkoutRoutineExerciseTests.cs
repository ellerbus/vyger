using System;
using System.Linq;
using FizzWare.NBuilder;
using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using vyger.Core.Models;

namespace vyger.Tests.Core.Models
{
    [TestClass]
    public class WorkoutRoutineExerciseTests
    {
        #region Methods

        [TestMethod]
        public void WorkoutRoutineExercise_OverlayWith_ShouldNot_IncludePrimaryKey()
        {
            var a = Design.One<WorkoutRoutineExercise>().Build();
            var b = Design.One<WorkoutRoutineExercise>().Build();

            a.OverlayWith(b);

            a.SequenceNumber.Should().Be(b.SequenceNumber);
            a.WorkoutRoutine.Should().Be(b.WorkoutRoutine);

            a.WeekId.Should().NotBe(b.WeekId);
            a.DayId.Should().NotBe(b.DayId);
            a.ExerciseId.Should().NotBe(b.ExerciseId);
        }

        #endregion

        #region Collection Tests

        [TestMethod]
        public void WorkoutRoutineExerciseCollection_UpdateReferences_Should_SetRoutine()
        {
            var routine = Design.One<WorkoutRoutine>().Build();

            var rex = new WorkoutRoutineExercise();

            routine.RoutineExercises.Add(rex);

            rex.Routine.Should().BeSameAs(routine);
        }

        [TestMethod]
        public void WorkoutRoutineExercise_Exercise_Should_PullFromRoutine()
        {
            var routine = Design.One<WorkoutRoutine>().Build();

            routine.AllExercises = BuildAn.ExerciseCollection();

            var last = routine.AllExercises.Last();

            var rex = new WorkoutRoutineExercise()
            {
                ExerciseId = last.Id
            };

            routine.RoutineExercises.Add(rex);

            rex.Exercise.Should().BeSameAs(last);

            rex.Routine.Should().BeSameAs(routine);
        }

        #endregion
    }
}
