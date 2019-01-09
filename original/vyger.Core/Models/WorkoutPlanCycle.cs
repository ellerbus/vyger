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
    [XmlRoot("workout-plan-exercise")]
    [DebuggerDisplay("{DebuggerDisplay,nq}")]
    public class WorkoutPlanCycle
    {
        #region Constructors

        public WorkoutPlanCycle()
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
                string id = $"[{CycleId}, {ExerciseId}]";

                string nm = $"[{Exercise?.Name}]";

                return "{0}, id={1}, nm={2}".FormatArgs("WorkoutPlanExercise", id, nm);
            }
        }

        #endregion

        #region Methods

        /// <summary>
        ///	Overlay all properties (except primary key, audits)
        /// </summary>
        public void OverlayWith(WorkoutPlanCycle other)
        {
            Weight = other.Weight;
            Reps = other.Reps;
            Pullback = other.Pullback;
        }

        #endregion

        #region Properties

        ///	<summary>
        ///
        ///	</summary>
        [Key]
        [DisplayName("Cycle Id")]
        [XmlAttribute("cycle-id")]
        public int CycleId { get; set; }

        ///	<summary>
        ///
        ///	</summary>
        [Key]
        [DisplayName("Exercise Id")]
        [XmlAttribute("exercise-id")]
        public string ExerciseId { get; set; }

        ///	<summary>
        ///	Gets / Sets database column 'weight'
        ///	</summary>
        [DisplayName("Weight")]
        [XmlAttribute("weight")]
        public int Weight { get; set; }

        ///	<summary>
        ///	Gets / Sets database column 'reps'
        ///	</summary>
        [DisplayName("Reps")]
        [XmlAttribute("reps")]
        public int Reps { get; set; }

        ///	<summary>
        ///	Gets / Sets database column 'pullback'
        ///	</summary>
        [DisplayName("Pullback")]
        [XmlAttribute("pullback")]
        public int Pullback { get; set; }

        ///	<summary>
        ///	Gets / Sets database column 'is_calculated'
        ///	</summary>
        [Required]
        [DisplayName("Is Calculated")]
        [XmlAttribute("is-calculated")]
        public bool IsCalculated { get; set; }

        /// <summary>
        ///
        /// </summary>
        [XmlIgnore]
        public double OneRepMax
        {
            get { return WorkoutCalculator.OneRepMax(Weight, Reps); }
        }

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

        #endregion
    }
}