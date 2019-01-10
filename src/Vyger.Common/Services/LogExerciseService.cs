using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using Augment;
using Augment.Caching;
using Vyger.Common.Models;

namespace Vyger.Common.Services
{
    ///	<summary>
    ///
    ///	</summary>
    public interface ILogExerciseService
    {
        ///	<summary>
        ///	Gets many LogExercises
        ///	</summary>
        LogExerciseCollection GetLogExerciseCollection();

        ///	<summary>
        ///	Get an LogExercise
        ///	</summary>
        LogExercise GetLogExercise(DateTime date, string id);

        ///	<summary>
        ///	Get an LogExercise
        ///	</summary>
        void CreateLogExercise(LogExercise exercise);

        ///	<summary>
        ///	Get an LogExercise
        ///	</summary>
        void UpdateLogExercise(LogExercise exercise);

        ///	<summary>
        ///	Saves many LogExercises
        ///	</summary>
        void SaveLogExercises();
    }

    public class LogExerciseService : ILogExerciseService
    {
        #region Members

        public const string StorageName = "logs.json";

        private IIdentity _identity;
        private IGoogleStorageService _storage;
        private ICacheManager _cache;

        #endregion

        #region Constructors

        public LogExerciseService(
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

        public LogExercise GetLogExercise(DateTime date, string id)
        {
            LogExercise exercise = GetLogExerciseCollection()
                .FilterForUpdating(date)
                .FirstOrDefault(x => x.Id.IsSameAs(id));

            return exercise;
        }

        public LogExerciseCollection GetLogExerciseCollection()
        {
            return _cache.Cache(() => BuildLogExercises())
                .DurationOf(20.Minutes(), CacheExpiration.Sliding)
                .By(_identity.Name)
                .CachedObject;
        }

        private LogExerciseCollection BuildLogExercises()
        {
            IList<LogExercise> list = _storage.GetContents<IList<LogExercise>>(StorageName);

            return new LogExerciseCollection(list);
        }

        public void CreateLogExercise(LogExercise exercise)
        {
            UpdateLogExercise(exercise);
        }

        public void UpdateLogExercise(LogExercise exercise)
        {
            LogExercise cache = GetLogExercise(exercise.Date, exercise.Id);

            if (cache == null)
            {
                GetLogExerciseCollection().Add(exercise);
            }
            else
            {
                cache.OverlayFrom(exercise);
            }

            SaveLogExercises();
        }

        public void SaveLogExercises()
        {
            LogExerciseCollection exercises = GetLogExerciseCollection();

            //_storage.StoreContents(StorageName, exercises);
        }

        #endregion
    }
}