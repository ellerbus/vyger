using FluentBuilders.Core;
using Vyger.Common.Models;

namespace Vyger.Tests
{
    ///	<summary>
    ///
    ///	</summary>
    public class RoutineBuilder : Builder<Routine>
    {
        #region Methods

        public static implicit operator Routine(RoutineBuilder builder)
        {
            return builder.Create();
        }

        public RoutineCollection CreateCollection(int count = 5)
        {
            RoutineCollection collection = new RoutineCollection(CreateMany(count));

            return collection;
        }

        public override Routine Create(int seed = 1)
        {
            return base.Create(seed);
        }

        protected override Routine Build(int seed)
        {
            Routine routine = new Routine()
            {
                Id = OptInFor(x => x.Id, () => "Id" + seed),
                Name = OptInFor(x => x.Name, () => "Name" + seed),
                Weeks = OptInFor(x => x.Weeks, () => seed),
                Days = OptInFor(x => x.Days, () => seed)
            };

            return routine;
        }

        #endregion

        #region Properties

        //public RoutineBuilder WithId(string id)
        //{
        //    OptInWith(x => x.Id, id);
        //
        //    return this;
        //}

        //public RoutineBuilder WithName(string name)
        //{
        //    OptInWith(x => x.Name, name);
        //
        //    return this;
        //}

        //public RoutineBuilder WithWeeks(int weeks)
        //{
        //    OptInWith(x => x.Weeks, weeks);
        //
        //    return this;
        //}

        //public RoutineBuilder WithDays(int days)
        //{
        //    OptInWith(x => x.Days, days);
        //
        //    return this;
        //}

        //public RoutineBuilder WithDefaultSets(string defaultSets)
        //{
        //    OptInWith(x => x.Sets, defaultSets);
        //
        //    return this;
        //}

        #endregion
    }
}