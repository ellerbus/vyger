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
    [XmlRoot("workout-plan-logs")]
    [DebuggerDisplay("{DebuggerDisplay,nq}")]
    public class WorkoutPlanLogCollection : Collection<WorkoutPlanLog>
    {
        #region Constructors

        public WorkoutPlanLogCollection(WorkoutPlan plan)
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

        protected override void InsertItem(int index, WorkoutPlanLog item)
        {
            base.InsertItem(index, item);

            UpdateReferences(item);
        }

        protected override void SetItem(int index, WorkoutPlanLog item)
        {
            base.SetItem(index, item);

            UpdateReferences(item);
        }

        private void UpdateReferences(WorkoutPlanLog item)
        {
            item.Plan = Plan;
        }

        public IEnumerable<WorkoutPlanLog> Filter(int cycleId)
        {
            return this.Where(x => x.CycleId == cycleId);
        }

        public IEnumerable<WorkoutPlanLog> Filter(int cycleId, int weekId, int dayId)
        {
            return this
                .Where(x => x.CycleId == cycleId && x.WeekId == weekId && x.DayId == dayId)
                .OrderBy(x => x.SequenceNumber);
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