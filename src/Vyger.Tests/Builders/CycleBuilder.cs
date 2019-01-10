using System;
using FluentBuilders.Core;
using Vyger.Common.Models;

namespace Vyger.Tests
{
    ///	<summary>
    ///
    ///	</summary>
    public class CycleBuilder : Builder<Cycle>
    {
        #region Methods

        public static implicit operator Cycle(CycleBuilder builder)
        {
            return builder.Create();
        }

        public CycleCollection CreateCollection(int count = 5)
        {
            CycleCollection collection = new CycleCollection(CreateMany(count));
            
            return collection;
        }

        public override Cycle Create(int seed = 1)
        {
            return base.Create(seed);
        }

        protected override Cycle Build(int seed)
        {
            Cycle cycle = new Cycle()
            { 
                Id = OptInFor(x => x.Id, () => "Id" + seed), 
                Name = OptInFor(x => x.Name, () => "Name" + seed), 
                Weeks = OptInFor(x => x.Weeks, () => seed), 
                Days = OptInFor(x => x.Days, () => seed), 
                Sequence = OptInFor(x => x.Sequence, () => seed), 
                LastLogged = OptInFor(x => x.LastLogged, () => "LastLogged" + seed)
            };

            return cycle;
        }

        #endregion

        #region Properties
        
        //public CycleBuilder WithId(string id)
        //{
        //    OptInWith(x => x.Id, id);
        //
        //    return this;
        //}
        
        //public CycleBuilder WithName(string name)
        //{
        //    OptInWith(x => x.Name, name);
        //
        //    return this;
        //}
        
        //public CycleBuilder WithWeeks(int weeks)
        //{
        //    OptInWith(x => x.Weeks, weeks);
        //
        //    return this;
        //}
        
        //public CycleBuilder WithDays(int days)
        //{
        //    OptInWith(x => x.Days, days);
        //
        //    return this;
        //}
        
        //public CycleBuilder WithSequence(int sequence)
        //{
        //    OptInWith(x => x.Sequence, sequence);
        //
        //    return this;
        //}
        
        //public CycleBuilder WithLastLogged(string lastLogged)
        //{
        //    OptInWith(x => x.LastLogged, lastLogged);
        //
        //    return this;
        //}
        
        #endregion
    }
}