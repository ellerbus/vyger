using System;
using FluentBuilders.Core;
using Vyger.Common.Models;

namespace Vyger.Tests
{
    ///	<summary>
    ///
    ///	</summary>
    public class LogExerciseBuilder : Builder<LogExercise>
    {
        #region Methods

        public static implicit operator LogExercise(LogExerciseBuilder builder)
        {
            return builder.Create();
        }

        public LogExerciseCollection CreateCollection(int count = 5)
        {
            LogExerciseCollection collection = new LogExerciseCollection(CreateMany(count));
            
            return collection;
        }

        public override LogExercise Create(int seed = 1)
        {
            return base.Create(seed);
        }

        protected override LogExercise Build(int seed)
        {
            LogExercise exercise = new LogExercise()
            { 
                Id = OptInFor(x => x.Id, () => "Id" + seed), 
                Name = OptInFor(x => x.Name, () => "Name" + seed), 
                Group = OptInFor(x => x.Group, () => "Group" + seed), 
                Category = OptInFor(x => x.Category, () => "Category" + seed), 
                Date = OptInFor(x => x.Date, () => "Date" + seed), 
                Sets = OptInFor(x => x.Sets, () => "Sets" + seed)
            };

            return exercise;
        }

        #endregion

        #region Properties
        
        //public LogExerciseBuilder WithId(string id)
        //{
        //    OptInWith(x => x.Id, id);
        //
        //    return this;
        //}
        
        //public LogExerciseBuilder WithName(string name)
        //{
        //    OptInWith(x => x.Name, name);
        //
        //    return this;
        //}
        
        //public LogExerciseBuilder WithGroup(string group)
        //{
        //    OptInWith(x => x.Group, group);
        //
        //    return this;
        //}
        
        //public LogExerciseBuilder WithCategory(string category)
        //{
        //    OptInWith(x => x.Category, category);
        //
        //    return this;
        //}
        
        //public LogExerciseBuilder WithDate(string date)
        //{
        //    OptInWith(x => x.Date, date);
        //
        //    return this;
        //}
        
        //public LogExerciseBuilder WithSets(string sets)
        //{
        //    OptInWith(x => x.Sets, sets);
        //
        //    return this;
        //}
        
        #endregion
    }
}