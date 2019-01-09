using System.Collections.Generic;
using System.Security.Principal;
using Augment;
using Augment.Caching;
using Vyger.Common.Models;

namespace Vyger.Common.Services
{
    ///	<summary>
    ///
    ///	</summary>
    public interface IExerciseService
    {
        ///	<summary>
        ///	Gets many Exercises
        ///	</summary>
        ExerciseCollection GetExerciseCollection();

        ///	<summary>
        ///	Get an Exercise
        ///	</summary>
        Exercise GetExercise(string id);

        ///	<summary>
        ///	Get an Exercise
        ///	</summary>
        void CreateExercise(Exercise exercise);

        ///	<summary>
        ///	Get an Exercise
        ///	</summary>
        void UpdateExercise(Exercise exercise);

        ///	<summary>
        ///	Saves many Exercises
        ///	</summary>
        void SaveExercises();
    }

    public class ExerciseService : IExerciseService
    {
        #region Members

        public const string StorageName = "exercises.json";

        private IIdentity _identity;
        private IGoogleStorageService _storage;
        private ICacheManager _cache;

        #endregion

        #region Constructors

        public ExerciseService(
            IIdentity identity,
            IGoogleStorageService storage,
            ICacheManager cache)
        {
            _identity = identity;
            _storage = storage;
            _cache = cache;
        }

        #endregion

        #region Methods

        public Exercise GetExercise(string id)
        {
            Exercise exercise = null;

            GetExerciseCollection().TryGetByPrimaryKey(id, out exercise);

            return exercise;
        }

        public ExerciseCollection GetExerciseCollection()
        {
            return _cache.Cache(() => BuildExercises())
                .DurationOf(20.Minutes(), CacheExpiration.Sliding)
                .By(_identity.Name)
                .CachedObject;
        }

        private ExerciseCollection BuildExercises()
        {
            IList<Exercise> list = _storage.GetContents<IList<Exercise>>(StorageName);

            return new ExerciseCollection(list);
        }

        public void CreateExercise(Exercise exercise)
        {
            ExerciseCollection exercises = GetExerciseCollection();

            while (exercises.ContainsPrimaryKey(exercise.Id))
            {
                exercise.Id = Generators.ExerciseId();
            }

            UpdateExercise(exercise);
        }

        public void UpdateExercise(Exercise exercise)
        {
            Exercise cache = GetExercise(exercise.Id);

            if (cache == null)
            {
                GetExerciseCollection().Add(exercise);
            }
            else
            {
                cache.OverlayFrom(exercise);
            }

            SaveExercises();
        }

        public void SaveExercises()
        {
            ExerciseCollection exercises = GetExerciseCollection();

            _storage.StoreContents(StorageName, exercises);
        }

        #endregion
    }
}