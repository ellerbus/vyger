using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Xml.Serialization;
using Augment;

namespace vyger.Core.Models
{
    ///	<summary>
    ///
    ///	</summary>
    [XmlRoot("workout-plan-exercises")]
    [DebuggerDisplay("{DebuggerDisplay,nq}")]
    public class WorkoutPlanCycleCollection : Collection<WorkoutPlanCycle>
    {
        #region Constructors

        public WorkoutPlanCycleCollection(WorkoutPlan plan)
        {
            Plan = plan;
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

        protected override void InsertItem(int index, WorkoutPlanCycle item)
        {
            base.InsertItem(index, item);

            UpdateReferences(item);
        }

        protected override void SetItem(int index, WorkoutPlanCycle item)
        {
            base.SetItem(index, item);

            UpdateReferences(item);
        }

        private void UpdateReferences(WorkoutPlanCycle item)
        {
            item.Plan = Plan;
        }

        public IEnumerable<WorkoutPlanCycle> Filter(int cycleId)
        {
            return this.Where(x => x.CycleId == cycleId);
        }

        public WorkoutPlanCycle Fetch(int cycleId, string exerciseId)
        {
            return this.FirstOrDefault(x => x.CycleId == cycleId && x.ExerciseId == exerciseId);
        }

        #endregion

        #region Foreign Key Properties

        /// <summary>
        ///
        /// </summary>
        [XmlIgnore]
        public WorkoutPlan Plan { get; private set; }

        #endregion
    }
}