using FluentBuilders.Core;
using Vyger.Common;
using Vyger.Common.Models;

namespace Vyger.Tests
{
    ///	<summary>
    ///
    ///	</summary>
    public class ExerciseBuilder : Builder<Exercise>
    {
        #region Methods

        public static implicit operator Exercise(ExerciseBuilder builder)
        {
            return builder.Create();
        }

        public ExerciseCollection CreateCollection(int count = 5)
        {
            ExerciseCollection collection = new ExerciseCollection(CreateMany(count));

            return collection;
        }

        public override Exercise Create(int seed = 1)
        {
            return base.Create(seed);
        }

        protected override Exercise Build(int seed)
        {
            Exercise exercise = new Exercise()
            {
                Id = OptInFor(x => x.Id, () => "Id" + seed),
                Name = OptInFor(x => x.Name, () => "Name" + seed),
                Group = OptInFor(x => x.Group, () => ExerciseGroups.None),
                Category = OptInFor(x => x.Category, () => ExerciseCategories.None)
            };

            return exercise;
        }

        #endregion

        #region Properties

        //public ExerciseBuilder WithId(string id)
        //{
        //    OptInWith(x => x.Id, id);
        //
        //    return this;
        //}

        //public ExerciseBuilder WithName(string name)
        //{
        //    OptInWith(x => x.Name, name);
        //
        //    return this;
        //}

        //public ExerciseBuilder WithGroup(string group)
        //{
        //    OptInWith(x => x.Group, group);
        //
        //    return this;
        //}

        //public ExerciseBuilder WithCategory(string category)
        //{
        //    OptInWith(x => x.Category, category);
        //
        //    return this;
        //}

        #endregion
    }
}