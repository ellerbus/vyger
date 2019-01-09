using System;
using System.Collections.Generic;
using System.Linq;
using EnsureThat;
using vyger.Core.Models;

namespace vyger.Core.Services
{
    #region Service interface

    /// <summary>
    /// Service Interface for Exercise
    /// </summary>
    public interface IPlanGeneratorService
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="routine"></param>
        /// <returns></returns>
        WorkoutPlan CreateWorkoutPlan(WorkoutRoutine routine);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="plan"></param>
        void CreateWorkoutCycle(WorkoutPlan plan);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="plan"></param>
        /// <param name="cycleId"></param>
        void GenerateCurrentCycle(WorkoutPlan plan);
    }

    #endregion

    /// <summary>
    /// Service Implementation for Exercise
    /// </summary>
    public class WorkoutPlanGeneratorService : IPlanGeneratorService
    {
        #region Members

        private IWorkoutLogService _logs;

        #endregion

        #region Constructors

        /// <summary>
        /// Creates a new instance
        /// </summary>
        public WorkoutPlanGeneratorService(IWorkoutLogService logs)
        {
            _logs = logs;
        }

        #endregion

        #region Create Plan Methods

        /// <summary>
        ///
        /// </summary>
        public WorkoutPlan CreateWorkoutPlan(WorkoutRoutine routine)
        {
            WorkoutPlan plan = new WorkoutPlan()
            {
                Id = routine.Plans.Count + 1,
                Routine = routine,
                CreatedAt = DateTime.UtcNow
            };

            CreateWorkoutCycle(plan);

            return plan;
        }

        /// <summary>
        ///
        /// </summary>
        public void CreateWorkoutCycle(WorkoutPlan plan)
        {
            int nextCycleId = plan.CurrentCycleId + 1;

            IList<WorkoutLog> historicalLogs = GetHistoricalLogs(plan).ToList();

            LoadWorkoutCycle(plan, nextCycleId, historicalLogs);
        }

        private IEnumerable<WorkoutLog> GetHistoricalLogs(WorkoutPlan plan)
        {
            if (plan.CurrentCycleId == 0)
            {
                //  new cycle - get last 90 days
                return _logs
                    .GetWorkoutLogs()
                    .Filter(DateTime.Today.AddDays(-90), DateTime.Today);
            }

            return _logs
                .GetWorkoutLogs()
                .Filter(plan.Routine.Id, plan.Id, plan.CurrentCycleId);
        }

        private void LoadWorkoutCycle(WorkoutPlan plan, int cycleId, IList<WorkoutLog> logs)
        {
            HashSet<string> exercises = new HashSet<string>();

            IEnumerable<WorkoutRoutineExercise> routineExercises = plan.Routine.RoutineExercises;

            foreach (WorkoutRoutineExercise routineExercise in routineExercises)
            {
                if (!exercises.Contains(routineExercise.ExerciseId))
                {
                    bool calculated = plan.Routine.RoutineExercises
                        .Where(x => x.ExerciseId == routineExercise.ExerciseId)
                        .SelectMany(x => x.Sets)
                        .Any(x => x.IsRepMax);

                    WorkoutPlanCycle planCycle = new WorkoutPlanCycle()
                    {
                        CycleId = cycleId,
                        ExerciseId = routineExercise.ExerciseId,
                        IsCalculated = calculated
                    };

                    exercises.Add(routineExercise.ExerciseId);

                    plan.PlanCycles.Add(planCycle);

                    MergePreviousCycle(plan, cycleId, planCycle);

                    CreateCycleGoal(plan, cycleId, planCycle, logs);
                }
            }
        }

        private void MergePreviousCycle(WorkoutPlan plan, int cycleId, WorkoutPlanCycle planCycle)
        {
            WorkoutPlanCycle previous = plan.PlanCycles.Fetch(cycleId - 1, planCycle.ExerciseId);

            if (previous != null && previous.IsCalculated)
            {
                planCycle.Weight = previous.Weight;
                planCycle.Reps = previous.Reps;
            }
        }

        private void CreateCycleGoal(WorkoutPlan plan, int cycleId, WorkoutPlanCycle planCycle, IList<WorkoutLog> previousCycleLogs)
        {
            WorkoutLog max = null;

            //  within the history find the best 1RM
            foreach (WorkoutLog log in previousCycleLogs.Where(x => x.ExerciseId == planCycle.ExerciseId))
            {
                if (max == null || log.OneRepMax > max.OneRepMax)
                {
                    max = log;
                }
            }

            if (max != null)
            {
                WorkoutLogSet maxSet = max.Sets.GetOneRepMax();

                if (planCycle.Weight > 0)
                {
                    if (maxSet.OneRepMax > planCycle.OneRepMax)
                    {
                        //  increased =-> new goal to target
                        planCycle.Weight = maxSet.Weight;
                        planCycle.Reps = maxSet.Reps;
                    }
                    else if (maxSet.OneRepMax == planCycle.OneRepMax)
                    {
                        //  stagnent - pullback 5%
                        planCycle.Weight = maxSet.Weight;
                        planCycle.Reps = maxSet.Reps;
                        planCycle.Pullback = 5;
                    }
                    else
                    {
                        //  never hit the target - pullback 10%
                        planCycle.Pullback = 10;
                    }
                }
                else if (cycleId == 1)
                {
                    //  obviously no previous target
                    //  take the best and pullback 10%
                    planCycle.Weight = maxSet.Weight;
                    planCycle.Reps = maxSet.Reps;
                    planCycle.Pullback = 10;
                }
            }
        }

        #endregion

        #region Generate Plan Lgs

        /// <summary>
        ///
        /// </summary>
        public void GenerateCurrentCycle(WorkoutPlan plan)
        {
            //  TODO check to see if already logged !

            int cycleId = plan.CurrentCycleId;

            IList<WorkoutPlanCycle> cycle = plan.PlanCycles.Filter(cycleId).ToList();

            WorkoutPlanLogCollection logs = CreateWorkoutPlanLogs(plan);

            CalculateWorkoutPlanMaxes(plan, logs, cycle);

            CalculateWorkoutSameDayReferences(plan, logs, cycle);

            CalculateWorkoutSameWeekReferences(plan, logs, cycle);

            CalculateWorkoutSameCycleReferences(plan, logs, cycle);

            ReplaceWorkoutPlanLogs(plan, logs);
        }

        private WorkoutPlanLogCollection CreateWorkoutPlanLogs(WorkoutPlan plan)
        {
            WorkoutPlanLogCollection logs = new WorkoutPlanLogCollection(plan);

            foreach (WorkoutRoutineExercise exercise in plan.Routine.RoutineExercises)
            {
                WorkoutPlanLog log = new WorkoutPlanLog()
                {
                    CycleId = plan.CurrentCycleId,
                    WeekId = exercise.WeekId,
                    DayId = exercise.DayId,
                    ExerciseId = exercise.ExerciseId,
                    SequenceNumber = exercise.SequenceNumber,
                    WorkoutPlan = exercise.WorkoutRoutine
                };

                logs.Add(log);
            }

            return logs;
        }

        private void CalculateWorkoutPlanMaxes(WorkoutPlan plan, WorkoutPlanLogCollection logs, IList<WorkoutPlanCycle> cycle)
        {
            foreach (WorkoutPlanCycle c in cycle)
            {
                foreach (WorkoutPlanLog log in logs.Where(x => x.ExerciseId == c.ExerciseId))
                {
                    for (int i = 0; i < log.Sets.Count; i++)
                    {
                        WorkoutPlanLogSet set = log.Sets[i];

                        if (set.IsRepMax)
                        {
                            double weight = WorkoutCalculator.Prediction(c.OneRepMax, set.RepMax);

                            weight = WorkoutCalculator.Round(weight * set.Percent);

                            string pattern = $"{weight:0}/{set.Reps}/{set.Sets}";

                            log.Sets[i] = new WorkoutPlanLogSet(pattern);
                        }
                    }

                    log.WorkoutPlan = log.Sets.Display;
                }
            }
        }

        private void CalculateWorkoutSameDayReferences(WorkoutPlan plan, WorkoutPlanLogCollection logs, IList<WorkoutPlanCycle> cycle)
        {
            foreach (WorkoutPlanLog log in logs)
            {
                for (int i = 0; i < log.Sets.Count; i++)
                {
                    WorkoutPlanLogSet set = log.Sets[i];

                    if (set.IsReference && set.WeekId == Constants.Referencing.Same && set.DayId == Constants.Referencing.Same)
                    {
                        WorkoutPlanLogSet reference = FindWorkoutPlanReference(plan, logs, log, set, i + 1);

                        double weight = reference.Weight * set.Percent;

                        weight = WorkoutCalculator.Round(weight);

                        string pattern = $"{weight:0}/{set.Reps}/{set.Sets}";

                        log.Sets[i] = new WorkoutPlanLogSet(pattern);
                    }
                }

                log.WorkoutPlan = log.Sets.Display;
            }
        }

        private void CalculateWorkoutSameWeekReferences(WorkoutPlan plan, WorkoutPlanLogCollection logs, IList<WorkoutPlanCycle> cycle)
        {
            foreach (WorkoutPlanLog log in logs)
            {
                for (int i = 0; i < log.Sets.Count; i++)
                {
                    WorkoutPlanLogSet set = log.Sets[i];

                    if (set.IsReference && set.WeekId == Constants.Referencing.Same)
                    {
                        WorkoutPlanLogSet reference = FindWorkoutPlanReference(plan, logs, log, set, i + 1);

                        double weight = reference.Weight * set.Percent;

                        weight = WorkoutCalculator.Round(weight);

                        string pattern = $"{weight:0}/{set.Reps}/{set.Sets}";

                        log.Sets[i] = new WorkoutPlanLogSet(pattern);
                    }
                }

                log.WorkoutPlan = log.Sets.Display;
            }
        }

        private void CalculateWorkoutSameCycleReferences(WorkoutPlan plan, WorkoutPlanLogCollection logs, IList<WorkoutPlanCycle> cycle)
        {
            foreach (WorkoutPlanLog log in logs)
            {
                for (int i = 0; i < log.Sets.Count; i++)
                {
                    WorkoutPlanLogSet set = log.Sets[i];

                    if (set.IsReference)
                    {
                        WorkoutPlanLogSet reference = FindWorkoutPlanReference(plan, logs, log, set, i + 1);

                        double weight = reference.Weight * set.Percent;

                        weight = WorkoutCalculator.Round(weight);

                        string pattern = $"{weight:0}/{set.Reps}/{set.Sets}";

                        log.Sets[i] = new WorkoutPlanLogSet(pattern);
                    }
                }

                log.WorkoutPlan = log.Sets.Display;
            }
        }

        private WorkoutPlanLogSet FindWorkoutPlanReference(WorkoutPlan plan, WorkoutPlanLogCollection logs, WorkoutPlanLog owner, WorkoutPlanLogSet referencer, int index)
        {
            int weekId = GetReferenceIndex(referencer.WeekId, plan.Routine.Weeks, owner.WeekId);
            int dayId = GetReferenceIndex(referencer.DayId, plan.Routine.Days, owner.DayId);

            foreach (WorkoutPlanLog log in logs)
            {
                if (log.ExerciseId == owner.ExerciseId)
                {
                    if (log.WeekId == weekId && log.DayId == dayId)
                    {
                        int setId = GetReferenceIndex(referencer.SetId, log.Sets.Count, index);

                        return log.Sets[setId - 1];
                    }
                }
            }

            throw new InvalidOperationException($"Couldn't Find Reference {referencer.Reference}");
        }

        private int GetReferenceIndex(int target, int last, int same)
        {
            switch (target)
            {
                case 1:
                case 2:
                case 3:
                case 4:
                case 5:
                case 6:
                case 7:
                case 8:
                case 9:
                    return target;

                case Constants.Referencing.Same:
                    Ensure.That(same, nameof(same)).IsNot(-99);
                    return same;

                case Constants.Referencing.First:
                    return 1;

                case Constants.Referencing.Last:
                    return last;

                case Constants.Referencing.Previous:
                    return same - 1;

                case Constants.Referencing.Next:
                    return same + 1;
            }

            throw new InvalidOperationException($"Couldn't Find Reference {target}");
        }

        private void ReplaceWorkoutPlanLogs(WorkoutPlan plan, WorkoutPlanLogCollection logs)
        {
            IList<WorkoutPlanLog> existing = plan.PlanLogs.Filter(plan.CurrentCycleId).ToList();

            foreach (WorkoutPlanLog log in existing)
            {
                plan.PlanLogs.Remove(log);
            }

            foreach (WorkoutPlanLog log in logs)
            {
                plan.PlanLogs.Add(log);
            }
        }

        #endregion
    }
}