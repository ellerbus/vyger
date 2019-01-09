using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Augment;

namespace Vyger.Common.Models
{
    ///	<summary>
    ///
    ///	</summary>
    [DebuggerDisplay("{DebuggerDisplay,nq}")]
    public class ExerciseCollection : SingleKeyCollection<Exercise, string>
    {
        #region Constructors

        public ExerciseCollection()
            : base(StringComparer.OrdinalIgnoreCase)
        {
        }

        public ExerciseCollection(IEnumerable<Exercise> exercises)
            : this()
        {
            if (exercises != null)
            {
                foreach (Exercise exercise in exercises)
                {
                    Add(exercise);
                }
            }
        }

        #endregion

        #region ToString/DebuggerDisplay

        ///	<summary>
        ///	DebuggerDisplay for this object
        ///	</summary>
        private string DebuggerDisplay
        {
            get { return "Count={0}".FormatArgs(Count); }
        }

        #endregion

        #region Methods

        public IEnumerable<Exercise> Filter(ExerciseGroups group, ExerciseCategories category)
        {
            return this
                .Where(x => group == ExerciseGroups.None || x.Group == group)
                .Where(x => category == ExerciseCategories.None || x.Category == category);
        }

        protected override string GetPrimaryKey(Exercise item)
        {
            return item.Id;
        }

        #endregion
    }
}