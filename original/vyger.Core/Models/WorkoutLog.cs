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
    [XmlType("workout-log")]
    [DebuggerDisplay("{DebuggerDisplay,nq}")]
    public class WorkoutLog
    {
        #region Constructors

        public WorkoutLog() : base()
        {
        }

        //public WorkoutLog(WorkoutPlanLog planLog) : this()
        //{
        //    ExerciseId = planLog.PlanExercise.ExerciseId;
        //    Exercise = planLog.PlanExercise.Exercise;
        //    Workout = planLog.WorkoutPlan;
        //    PlanId = planLog.Cycle.Plan.Id;
        //    CycleId = planLog.Cycle.CycleId;
        //    WeekId = planLog.WeekId;
        //    DayId = planLog.DayId;
        //    SequenceNumber = planLog.SequenceNumber;
        //}

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
                string id = $"[{LogDate:d}, {ExerciseId}]";

                string nm = $"[{Exercise?.Name},{Workout}]";

                return "{0}, id={1}, nm={2}".FormatArgs("WorkoutLog", id, nm);
            }
        }

        #endregion

        #region Methods

        /// <summary>
        ///	Overlay all properties (except primary key, audits)
        /// </summary>
        public void OverlayWith(WorkoutLog other)
        {
            //RoutineId = other.RoutineId;
            //PlanId = other.PlanId;
            //CycleId = other.CycleId;
            //WeekId = other.WeekId;
            //DayId = other.DayId;
            SequenceNumber = other.SequenceNumber;
            Workout = WorkoutLogSetCollection.Format(other.Workout);
        }

        #endregion

        #region Properties

        ///	<summary>
        ///	Gets / Sets database column 'log_date'
        ///	</summary>
        [Key]
        [DisplayName("Log Date")]
        [XmlAttribute("log-date")]
        public DateTime LogDate { get; set; }

        ///	<summary>
        ///	Gets / Sets database column 'exercise_id'
        ///	</summary>
        [Key]
        [DisplayName("Exercise Id")]
        [XmlAttribute("exercise-id")]
        public string ExerciseId
        {
            get { return Exercise == null ? _exerciseId : Exercise.Id; }
            set { _exerciseId = value; }
        }

        private string _exerciseId;

        ///	<summary>
        ///	Gets / Sets database column 'workout'
        ///	</summary>
        [DisplayName("Workout")]
        [XmlAttribute("workout")]
        public string Workout
        {
            get { return _workout; }
            set
            {
                _workout = WorkoutLogSetCollection.Format(value.AssertNotNull());
            }
        }

        private string _workout;

        ///	<summary>
        ///	Gets / Sets database column 'routine_id'
        ///	</summary>
        [DisplayName("Routine Id")]
        [XmlAttribute("routine-id")]
        public string RoutineId { get; set; }

        ///	<summary>
        ///	Gets / Sets database column 'plan_id'
        ///	</summary>
        [DisplayName("Plan Id")]
        [XmlAttribute("plan-id")]
        public int PlanId { get; set; }

        ///	<summary>
        ///	Gets / Sets database column 'cycle_id'
        ///	</summary>
        [DisplayName("Cycle Id")]
        [XmlAttribute("cycle-id")]
        public int CycleId { get; set; }

        ///	<summary>
        ///	Gets / Sets database column 'week_id'
        ///	</summary>
        [DisplayName("Week Id")]
        [XmlAttribute("week-id")]
        public int WeekId { get; set; }

        ///	<summary>
        ///	Gets / Sets database column 'day_id'
        ///	</summary>
        [DisplayName("Day Id")]
        [XmlAttribute("day-id")]
        public int DayId { get; set; }

        ///	<summary>
        ///	Gets / Sets database column 'sequence_number'
        ///	</summary>
        [DisplayName("Sequence Number")]
        [XmlAttribute("sequence-number")]
        public int SequenceNumber { get; set; }

        /// <summary>
        ///
        /// </summary>
        [XmlIgnore]
        public double OneRepMax
        {
            get
            {
                if (Sets != null && Sets.Count > 0)
                {
                    return Sets.Max(x => x.OneRepMax);
                }

                return double.NaN;
            }
        }

        #endregion

        #region Foreign Key Properties

        ///	<summary>
        ///	Gets / Sets the foreign key to 'exercise_id'
        ///	</summary>
        [XmlIgnore]
        public Exercise Exercise { get; set; }

        /// <summary>
        ///
        /// </summary>
        /// <returns></returns>
        [XmlIgnore]
        public WorkoutLogSetCollection Sets
        {
            get
            {
                if (_sets == null && Workout.IsNotEmpty())
                {
                    _sets = new WorkoutLogSetCollection(Workout);
                }
                return _sets;
            }
        }

        private WorkoutLogSetCollection _sets;

        #endregion
    }
}