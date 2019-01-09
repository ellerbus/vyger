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
    [XmlType("workout-routine")]
    [DebuggerDisplay("{DebuggerDisplay,nq}")]
    public class WorkoutRoutine
    {
        #region Constructors

        public WorkoutRoutine()
        {
            RoutineExercises = new WorkoutRoutineExerciseCollection(this);
            Plans = new WorkoutPlanCollection(this);
        }

        public WorkoutRoutine(WorkoutRoutine routine)
            : this()
        {
            Id = routine.Id;
            Name = routine.Name;
            Weeks = routine.Weeks;
            Days = routine.Days;
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
                string id = $"[{Id}]";

                string nm = $"[{Name}]";

                return "{0}, id={1}, nm={2}".FormatArgs(nameof(WorkoutRoutine), id, nm);
            }
        }

        #endregion

        #region Methods

        /// <summary>
        ///	Overlay all properties (except primary key, audits)
        /// </summary>
        public void OverlayWith(WorkoutRoutine other)
        {
            Name = other.Name;
            Weeks = other.Weeks;
            Days = other.Days;
        }

        #endregion

        #region Properties

        ///	<summary>
        ///
        ///	</summary>
        [Key]
        [DisplayName("ID")]
        [XmlAttribute("id")]
        public string Id
        {
            get { return _id; }
            set { _id = value.AssertNotNull().ToUpper(); }
        }

        private string _id;

        ///	<summary>
        ///
        ///	</summary>
        [DisplayName("Name")]
        [XmlAttribute("name")]
        public string Name { get; set; }

        ///	<summary>
        ///
        ///	</summary>
        [DisplayName("Weeks")]
        [XmlAttribute("weeks")]
        public int Weeks { get; set; }

        ///	<summary>
        ///
        ///	</summary>
        [DisplayName("Days")]
        [XmlAttribute("days")]
        public int Days { get; set; }

        #endregion

        #region Foreign Key Properties

        /// <summary>
        ///
        /// </summary>
        [XmlIgnore]
        public ExerciseCollection AllExercises { get; set; }

        /// <summary>
        ///
        /// </summary>
        [XmlArray("workout-routine-exercises"), XmlArrayItem("workout-routine-exercise")]
        public WorkoutRoutineExerciseCollection RoutineExercises { get; private set; }

        /// <summary>
        ///
        /// </summary>
        [XmlArray("workout-plans"), XmlArrayItem("workout-plan")]
        public WorkoutPlanCollection Plans { get; private set; }

        #endregion
    }
}