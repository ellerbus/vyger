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
    public interface ICycleService
    {
        ///	<summary>
        ///	Gets many Cycles
        ///	</summary>
        CycleCollection GetCycleCollection();

        ///	<summary>
        ///	Get an Cycle
        ///	</summary>
        Cycle GetCycle(string id);

        ///	<summary>
        ///	Get an Cycle
        ///	</summary>
        void CreateCycle(Cycle cycle);

        ///	<summary>
        ///	Get an Cycle
        ///	</summary>
        void UpdateCycle(Cycle cycle);

        /// <summary>
        ///
        /// </summary>
        void CalculateCycle(Cycle cycle);

        ///	<summary>
        ///	Saves many Cycles
        ///	</summary>
        void SaveCycles();
    }

    public class CycleService : ICycleService
    {
        #region Members

        public const string StorageName = "cycles.json";

        private IIdentity _identity;
        private IGoogleStorageService _storage;
        private ICacheManager _cache;

        #endregion

        #region Constructors

        public CycleService(
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

        public Cycle GetCycle(string id)
        {
            Cycle cycle = null;

            GetCycleCollection().TryGetByPrimaryKey(id, out cycle);

            return cycle;
        }

        public CycleCollection GetCycleCollection()
        {
            return _cache.Cache(() => BuildCycles())
                .DurationOf(20.Minutes(), CacheExpiration.Sliding)
                .By(_identity.Name)
                .CachedObject;
        }

        private CycleCollection BuildCycles()
        {
            IList<Cycle> list = _storage.GetContents<IList<Cycle>>(StorageName);

            return new CycleCollection(list);
        }

        public void CreateCycle(Cycle cycle)
        {
            CycleCollection cycles = GetCycleCollection();

            while (cycles.ContainsPrimaryKey(cycle.Id))
            {
                cycle.Id = Generators.CycleId();
            }

            cycle.Sequence = cycles.Count + 1;

            UpdateCycle(cycle);
        }

        public void UpdateCycle(Cycle cycle)
        {
            Cycle cache = GetCycle(cycle.Id);

            if (cache == null)
            {
                GetCycleCollection().Add(cycle);
            }
            else
            {
                cache.OverlayFrom(cycle);
            }

            SaveCycles();
        }

        public void SaveCycles()
        {
            CycleCollection cycles = GetCycleCollection();

            _storage.StoreContents(StorageName, cycles);
        }

        #endregion

        #region Calculate Cycle

        public void CalculateCycle(Cycle cycle)
        {
            CalculateCycleMaxes(cycle);

            CalculateCycleLookups(cycle);
        }

        private void CalculateCycleMaxes(Cycle cycle)
        {
            foreach (CycleExercise exercise in cycle.Exercises)
            {
                WorkoutSet[] sets = exercise.Plan.Select(x => new WorkoutSet(x)).ToArray();

                for (int i = 0; i < sets.Length; i++)
                {
                    WorkoutSet set = sets[i];

                    if (set.Type == WorkoutSetTypes.RepMax)
                    {
                        CycleInput input = cycle.Inputs.GetByPrimaryKey(exercise.Id);

                        double pullback = 1 - (input.Pullback / 100.0);

                        double prediction = WorkoutCalculator.Prediction(input.OneRepMax, set.RepMax) * (set.Percent / 100.0);

                        prediction = prediction * pullback;

                        double weight = WorkoutCalculator.Round(prediction);

                        set.Type = WorkoutSetTypes.Static;

                        set.Weight = weight;
                    }
                }

                exercise.Plan = sets.SelectMany(x => x.Format(false)).ToArray();
            }
        }

        private void CalculateCycleLookups(Cycle cycle)
        {
            foreach (CycleExercise exercise in cycle.Exercises)
            {
                WorkoutSet[] sets = exercise.Plan.Select(x => new WorkoutSet(x)).ToArray();

                for (int i = 0; i < sets.Length; i++)
                {
                    WorkoutSet set = sets[i];

                    if (set.Type == WorkoutSetTypes.Reference)
                    {
                        if (set.Reference == "L")
                        {
                            WorkoutSet last = sets.Last();

                            double weight = WorkoutCalculator.Round(last.Weight * (set.Percent / 100.0));

                            set.Type = WorkoutSetTypes.Static;

                            set.Weight = weight;
                        }
                    }
                }

                exercise.Plan = sets.SelectMany(x => x.Format(false)).ToArray();
            }
        }

        #endregion
    }
}