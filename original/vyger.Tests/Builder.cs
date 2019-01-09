using System;
using System.Linq;
using FizzWare.NBuilder;
using FizzWare.NBuilder.PropertyNaming;
using vyger.Core.Models;

namespace vyger.Tests
{
    static class Design
    {
        static Design()
        {
            BuilderSettings settings = new BuilderSettings()
            {
                AutoNameProperties = true
            };

            BuilderSetup.SetDefaultPropertyName(new RandomValuePropertyNamer(settings));
        }

        public static ISingleObjectBuilder<T> One<T>()
        {
            return Builder<T>.CreateNew();
        }

        public static IListBuilder<T> Many<T>(int size = 5)
        {
            return Builder<T>.CreateListOfSize(size);
        }
    }

    static class BuildAn
    {
        public static ExerciseCollection ExerciseCollection(int size = 5)
        {
            var exercises = Design.Many<Exercise>(size)
                .All()
                .Build();

            return new ExerciseCollection(exercises);
        }
    }

    static class BuildA
    {
        //public static WorkoutRoutine WorkoutRoutine(int numberWeeks = 4, int numberDays = 3, int numberExercises = 5)
        //{
        //    var exercises = BuildAn.ExerciseCollection(numberExercises);

        //    var routine = Design.One<WorkoutRoutine>()
        //        .With(x => x.Weeks = numberWeeks)
        //        .And(x => x.Days = numberDays)
        //        .And(x => x.AllExercises = exercises)
        //        .Build();


        //    //  apply all exercises over weeks & days
        //    for (int week = 1; week <= numberWeeks; week++)
        //    {
        //        for (int day = 1; day <= numberDays; day++)
        //        {
        //            foreach (var ex in exercises)
        //            {
        //                var rex = Design.One<WorkoutRoutineExercise>()
        //                    .With(x => x.ExerciseId = ex.Id)
        //                    .And(x => x.WeekId = week)
        //                    .And(x => x.DayId = day)
        //                    .And(x => x.Routine = routine)
        //                    .Build();

        //                routine.RoutineExercises.Add(rex);
        //            }
        //        }
        //    }

        //    return routine;
        //}

        //public static WorkoutPlan WorkoutPlanFrom(WorkoutRoutine routine, int numberOfCycles = 1)
        //{
        //    var plan = new WorkoutPlan(routine);

        //    for (int cycle = 1; cycle <= numberOfCycles; cycle++)
        //    {
        //        plan.Cycles.Add(new WorkoutPlanCycle() { CycleId = cycle });
        //    }

        //    //cycle.GenerateExerciseSetup(new WorkoutLog[0]);

        //    //var pex = cycle.PlanExercises.First();

        //    return plan;
        //}

        //public static Plan CompletePlan(int numberOfCycles = 1, int numberWeeks = 4, int numberDays = 3, int numberExercises = 5)
        //{
        //    var routine = CompleteRoutine(numberWeeks, numberDays, numberExercises);

        //    var plan = new Plan(routine)
        //    {
        //        StartDate = DateTime.Now.GetNextSunday()
        //    };

        //    foreach (var cycle in plan.Cycles)
        //    {
        //        cycle.CreateExercisesForCycle();
        //    }

        //    return plan;
        //}
    }
}
