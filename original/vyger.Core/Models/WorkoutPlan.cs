using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics;
using System.Linq;
using System.Xml.Serialization;
using Augment;

namespace vyger.Core.Models
{
    ///	<summary>
    ///
    ///	</summary>
    [XmlType("workout-plan")]
    [DebuggerDisplay("{DebuggerDisplay,nq}")]
    public class WorkoutPlan
    {
        #region Constructors

        public WorkoutPlan()
        {
            PlanCycles = new WorkoutPlanCycleCollection(this);
            PlanLogs = new WorkoutPlanLogCollection(this);
        }

        #endregion

        #region ToString/DebuggerDisplay

        public override string ToString()
        {
            return DebuggerDisplay;
        }

        ///	<summary>
        ///	DebuggerDisplay for this object
        ///	</summary>
        private string DebuggerDisplay
        {
            get
            {
                string pk = $"[{Id}]";

                string uq = $"[{Routine?.Name}]";

                return "{0}, pk={1}, uq={2}".FormatArgs("WorkoutPlan", pk, uq);
            }
        }

        #endregion

        #region Methods

        /// <summary>
        ///	Overlay all properties (except primary key, audits)
        /// </summary>
        public void OverlayWith(WorkoutPlan other)
        {
        }

        #endregion

        #region Properties

        ///	<summary>
        ///	
        ///	</summary>
        [Key]
        [DisplayName("Plan Id")]
        [XmlAttribute("plan-id")]
        public int Id { get; set; }

        /// <summary>
        ///
        /// </summary>
        [XmlAttribute("created-at")]
        public DateTime CreatedAt { get; set; }

        #endregion

        #region Foreign Key Properties

        ///	<summary>
        ///	Gets / Sets the foreign key to 'routine_id'
        ///	</summary>
        [XmlIgnore]
        public WorkoutRoutine Routine { get; set; }

        /// <summary>
        ///
        /// </summary>
        [XmlArray("workout-plan-cycle"), XmlArrayItem("workout-plan-cycle")]
        public WorkoutPlanCycleCollection PlanCycles { get; private set; }

        /// <summary>
        ///
        /// </summary>
        [XmlArray("workout-plan-logs"), XmlArrayItem("workout-plan-log")]
        public WorkoutPlanLogCollection PlanLogs { get; private set; }

        /// <summary>
        /// 
        /// </summary>
        [XmlIgnore]
        public int CurrentCycleId
        {
            get
            {
                if (PlanCycles.Count == 0)
                {
                    return 0;
                }

                return PlanCycles.Max(x => x.CycleId);
            }
        }

        #endregion
    }
}