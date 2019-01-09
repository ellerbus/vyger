using System.ComponentModel.DataAnnotations;
using System.Diagnostics;
using Augment;
using Newtonsoft.Json;

namespace Vyger.Common.Models
{
    ///	<summary>
    ///
    ///	</summary>
    [DebuggerDisplay("{DebuggerDisplay,nq}")]
    public class Routine
    {
        #region Constructors

        public Routine()
        {
            Id = Generators.RoutineId();
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

                return "{0}, id={1}, nm={2}".FormatArgs(nameof(Routine), id, nm);
            }
        }

        #endregion

        #region Properties

        ///	<summary>
        ///
        ///	</summary>
        [Display(Name = "Id", Prompt = "Id")]
        [JsonProperty("id")]
        public string Id { get; set; }

        ///	<summary>
        ///
        ///	</summary>
        [Display(Name = "Name", Prompt = "Name")]
        [JsonProperty("name")]
        public string Name { get; set; }

        ///	<summary>
        ///
        ///	</summary>
        [Display(Name = "Weeks", Prompt = "Weeks")]
        [JsonProperty("weeks")]
        public int Weeks { get; set; }

        ///	<summary>
        ///
        ///	</summary>
        [Display(Name = "Days", Prompt = "Days")]
        [JsonProperty("days")]
        public int Days { get; set; }

        ///	<summary>
        ///
        ///	</summary>
        [Display(Name = "Default Sets", Prompt = "Default Sets")]
        [JsonProperty("sets")]
        public string[] Sets { get; set; }

        /////	<summary>
        /////
        /////	</summary>
        //[Display(Name = "Default Sets", Prompt = "Default Sets")]
        //[JsonIgnore()]
        //public string WorkoutPattern
        //{
        //    get { return Sets.Select(x => WorkoutSet.Format(x)).Join(", "); }
        //    set { Sets = WorkoutSet.ParsePatterns(value).ToArray(); }
        //}

        /// <summary>
        ///
        /// </summary>
        [JsonProperty("exercises")]
        public RoutineExerciseCollection Exercises { get; set; }

        ///// <summary>
        /////
        ///// </summary>
        //[XmlIgnore]
        //public ExerciseCollection AllExercises { get; set; }

        ///// <summary>
        /////
        ///// </summary>
        //[XmlArray("workout-routine-exercises"), XmlArrayItem("workout-routine-exercise")]
        //public RoutineExerciseCollection RoutineExercises { get; private set; }

        ///// <summary>
        /////
        ///// </summary>
        //[XmlArray("workout-plans"), XmlArrayItem("workout-plan")]
        //public PlanCollection Plans { get; private set; }

        #endregion
    }
}