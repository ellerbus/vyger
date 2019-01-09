using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Xml.Serialization;
using Augment;

namespace vyger.Core.Models
{
    ///	<summary>
    ///
    ///	</summary>
    [XmlRoot("exercises")]
    [DebuggerDisplay("{DebuggerDisplay,nq}")]
    public class ExerciseCollection : SingleKeyCollection<Exercise, string>
    {
        #region Constructors

        public ExerciseCollection()
        {
        }

        public ExerciseCollection(IEnumerable<Exercise> exercises)
            : this()
        {
            AddRange(exercises);
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

        protected override void InsertItem(int index, Exercise item)
        {
            base.InsertItem(index, item);
        }

        protected override void SetItem(int index, Exercise item)
        {
            base.SetItem(index, item);
        }

        public void AddRange(IEnumerable<Exercise> exercises)
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
    }
}