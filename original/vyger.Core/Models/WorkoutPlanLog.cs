using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics;
using System.Xml.Serialization;
using Augment;

namespace vyger.Core.Models
{
    ///	<summary>
    ///
    ///	</summary>
    [XmlType("workout-plan-log")]
    [DebuggerDisplay("{DebuggerDisplay,nq}")]
    public class WorkoutPlanLog
    {
        #region Constructors

        public WorkoutPlanLog() : base()
        {
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
                string pk = $"[{WeekId}, {DayId}, {ExerciseId}]";

                string uq = $"[{Exercise?.Name}]";

                return "{0}, pk={1}, uq={2}".FormatArgs("WorkoutPlanLog", pk, uq);
            }
        }

        #endregion

        #region Methods

        /// <summary>
        ///	Overlay all properties (except primary key, audits)
        /// </summary>
        public void OverlayWith(WorkoutPlanLog other)
        {
            SequenceNumber = other.SequenceNumber;
            WorkoutPlan = other.WorkoutPlan;
            Status = other.Status;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="logDate"></param>
        /// <returns></returns>
        public WorkoutLog ToWorkoutLog(DateTime logDate)
        {
            WorkoutLog log = new WorkoutLog()
            {
                LogDate = logDate,
                Exercise = Exercise,
                Workout = WorkoutPlan,
                SequenceNumber = SequenceNumber
            };

            return log;
        }

        #endregion

        #region Properties

        ///	<summary>
        ///	Gets / Sets database column 'cycle_id'
        ///	</summary>
        [Key]
        [DisplayName("Cycle Id")]
        [XmlAttribute("cycle-id")]
        public int CycleId { get; set; }

        ///	<summary>
        ///	Gets / Sets database column 'week_id'
        ///	</summary>
        [Key]
        [DisplayName("Week Id")]
        [XmlAttribute("week-id")]
        public int WeekId { get; set; }

        ///	<summary>
        ///	Gets / Sets database column 'day_id'
        ///	</summary>
        [Key]
        [DisplayName("Day Id")]
        [XmlAttribute("day-id")]
        public int DayId { get; set; }

        ///	<summary>
        ///	Gets / Sets database column 'exercise_id'
        ///	</summary>
        [Key]
        [DisplayName("Exercise Id")]
        [XmlAttribute("exercise-id")]
        public string ExerciseId { get; set; }

        ///	<summary>
        ///	Gets / Sets database column 'sequence_number'
        ///	</summary>
        [DisplayName("Sequence Number")]
        [XmlAttribute("sequence-number")]
        public int SequenceNumber { get; set; }

        ///	<summary>
        ///	Gets / Sets database column 'workout_plan'
        ///	</summary>
        [DisplayName("Workout Plan")]
        [XmlAttribute("workout-plan")]
        public string WorkoutPlan
        {
            get { return _workoutPlan; }
            set
            {
                _workoutPlan = WorkoutPlanLogSetCollection.Format(value.AssertNotNull());

                _sets = null;
            }
        }

        private string _workoutPlan;

        /// <summary>
        ///
        /// </summary>
        [XmlAttribute("status")]
        public StatusTypes Status { get; set; }

        #endregion

        #region Foreign Key Properties

        ///	<summary>
        ///	Gets / Sets the foreign key to 'plan_id'
        ///	</summary>
        [XmlIgnore]
        public WorkoutPlan Plan { get; set; }

        ///	<summary>
        ///	Gets / Sets the foreign key to 'exercise_id'
        ///	</summary>
        [XmlIgnore]
        public Exercise Exercise
        {
            get
            {
                if (Plan == null || Plan.Routine == null || Plan.Routine.AllExercises == null)
                {
                    return null;
                }

                Exercise ex = null;

                Plan.Routine.AllExercises.TryGetByPrimaryKey(ExerciseId, out ex);

                return ex;
            }
        }

        /// <summary>
        ///
        /// </summary>
        /// <returns></returns>
        [XmlIgnore]
        public WorkoutPlanLogSetCollection Sets
        {
            get
            {
                if (_sets == null)
                {
                    _sets = new WorkoutPlanLogSetCollection(WorkoutPlan);
                }
                return _sets;
            }
        }

        private WorkoutPlanLogSetCollection _sets;

        #endregion
    }
}