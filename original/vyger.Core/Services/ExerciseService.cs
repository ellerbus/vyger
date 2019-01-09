using System.Collections.Generic;
using Augment;
using Augment.Caching;
using vyger.Core.Models;
using vyger.Core.Repositories;

namespace vyger.Core.Services
{
    #region Service interface

    /// <summary>
    /// Service Interface for Exercise
    /// </summary>
    public interface IExerciseService
    {
        /// <summary>
        ///
        /// </summary>
        ExerciseCollection GetExercises();

        /// <summary>
        ///
        /// </summary>
        void SaveExercises();
    }

    #endregion

    /// <summary>
    /// Service Implementation for Exercise
    /// </summary>
    public class ExerciseService : IExerciseService
    {
        #region Members

        private ISecurityActor _actor;
        private IExerciseRepository _repository;
        private ICacheManager _cache;

        #endregion

        #region Constructors

        /// <summary>
        /// Creates a new instance
        /// </summary>
        public ExerciseService(
            ISecurityActor actor,
            IExerciseRepository repository,
            ICacheManager cache)
        {
            _actor = actor;
            _repository = repository;
            _cache = cache;
        }

        #endregion

        #region Methods

        /// <summary>
        ///
        /// </summary>
        public ExerciseCollection GetExercises()
        {
            ExerciseCollection exercises = _cache
                .Cache(() => _repository.GetExercises())
                .By("Actor", _actor.Email)
                .DurationOf(5.Minutes())
                .CachedObject;

            return exercises;
        }

        public void SaveExercises()
        {
            ExerciseCollection exercises = GetExercises();

            _repository.SaveExercises(exercises);
        }

        #endregion
    }
}