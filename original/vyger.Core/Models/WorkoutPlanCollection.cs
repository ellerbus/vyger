using System.Collections.Generic;
using System.Diagnostics;
using System.Xml.Serialization;
using Augment;

namespace vyger.Core.Models
{
    ///	<summary>
    ///
    ///	</summary>
    [XmlRoot("workout-plans")]
    [DebuggerDisplay("{DebuggerDisplay,nq}")]
    public class WorkoutPlanCollection : SingleKeyCollection<WorkoutPlan, int>
    {
        #region Constructors

        public WorkoutPlanCollection(WorkoutRoutine routine)
        {
            Routine = routine;
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

        protected override int GetPrimaryKey(WorkoutPlan item)
        {
            return item.Id;
        }

        protected override void InsertItem(int index, WorkoutPlan item)
        {
            base.InsertItem(index, item);

            UpdateReferences(item);
        }

        protected override void SetItem(int index, WorkoutPlan item)
        {
            base.SetItem(index, item);

            UpdateReferences(item);
        }

        private void UpdateReferences(WorkoutPlan item)
        {
            if (Routine != null)
            {
                item.Routine = Routine;
            }
        }

        public void AddRange(IEnumerable<WorkoutPlan> plans)
        {
            if (plans != null)
            {
                foreach (WorkoutPlan plan in plans)
                {
                    Add(plan);
                }
            }
        }

        #endregion

        #region Foreign Key Properties

        /// <summary>
        ///
        /// </summary>
        [XmlIgnore]
        public WorkoutRoutine Routine { get; private set; }

        #endregion
    }
}