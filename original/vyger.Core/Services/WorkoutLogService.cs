using System.Collections.Generic;
using Augment;
using Augment.Caching;
using vyger.Core;
using vyger.Core.Models;
using vyger.Core.Repositories;

namespace vyger.Core.Services
{
    #region Service interface

    /// <summary>
    /// Service Interface for WorkoutPlan
    /// </summary>
    public interface IWorkoutLogService
    {
        /// <summary>
        ///
        /// </summary>
        WorkoutLogCollection GetWorkoutLogs();

        /// <summary>
        ///
        /// </summary>
        void SaveWorkoutLogs();
    }

    #endregion

    /// <summary>
    /// Service Implementation for WorkoutLog
    /// </summary>
    public class WorkoutLogService : IWorkoutLogService
    {
        #region Members

        private IExerciseService _exercises;
        private ISecurityActor _actor;
        private IWorkoutLogRepository _repository;
        private ICacheManager _cache;

        #endregion

        #region Constructors

        /// <summary>
        /// Creates a new instance
        /// </summary>
        public WorkoutLogService(
            IExerciseService exercises,
            ISecurityActor actor,
            IWorkoutLogRepository repository,
            ICacheManager cache)
        {
            _exercises = exercises;
            _actor = actor;
            _repository = repository;
            _cache = cache;
        }

        #endregion

        #region Methods

        /// <summary>
        ///
        /// </summary>
        public WorkoutLogCollection GetWorkoutLogs()
        {
            WorkoutLogCollection routines = _cache
                .Cache(() => BuildLogCollection())
                .By("Actor", _actor.Email)
                .DurationOf(5.Minutes())
                .CachedObject;

            return routines;
        }

        /// <summary>
        /// Creates a new instance
        /// </summary>
        public WorkoutLogCollection BuildLogCollection()
        {
            ExerciseCollection ex = _exercises.GetExercises();

            IEnumerable<WorkoutLog> logs = _repository.GetWorkoutLogs();

            return new WorkoutLogCollection(ex, logs);
        }

        public void SaveWorkoutLogs()
        {
            WorkoutLogCollection logs = GetWorkoutLogs();

            _repository.SaveWorkoutLogs(logs);
        }

        #endregion
    }
}