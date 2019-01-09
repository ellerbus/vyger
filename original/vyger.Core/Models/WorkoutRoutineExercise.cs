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
    [XmlRoot("workout-routine-exercise")]
    [DebuggerDisplay("{DebuggerDisplay,nq}")]
    public class WorkoutRoutineExercise
    {
        #region Constructors

        public WorkoutRoutineExercise()
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
                string id = $"[{WeekId}, {DayId}, {ExerciseId}]";

                string nm = $"[{Exercise?.Name}]";

                return "{0}, id={1}, nm={2}".FormatArgs("WorkoutRoutineExercise", id, nm);
            }
        }

        #endregion

        #region Methods

        /// <summary>
        ///	Overlay all properties (except primary key, audits)
        /// </summary>
        public void OverlayWith(WorkoutRoutineExercise other)
        {
            SequenceNumber = other.SequenceNumber;
            WorkoutRoutine = WorkoutRoutineSetCollection.Format(other.WorkoutRoutine);
        }

        #endregion

        #region Properties

        ///	<summary>
        ///
        ///	</summary>
        [Key]
        [DisplayName("Week Id")]
        [XmlAttribute("week-id")]
        public int WeekId { get; set; }

        ///	<summary>
        ///
        ///	</summary>
        [Key]
        [DisplayName("Day Id")]
        [XmlAttribute("day-id")]
        public int DayId { get; set; }

        ///	<summary>
        ///
        ///	</summary>
        [Key]
        [DisplayName("Exercise Id")]
        [XmlAttribute("exercise-id")]
        public string ExerciseId { get; set; }

        ///	<summary>
        ///
        ///	</summary>
        [DisplayName("Sequence Number")]
        [XmlAttribute("sequence-number")]
        public int SequenceNumber { get; set; }

        ///	<summary>
        ///
        ///	</summary>
        [DisplayName("Workout Routine")]
        [XmlAttribute("workout-routine")]
        public string WorkoutRoutine
        {
            get { return _workoutRoutine; }
            set
            {
                _workoutRoutine = WorkoutRoutineSetCollection.Format(value.AssertNotNull());

                _sets = null;
            }
        }

        private string _workoutRoutine;

        #endregion

        #region Foreign Key Properties

        ///	<summary>
        ///	Gets / Sets the foreign key to 'routine_id'
        ///	</summary>
        [XmlIgnore]
        public WorkoutRoutine Routine { get; set; }

        ///	<summary>
        ///	Gets / Sets the foreign key to 'exercise_id'
        ///	</summary>
        [XmlIgnore]
        public Exercise Exercise
        {
            get
            {
                if (Routine == null || Routine.AllExercises == null)
                {
                    return null;
                }

                Exercise ex = null;

                Routine.AllExercises.TryGetByPrimaryKey(ExerciseId, out ex);

                return ex;
            }
        }

        /// <summary>
        ///
        /// </summary>
        /// <returns></returns>
        [XmlIgnore]
        public WorkoutRoutineSetCollection Sets
        {
            get
            {
                if (_sets == null)
                {
                    _sets = new WorkoutRoutineSetCollection(WorkoutRoutine);
                }
                return _sets;
            }
        }

        private WorkoutRoutineSetCollection _sets;

        #endregion
    }
}