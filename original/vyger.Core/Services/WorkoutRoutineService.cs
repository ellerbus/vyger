using System.Collections.Generic;
using Augment;
using Augment.Caching;
using vyger.Core.Models;
using vyger.Core.Repositories;

namespace vyger.Core.Services
{
    #region Service interface

    /// <summary>
    /// Service Interface for WorkoutRoutine
    /// </summary>
    public interface IWorkoutRoutineService
    {
        /// <summary>
        ///
        /// </summary>
        WorkoutRoutineCollection GetWorkoutRoutines();

        /// <summary>
        /// 
        /// </summary>
        void SaveWorkoutRoutines();
    }

    #endregion

    /// <summary>
    /// Service Implementation for WorkoutRoutine
    /// </summary>
    public class WorkoutRoutineService : IWorkoutRoutineService
    {
        #region Members

        private IExerciseService _exercises;
        private ISecurityActor _actor;
        private IWorkoutRoutineRepository _repository;
        private ICacheManager _cache;

        #endregion

        #region Constructors

        /// <summary>
        /// Creates a new instance
        /// </summary>
        public WorkoutRoutineService(
            IExerciseService exercises,
            ISecurityActor actor,
            IWorkoutRoutineRepository repository,
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
        public WorkoutRoutineCollection GetWorkoutRoutines()
        {
            WorkoutRoutineCollection routines = _cache
                .Cache(() => BuildRoutineCollection())
                .By("Actor", _actor.Email)
                .DurationOf(5.Minutes())
                .CachedObject;

            return routines;
        }

        /// <summary>
        ///
        /// </summary>
        public WorkoutRoutineCollection BuildRoutineCollection()
        {
            ExerciseCollection exercises = _exercises.GetExercises();

            IEnumerable<WorkoutRoutine> routines = _repository.GetWorkoutRoutines();

            return new WorkoutRoutineCollection(exercises, routines);
        }

        /// <summary>
        ///
        /// </summary>
        public void SaveWorkoutRoutines()
        {
            WorkoutRoutineCollection routines = GetWorkoutRoutines();

            _repository.SaveWorkoutRoutines(routines);
        }

        #endregion
    }
}