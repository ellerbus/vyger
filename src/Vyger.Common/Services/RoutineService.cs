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
    public interface IRoutineService
    {
        ///	<summary>
        ///	Gets many Routines
        ///	</summary>
        RoutineCollection GetRoutineCollection();

        ///	<summary>
        ///	Get an Routine
        ///	</summary>
        Routine GetRoutine(string id);

        ///	<summary>
        ///	Get an Routine
        ///	</summary>
        void CreateRoutine(Routine routine);

        ///	<summary>
        ///	Get an Routine
        ///	</summary>
        void UpdateRoutine(Routine routine);

        ///	<summary>
        ///	Saves many Routines
        ///	</summary>
        void SaveRoutines();
    }

    public class RoutineService : IRoutineService
    {
        #region Members

        public const string StorageName = "routines.json";

        private IIdentity _identity;
        private IGoogleStorageService _storage;
        private ICacheManager _cache;

        #endregion

        #region Constructors

        public RoutineService(
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

        public Routine GetRoutine(string id)
        {
            Routine routine = null;

            GetRoutineCollection().TryGetByPrimaryKey(id, out routine);

            return routine;
        }

        public RoutineCollection GetRoutineCollection()
        {
            return _cache.Cache(() => BuildRoutines())
                .DurationOf(20.Minutes(), CacheExpiration.Sliding)
                .By(_identity.Name)
                .CachedObject;
        }

        private RoutineCollection BuildRoutines()
        {
            IList<Routine> list = _storage.GetContents<IList<Routine>>(StorageName);

            return new RoutineCollection(list);
        }

        public void CreateRoutine(Routine routine)
        {
            RoutineCollection routines = GetRoutineCollection();

            while (routines.ContainsPrimaryKey(routine.Id))
            {
                routine.Id = Generators.RoutineId();
            }

            UpdateRoutine(routine);
        }

        public void UpdateRoutine(Routine routine)
        {
            Routine cache = GetRoutine(routine.Id);

            if (cache == null)
            {
                GetRoutineCollection().Add(routine);
            }
            else
            {
                cache.OverlayFrom(routine);
            }

            SaveRoutines();
        }

        public void SaveRoutines()
        {
            RoutineCollection routines = GetRoutineCollection();

            _storage.StoreContents(StorageName, routines);
        }

        #endregion
    }
}